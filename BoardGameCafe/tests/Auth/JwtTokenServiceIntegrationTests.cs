using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Api.Infrastructure.Auth;
using Api.Infrastructure.Persistence.AccountManagement;
using Api.Domain.Entities;
using Api.Domain.Enums; 

namespace Tests.Auth
{
    [TestFixture]
    public class JwtTokenServiceIntegrationTests
    {
        private ServiceProvider _provider = null!;
        private AccountManagementDbContext _dbContext = null!;
        private JwtTokenService _jwtTokenService = null!;

        [SetUp]
        public void Setup()
        {
            var inMemorySettings = new Dictionary<string, string?>
            {
                {"Jwt:Key", "ThisIsASecretKeyForTesting123456"},
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"},
                {"Jwt:ExpiryMinutes", "60"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings.ToList())
                .Build();

            var services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(configuration);
            services.AddDbContext<AccountManagementDbContext>(options =>
                options.UseInMemoryDatabase("TestDb"));
            services.AddTransient<JwtTokenService>();

            _provider = services.BuildServiceProvider();
            _dbContext = _provider.GetRequiredService<AccountManagementDbContext>();
            _jwtTokenService = _provider.GetRequiredService<JwtTokenService>();
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext?.Database.EnsureDeleted();
            _dbContext?.Dispose();
            _provider?.Dispose();
        }

        [Test]
        public void GenerateToken_ShouldReturn_ValidJwtToken()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                EmailAddress = "test@example.com",
                Role = UserRole.Customer
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            // Act
            var token = _jwtTokenService.GenerateToken(user);

            // Assert
            Assert.IsNotNull(token);

            // Decode JWT
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            // Validate claims
            var nameIdClaim = jwt.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
            Assert.IsNotNull(nameIdClaim);
            Assert.AreEqual(user.Id.ToString(), nameIdClaim!.Value);

            var emailClaim = jwt.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Name);
            Assert.IsNotNull(emailClaim);
            Assert.AreEqual(user.EmailAddress, emailClaim!.Value);

            var roleClaim = jwt.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role);
            Assert.IsNotNull(roleClaim);
            Assert.AreEqual(user.Role.ToString(), roleClaim!.Value);
        }
    }
}