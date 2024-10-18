using Shared.Repositories;
using Client.Repositories;
using Client.Components;
using Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Server.Circuits;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();

builder.Services.AddScoped<NotificationService>();

builder.Services.AddScoped<HubService>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthentication("CustomScheme")
    .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>("CustomScheme", null);
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<CircuitServicesAccessor>();
builder.Services.AddScoped<CircuitHandler, ServicesAccessorCircuitHandler>();
builder.Services.AddTransient<TokenHandler>();

builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());



var baseAddress = new Uri("https://localhost:7103");

void AddHttpClients<TInterface, TImplementation>()
    where TImplementation : class, TInterface
    where TInterface : class
{
    builder.Services.AddHttpClient<TInterface, TImplementation>(httpClient =>
    {
        httpClient.BaseAddress = baseAddress;
    }).AddHttpMessageHandler<TokenHandler>();
}

AddHttpClients<IBookSaleRepository, BookSaleRepository>();
AddHttpClients<IAuthorRepository, AuthorRepository>();
AddHttpClients<IBillRepository, BillRepository>();
AddHttpClients<IUserClientRepository, UserRepository>();
AddHttpClients<IRoleRepository, RoleRepository>();
AddHttpClients<IAuthService, AuthService>();


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
