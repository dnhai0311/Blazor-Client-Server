using Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Repositories;
using System.Security.Claims;
using System.Text.Json;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HubService HubService;
    private readonly TokenHandler TokenHandler;
    private readonly IUserClientRepository UserRepository;

    public CustomAuthenticationStateProvider(HubService hubService, TokenHandler tokenHandler, IUserClientRepository userClientRepository)
    {
        HubService = hubService;
        TokenHandler = tokenHandler;
        UserRepository = userClientRepository;
    }

    private AuthenticationState AnonymousState()
    {
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var savedToken = await TokenHandler.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(savedToken))
        {
            return AnonymousState();
        }


        var claims = ParseClaimsFromJwt(savedToken);

        var currentId = await GetUserIdAsync();
        var currentRole = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (Int32.Parse(currentId) <= 0)
        {
            return AnonymousState();
        }

        var user = await UserRepository.GetUserById(Int32.Parse(currentId));

        if (user.Role?.RoleName != currentRole)
        {
            await MarkUserAsLoggedOut();
            return AnonymousState();
        }

        await HubService.DisposeAsync();
        await HubService.Initialize();

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
        await HubService.DisposeAsync();
        HubService.messages.Clear();
        await TokenHandler.DeleteTokenAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(AnonymousState()));
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
            return "0";
        }
        var claims = ParseClaimsFromJwt(savedToken);
        var userIdClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        return userIdClaim?.Value ?? "0";
    }
}
