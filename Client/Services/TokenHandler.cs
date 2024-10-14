﻿using Blazored.LocalStorage;
using Client.Services;
using System.Net.Http.Headers;

public class TokenHandler : DelegatingHandler
{
    private readonly CircuitServicesAccessor _servicesAccessor;

    public TokenHandler(CircuitServicesAccessor servicesAccessor)
    {
        _servicesAccessor = servicesAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var localStorage = _servicesAccessor.Services?.GetService<ILocalStorageService>();

        if (localStorage != null)
        {
            var token = await localStorage.GetItemAsync<string>("authToken");

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }

    public async Task<string> GetTokenAsync()
    {
        var localStorage = _servicesAccessor.Services?.GetService<ILocalStorageService>();
        return localStorage != null ? await localStorage.GetItemAsync<string>("authToken") : null;
    }

    public async Task SaveTokenAsync(string token)
    {
        var localStorage = _servicesAccessor.Services?.GetService<ILocalStorageService>();
        if (localStorage != null && !string.IsNullOrEmpty(token))
        {
            await localStorage.SetItemAsync("authToken", token);
        }
    }

    public async Task DeleteTokenAsync()
    {
        var localStorage = _servicesAccessor.Services?.GetService<ILocalStorageService>();
        if (localStorage != null)
        {
            await localStorage.RemoveItemAsync("authToken");
        }
    }
}