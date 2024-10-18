using Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Shared.Models;
using Shared.Repositories;
using System.Security.Claims;
using System.Text.Json;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HubService HubService;
    private readonly IUserClientRepository UserRepository;
    private readonly CircuitServicesAccessor ServicesAccessor;

    public CustomAuthenticationStateProvider(HubService hubService, IUserClientRepository userClientRepository, CircuitServicesAccessor servicesAccessor)
    {
        HubService = hubService;
        UserRepository = userClientRepository;
        ServicesAccessor = servicesAccessor;
    }

    private AuthenticationState AnonymousState()
    {
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var sessionState = await GetTokenAsync();

        if (string.IsNullOrEmpty(sessionState.Token))
        {
            return AnonymousState();
        }

        var claims = ParseClaimsFromJwt(sessionState.Token);
        var notiService = ServicesAccessor.Services?.GetService<NotificationService>();

        var currentId = await GetUserIdAsync();
        var currentRole = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (Int32.Parse(currentId) <= 0)
        {
            notiService?.ShowErrorMessage("User Id không tồn tại");
            return AnonymousState();
        }
        User user;
        try
        {
            user = await UserRepository.GetUserById(Int32.Parse(currentId));

        }
         catch
        {
            notiService?.ShowErrorMessage("Token không hợp lệ");
            await MarkUserAsLoggedOut();
            return AnonymousState();
        }

        if (user.Role?.RoleName != currentRole)
        {
            notiService?.ShowErrorMessage("Token không hợp lệ");
            await MarkUserAsLoggedOut();
            return AnonymousState();
        }

        await HubService.DisposeAsync();
        await HubService.Initialize();

        HubService.OnRoleChanged += async (userId) =>
        {
            if (currentId == userId)
            {
                notiService?.ShowErrorMessage("Ai đó vừa thay đổi quyền của bạn");
                await MarkUserAsLoggedOut();
            }
        };

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
    }


    public async Task MarkUserAsAuthenticated(LoginResult model)
    {
        await SaveTokenAsync(model);
        var claims = ParseClaimsFromJwt(model.Token);
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(authenticatedUser)));
    }

    public async Task MarkUserAsLoggedOut()
    {
        await HubService.DisposeAsync();
        HubService.messages.Clear();
        await DeleteTokenAsync();
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
        var sessionState = await GetTokenAsync();
        if (string.IsNullOrEmpty(sessionState.Token))
        {
            return "0";
        }
        var claims = ParseClaimsFromJwt(sessionState.Token);
        var userIdClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        return userIdClaim?.Value ?? "0";
    }

    public async Task<LoginResult> GetTokenAsync()
    {
        var localStorage = ServicesAccessor.Services?.GetService<ProtectedLocalStorage>();
        if (localStorage != null)
        {
            var sessionState = (await localStorage.GetAsync<LoginResult>("sessionState")).Value;
            return sessionState ?? new LoginResult();
        }
        return new LoginResult();
    }

    public async Task SaveTokenAsync(LoginResult model)
    {
        var localStorage = ServicesAccessor.Services?.GetService<ProtectedLocalStorage>();
        if (localStorage != null && model.Successful)
        {
            await localStorage.SetAsync("sessionState", model);
        }
    }

    public async Task DeleteTokenAsync()
    {
        var localStorage = ServicesAccessor.Services?.GetService<ProtectedLocalStorage>();
        if (localStorage != null)
        {
            await localStorage.DeleteAsync("sessionState");
        }
    }

    public async Task<int> IsTokenExpired()
    {
        var sessionState = await GetTokenAsync();
        if (!string.IsNullOrEmpty(sessionState.Token))
        {
            var claims = ParseClaimsFromJwt(sessionState.Token);
            var expiry = long.Parse(claims.FirstOrDefault(c => c.Type == "exp")?.Value!);
            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // Token đã hết hạn
            if (expiry < currentTime)
            {
                return 0;
            }

            // Token còn hạn nhưng dưới 5 phút
            if (expiry - currentTime <= 5 * 60)
            {
                return 1;
            }

            // Token còn hạn và trên 5 phút
            return 2;
        }

        // Nếu không có token
        return 0;
    }

}