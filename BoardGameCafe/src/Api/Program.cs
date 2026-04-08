using Microsoft.EntityFrameworkCore;
using MediatR;
using FluentValidation;
using FluentValidation.AspNetCore;
using Api.Infrastructure.Behaviors;
using Api;
using Microsoft.AspNetCore.Authentication;
using Api.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Api.Features.AccountManagementContext.Shared.Services;
using Api.Infrastructure.Persistence.AccountManagement;
using Api.Infrastructure.Persistence.GameCatalogManagement;
using Api.Infrastructure.Persistence.CafeFloorManagement;
using Api.Infrastructure.Persistence.ContentAndPromotions;
using Api.Infrastructure.Persistence.EconomyManagement;
using Api.Infrastructure.Persistence.Notifications;
using Api.Infrastructure.Persistence.Report;
using Api.Infrastructure.Persistence.ReservationManagement;

var builder = WebApplication.CreateBuilder(args);


// ------------------------------------------------------
// Add DbContext (PostgreSQL)
// Needs one service registration per DbContext, even if they use the same connection string
// ------------------------------------------------------

// DbContexts for MainDb Database
builder.Services.AddDbContext<AccountManagementDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MainDbConnection"))

);builder.Services.AddDbContext<CafeFloorManagementDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MainDbConnection"))

);builder.Services.AddDbContext<ContentAndPromotionsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MainDbConnection"))

);builder.Services.AddDbContext<EconomyManagementDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MainDbConnection"))

);builder.Services.AddDbContext<GameCatalogManagementDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MainDbConnection"))

);builder.Services.AddDbContext<NotificationsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MainDbConnection"))

);builder.Services.AddDbContext<ReportDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MainDbConnection"))

);builder.Services.AddDbContext<ReservationManagementDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MainDbConnection"))
);

// ------------------------------------------------------
// MediatR
// ------------------------------------------------------
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<ApiAssemblyMarker>();
});

// ------------------------------------------------------
// FluentValidation
// ------------------------------------------------------
builder.Services
    .AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<ApiAssemblyMarker>();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// ------------------------------------------------------
// Controllers + Swagger
// ------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });


// ------------------------------------------------------
// Optional: CORS (recommended for React front-end)
// ------------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// ------------------------------------------------------
// Authentication & Authorization (Dummy)
// ------------------------------------------------------
var useDummyAuth = builder.Configuration.GetValue<bool>("UseDummyAuth");

if (useDummyAuth)
{
    builder.Services
        .AddAuthentication("Dummy")
        .AddScheme<AuthenticationSchemeOptions, DummyAuthHandler>("Dummy", null);
}
else
{
    var jwtKey = builder.Configuration["Jwt:Key"];

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey!)
            )
        };
    });

    builder.Services.AddScoped<JwtTokenService>();
    builder.Services.AddScoped<LoginService>();
}

builder.Services.AddAuthorization();




var app = builder.Build();

// ------------------------------------------------------
// Apply migrations automatically (optional but great in dev)
// ------------------------------------------------------

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Needs one service per DbContext

    // DbContexts for MainDb Database
    services.GetRequiredService<AccountManagementDbContext>()
        .Database.Migrate();

    services.GetRequiredService<CafeFloorManagementDbContext>()
        .Database.Migrate();

    services.GetRequiredService<ContentAndPromotionsDbContext>()
    .Database.Migrate();

    services.GetRequiredService<EconomyManagementDbContext>()
    .Database.Migrate();

    services.GetRequiredService<GameCatalogManagementDbContext>()
        .Database.Migrate();

    services.GetRequiredService<NotificationsDbContext>()
        .Database.Migrate();

    services.GetRequiredService<ReportDbContext>()
        .Database.Migrate();

    services.GetRequiredService<ReservationManagementDbContext>()
        .Database.Migrate();
}


// ------------------------------------------------------
// Middleware pipeline
// ------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapGet("/health", () => Results.Ok("Healthy")); // Health check route

app.MapControllers();

app.Run();