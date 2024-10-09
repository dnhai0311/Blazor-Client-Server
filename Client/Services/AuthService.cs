using Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using System.Net.Http.Headers;
using Shared.Models;

public class AuthService : IAuthService
{
    private readonly HttpClient HttpClient;
    private readonly ILocalStorageService LocalStorage;
    private readonly AuthenticationStateProvider AuthenticationStateProvider;
    private const string TokenKey = "authToken";

    public AuthService(HttpClient httpClient,
                       ILocalStorageService localStorage,
                       AuthenticationStateProvider authenticationStateProvider)
    {
        HttpClient = httpClient;
        LocalStorage = localStorage;
        AuthenticationStateProvider = authenticationStateProvider;
    }

    public async Task Login(LoginRequest loginRequest)
    {
        var response = await HttpClient.PostAsJsonAsync("api/auth/login", loginRequest);

        var loginResult = await response.Content.ReadFromJsonAsync<LoginResult>();

        if (response.IsSuccessStatusCode && loginResult != null && loginResult.Successful)
        {
            await SaveToken(loginResult.Token);
            ((CustomAuthenticationStateProvider)AuthenticationStateProvider).MarkUserAsAuthenticated(loginResult.Token);
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.Token);
        }
        else
        {
            throw new ApplicationException(loginResult?.Error);
        }
    }

    public async Task Logout()
    {
        await  RemoveToken();
        ((CustomAuthenticationStateProvider)AuthenticationStateProvider).MarkUserAsLoggedOut();
        HttpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task SaveToken(string token)
    {
        await LocalStorage.SetItemAsync(TokenKey, token);
    }

    public async Task RemoveToken()
    {
        await LocalStorage.RemoveItemAsync(TokenKey);
    }
}
