using Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Models;
using System.Security.Claims;
using System.Text.Json;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HubService HubService;
    private readonly TokenHandler TokenHandler;

    public CustomAuthenticationStateProvider(HubService hubService, TokenHandler tokenHandler)
    {
        HubService = hubService;
        TokenHandler = tokenHandler;
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var savedToken = await TokenHandler.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(savedToken))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var claims = ParseClaimsFromJwt(savedToken);

        await HubService.DisposeAsync();
        await HubService.Initialize();

        var currentId = await GetUserIdAsync();
        HubService.OnRoleChanged += async (userId) =>
        {
            if (currentId == userId)
            {
                await MarkUserAsLoggedOut();
            }
        };

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
    }


    public async Task MarkUserAsAuthenticated(string token)
    {
        await TokenHandler.SaveTokenAsync(token);
        var claims = ParseClaimsFromJwt(token);
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(authenticatedUser)));
    }

    public async Task MarkUserAsLoggedOut()
    {
        await TokenHandler.DeleteTokenAsync();
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousUser)));
    }


    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();

        var parts = jwt.Split('.');
        if (parts.Length != 3)
        {
            throw new ApplicationException("JWT does not have the correct format.");
        }

        var payload = parts[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

        if (roles != null)
        {
            if (roles.ToString().Trim().StartsWith("["))
            {
                var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                foreach (var parsedRole in parsedRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                }
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
            }

            keyValuePairs.Remove(ClaimTypes.Role);
        }

        claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
        return claims;
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }

    public async Task<string> GetUserIdAsync()
    {
        var savedToken = await TokenHandler.GetTokenAsync();
        if (string.IsNullOrWhiteSpace(savedToken))
        {
            return null;
        }

        var claims = ParseClaimsFromJwt(savedToken);
        var userIdClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        return userIdClaim?.Value;
    }
}
