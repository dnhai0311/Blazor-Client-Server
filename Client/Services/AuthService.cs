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

    public async Task<RegisterResult> Register(RegisterRequest registerRequest)
    {
        var response = await HttpClient.PostAsJsonAsync("api/account", registerRequest);
        var registerResult = await response.Content.ReadFromJsonAsync<RegisterResult>();

        return registerResult!;
    }

    public async Task<LoginResult> Login(LoginRequest loginRequest)
    {
        var response = await HttpClient.PostAsJsonAsync("api/auth", loginRequest);
        var loginResult = await response.Content.ReadFromJsonAsync<LoginResult>();

        if (!response.IsSuccessStatusCode)
        {
            return loginResult!;
        }
        await LocalStorage.SetItemAsync(TokenKey, loginResult!.Token);
        ((MyAuthenticationStateProvider)AuthenticationStateProvider).MarkUserAsAuthenticated(loginRequest.UserName);
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Token);
        return loginResult;
    }

    public async Task Logout()
    {
        await RemoveToken();
        ((MyAuthenticationStateProvider)AuthenticationStateProvider).MarkUserAsLoggedOut();
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
