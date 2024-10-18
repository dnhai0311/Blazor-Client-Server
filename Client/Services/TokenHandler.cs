using Client.Services;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity.Data;
using Shared.Models;

public class TokenHandler : DelegatingHandler
{
    private readonly CircuitServicesAccessor ServicesAccessor;
    private readonly HttpClient HttpClient;

    public TokenHandler(CircuitServicesAccessor servicesAccessor, HttpClient httpClient)
    {
        ServicesAccessor = servicesAccessor;
        HttpClient = httpClient;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var localStorage = ServicesAccessor.Services?.GetService<ProtectedLocalStorage>();
        var authProvider = ServicesAccessor.Services?.GetService<CustomAuthenticationStateProvider>();

        if (localStorage != null && authProvider != null)
        {
            var sessionState = await authProvider.GetTokenAsync();

            if (!string.IsNullOrEmpty(sessionState.Token))
            {
                int stateToken = await authProvider.IsTokenExpired();
                if(stateToken == 0)
                {
                    await authProvider.MarkUserAsLoggedOut();
                } else if(stateToken == 1)
                {
                    var response = await HttpClient.GetFromJsonAsync<LoginResult>($"/api/auth/refresh-token?refreshToken={sessionState.RefreshToken}");
                    if(response != null && response.Successful)
                    {
                        await authProvider.MarkUserAsAuthenticated(response);
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", response.Token);
                    } else
                    {
                        await authProvider.MarkUserAsLoggedOut();
                    }
                }
                else if (stateToken == 2)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", sessionState.Token);
                }
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }

    
}
