using Microsoft.EntityFrameworkCore;
using Api.Infrastructure.Persistence;
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

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------------------
// Add DbContext (PostgreSQL)
// ------------------------------------------------------
builder.Services.AddDbContext<MainDbContext>(options =>
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
}

builder.Services.AddAuthorization();




var app = builder.Build();

// ------------------------------------------------------
// Apply migrations automatically (optional but great in dev)
// ------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MainDbContext>();
    db.Database.Migrate();
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