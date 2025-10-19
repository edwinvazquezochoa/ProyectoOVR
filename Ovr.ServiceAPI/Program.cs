using GoNetPos.Ovr.ApiServices.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ovr.ClientServices.Implementations;
using Ovr.ClientServices.Intefaces;
using Ovr.Core.Infrastructures.Configs;
using Ovr.Core.Infrastructures.Loggers.Implementations;
using Ovr.Core.Infrastructures.Loggers.Interfaces;
using Ovr.Core.Infrastructures.Utils.Emails;
using Ovr.DaoServices.Implementations;
using Ovr.DaoServices.Interfaces;
using Ovr.DaoServices.Services;
using Ovr.Domain.Config;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Config
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

GlobalSettings.PathLog = Path.Combine(Directory.GetCurrentDirectory(), "Logs");

// JWT
var key = Encoding.ASCII.GetBytes(ObtenerValores.JwtSettingKey());

// CORS
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowFrontendClients", policy =>
//    {
//        policy.WithOrigins(
//            "https://ovrapp.com",
//            "https://www.ovrapp.com",
//            "https://localhost:7255"   // ← para pruebas locales del front
//        )
//        .AllowAnyHeader()
//        .AllowAnyMethod()
//        .AllowCredentials();
//    });
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()   // acepta cualquier origen
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ovr.ApiServices", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Ej: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// Auth
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // SmartASP sin SSL propio
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = ObtenerValores.JwtIssuer(),
        ValidAudience = ObtenerValores.JwtAudience(),
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.FromMinutes(1)
    };
});

// DI
builder.Services.AddScoped<SecurityUtils>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISaleNoteListService, SaleNoteListService>();
builder.Services.AddScoped<ISalesNoteService, SalesNoteService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IGenderService, GenderService>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEventLogger, EventLogger>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IDashBoardService, DashBoardService>();
builder.Services.AddScoped<IFrameService, FrameService>();
builder.Services.AddScoped<ILensesService, LensesService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<ILaboratoryService, LaboratoryService>();
builder.Services.AddScoped<IMenuPermissionService, MenuPermissionService>();

var app = builder.Build();

// Swagger también en producción
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ovr.ApiServices v1");
    c.RoutePrefix = "swagger";
});

// HTTPS solo en local (SmartASP usa HTTP si no tienes SSL)
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

//app.UseCors("AllowFrontendClients");

app.UseCors("AllowAll");


app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
