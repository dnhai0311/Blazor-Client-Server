using Shared.Repositories;
using Client.Repositories;
using Client.Components;
using Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthentication("CustomScheme")
    .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>("CustomScheme", null);
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());

var baseAddress = new Uri("https://localhost:7103");
builder.Services.AddHttpClient<IBookSaleRepository, BookSaleRepository>(httpClient => httpClient.BaseAddress = baseAddress);
builder.Services.AddHttpClient<IAuthorRepository, AuthorRepository>(httpClient => httpClient.BaseAddress = baseAddress);
builder.Services.AddHttpClient<IBillRepository, BillRepository>(httpClient => httpClient.BaseAddress = baseAddress);
builder.Services.AddHttpClient<IUserClientRepository, UserRepository>(httpClient => httpClient.BaseAddress = baseAddress);
builder.Services.AddHttpClient<IRoleRepository, RoleRepository>(httpClient => httpClient.BaseAddress = baseAddress);
builder.Services.AddHttpClient<IAuthService, AuthService>(httpClient => httpClient.BaseAddress = baseAddress);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    await next.Invoke();
    if (context.Response.StatusCode == 404)
    {
        context.Response.Redirect("/not-found");
    }
    else if (context.Response.StatusCode == 403)
    {
        context.Response.Redirect("/access-denied");
    }
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
