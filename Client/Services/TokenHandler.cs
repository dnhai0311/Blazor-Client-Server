using Client.Services;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

public class TokenHandler : DelegatingHandler
{
    private readonly CircuitServicesAccessor ServicesAccessor;

    public TokenHandler(CircuitServicesAccessor servicesAccessor)
    {
        ServicesAccessor = servicesAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var localStorage = ServicesAccessor.Services?.GetService<ProtectedLocalStorage>();
        var authProvider = ServicesAccessor.Services?.GetService<CustomAuthenticationStateProvider>();

        if (localStorage != null && authProvider != null)
        {
            var token = (await localStorage.GetAsync<string>("authToken")).Value;

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }

    public async Task<string?> GetTokenAsync()
    {
        var localStorage = ServicesAccessor.Services?.GetService<ProtectedLocalStorage>();
        return localStorage != null ? (await localStorage.GetAsync<string>("authToken")).Value : null;
    }

    public async Task SaveTokenAsync(string token)
    {
        var localStorage = ServicesAccessor.Services?.GetService<ProtectedLocalStorage>();
        if (localStorage != null && !string.IsNullOrEmpty(token))
        {
            await localStorage.SetAsync("authToken", token);
        }
    }

    public async Task DeleteTokenAsync()
    {
        var localStorage = ServicesAccessor.Services?.GetService<ProtectedLocalStorage>();
        if (localStorage != null)
        {
            await localStorage.DeleteAsync("authToken");
        }
    }
}
