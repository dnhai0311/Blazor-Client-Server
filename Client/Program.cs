using Shared.Repositories;
using Client.Repositories;
using Client.Components;
using Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());

var baseAddress = new Uri("https://localhost:7103");
builder.Services.AddHttpClient<IBookSaleRepository, BookSaleRepository>(httpClient => httpClient.BaseAddress = baseAddress);
builder.Services.AddHttpClient<IAuthorRepository, AuthorRepository>(httpClient => httpClient.BaseAddress = baseAddress);
builder.Services.AddHttpClient<IBillRepository, BillRepository>(httpClient => httpClient.BaseAddress = baseAddress);
builder.Services.AddHttpClient<IUserClientRepository, UserRepository>(httpClient => httpClient.BaseAddress = baseAddress);
builder.Services.AddHttpClient<IAuthService, AuthService>(httpClient => httpClient.BaseAddress = baseAddress);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
