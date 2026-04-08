using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Api.Infrastructure.Auth;
using Api.Domain.Entities;
using Api.Domain.Enums;

namespace Tests.Auth;

[TestFixture]
public class JwtTokenServiceTests
{
    private JwtTokenService _jwtService = null!;
    private ServiceProvider _provider = null!;

    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();

        // Mock IConfiguration with JWT settings
        var configValues = new System.Collections.Generic.Dictionary<string, string?>
        {
            { "Jwt:Key", "THIS_IS_A_SUPER_SECRET_KEY_12345" },
            { "Jwt:Issuer", "TestIssuer" },
            { "Jwt:Audience", "TestAudience" },
            { "Jwt:ExpiryMinutes", "60" }
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues)
            .Build();

        services.AddSingleton(configuration);

        // Register JwtTokenService
        services.AddScoped<JwtTokenService>();

        _provider = services.BuildServiceProvider();
        _jwtService = _provider.GetRequiredService<JwtTokenService>();
    }

    [TearDown]
    public void TearDown()
    {
        _provider?.Dispose();
    }

    private User GetTestUser()
    {
        return new User
        {
            Id = Guid.Parse("4d139cb2-7342-4d98-a32f-ab98cd2387fe"),
            Name = "John Doe",
            EmailAddress = "john@test.com",
            Role = UserRole.Admin
        };
    }

    [Test]
    public void GenerateToken_ShouldReturnToken()
    {
        var user = GetTestUser();

        string token = _jwtService.GenerateToken(user);

        Assert.IsNotNull(token);
        Assert.IsNotEmpty(token);

        TestContext.Progress.WriteLine($"Generated Token: {token}");
    }

    [Test]
    public void GenerateToken_ShouldContainUserId()
    {
        var user = GetTestUser();
        string token = _jwtService.GenerateToken(user);

        var jwtHandler = new JwtSecurityTokenHandler();
        var jwt = jwtHandler.ReadJwtToken(token);

        var userIdClaim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        Assert.IsNotNull(userIdClaim);
        Assert.AreEqual(user.Id.ToString(), userIdClaim.Value);
    }

    // Parameterized test for Admin and Customer roles
    [TestCase(UserRole.Admin)]
    [TestCase(UserRole.Customer)]
    public void GenerateToken_ShouldContainCorrectRole(UserRole role)
    {
        var user = GetTestUser();
        user.Role = role;

        string token = _jwtService.GenerateToken(user);

        var jwtHandler = new JwtSecurityTokenHandler();
        var jwt = jwtHandler.ReadJwtToken(token);

        var roleClaim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        Assert.IsNotNull(roleClaim);
        Assert.AreEqual(user.Role.ToString(), roleClaim.Value);
    }

    [Test]
    public void GenerateToken_ShouldHaveExpiration()
    {
        var user = GetTestUser();
        string token = _jwtService.GenerateToken(user);

        var jwtHandler = new JwtSecurityTokenHandler();
        var jwt = jwtHandler.ReadJwtToken(token);

        Assert.IsTrue(jwt.ValidTo > DateTime.UtcNow);
        Assert.IsTrue(jwt.ValidTo <= DateTime.UtcNow.AddMinutes(60));
    }
}