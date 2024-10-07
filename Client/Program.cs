using Shared.Repositories;
using Client.Repositories;
using Client.Components;
using Client.Services;

var builder = WebApplication.CreateBuilder(args);

var baseAddress = new Uri("https://localhost:7103");
builder.Services.AddHttpClient<IBookSaleRepository, BookSaleRepository>(httpClient => httpClient.BaseAddress = baseAddress);
builder.Services.AddHttpClient<IAuthorRepository, AuthorRepository>(httpClient => httpClient.BaseAddress = baseAddress);
builder.Services.AddHttpClient<IBillRepository, BillRepository>(httpClient => httpClient.BaseAddress = baseAddress);

builder.Services.AddScoped<AuthService>();


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

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
