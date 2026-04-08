using Microsoft.EntityFrameworkCore;
using Api.Features.AccountManagementContext.Entities;
using Api.Features.AccountManagementContext.Enums;
using System;

namespace Api.Infrastructure.Persistence.AccountManagement
{
    public class AccountManagementDbContext : DbContext
    {
        public AccountManagementDbContext(DbContextOptions<AccountManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                // Primary key
                entity.HasKey(u => u.Id);

                // Unique index on EmailAddress
                entity.HasIndex(u => u.EmailAddress)
                      .IsUnique();

                // Seed data
                entity.HasData(
                    new User
                    {
                        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Name = "Admin User",
                        EmailAddress = "admin@test.com",
                        PasswordHash = "admin",
                        Role = UserRole.Admin,
                        IsActive = true
                    },
                    new User
                    {
                        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        Name = "Customer User",
                        EmailAddress = "customer@test.com",
                        PasswordHash = "customer",
                        Role = UserRole.Customer,
                        IsActive = true
                    },
                    new User
                    {
                        Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                        Name = "Steward User",
                        EmailAddress = "steward@test.com",
                        PasswordHash = "steward",
                        Role = UserRole.Steward,
                        IsActive = true
                    }
                );
            });
        }
    }
}