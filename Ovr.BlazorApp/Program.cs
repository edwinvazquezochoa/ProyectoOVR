using Blazored.SessionStorage;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Ovr.BlazorApp;
using Ovr.BlazorApp.Extensions;
using Ovr.BlazorApp.Helpers;
using Ovr.BlazorApp.Services.Implementations;
using Ovr.BlazorApp.Services.Intefaces;
using Ovr.BlazorApp.Services.Interfaces;
using Ovr.Client.Services;
using Ovr.Client.Services.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 👇 appsettings.json de wwwroot se carga automáticamente con CreateDefault
var configuration = builder.Configuration;

// ✅ Lee la URL de la API desde appsettings.json (Urls:ApiBaseUrl)
var apiBaseUrl = configuration["Urls:ApiBaseUrl"]
    ?? throw new InvalidOperationException("Falta 'Urls:ApiBaseUrl' en wwwroot/appsettings.json");

// 🔧 Token handler
builder.Services.AddTransient<TokenAuthorizationHandler>();

// 🔧 HttpClient nombrado con handler para token
builder.Services.AddHttpClient("OvrAPI", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<TokenAuthorizationHandler>();

// HttpClient principal inyectado
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("OvrAPI"));

// 🔧 Servicios personalizados
builder.Services.AddScoped<JwtTokenUtils>();
builder.Services.AddScoped<IApiHelper, ApiHelper>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IMenuPermissionService, MenuPermissionService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDashBoardService, DashBoardService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IFrameClientService, FrameClientService>();
builder.Services.AddScoped<IAuthGuardService, AuthGuardService>();
builder.Services.AddScoped<ILensesServices, LensesServices>();
builder.Services.AddScoped<IPersonsServices, PersonsServices>();
builder.Services.AddScoped<ILaboratoryService, LaboratoryService>();
builder.Services.AddScoped<ISaleNoteListService, SaleNoteListService>();
builder.Services.AddScoped<ISalesNoteService, SalesNoteService>();

// 🔐 Auth + Session
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped<AuthenticationStateProvider, AutenticacionExtension>();
builder.Services.AddScoped<AutenticacionExtension>();
builder.Services.AddAuthorizationCore();

// 🎨 SweetAlert2
builder.Services.AddSweetAlert2();

// 🚀 Run
await builder.Build().RunAsync();
