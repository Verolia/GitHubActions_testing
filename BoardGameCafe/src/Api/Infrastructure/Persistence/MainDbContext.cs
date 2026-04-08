using Microsoft.EntityFrameworkCore;
using Api.Domain.Entities;

namespace Api.Infrastructure.Persistence
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameCopy> GameCopies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define Keys here

            //Key definitions for GameCopy
            modelBuilder.Entity<GameCopy>().HasKey(gc => gc.Id); // Defines the primary key
            modelBuilder.Entity<GameCopy>().HasOne(g => g.Game).WithMany(gc => gc.GameCopies).HasForeignKey(gc => gc.GameId);

            // Seed data
            // Seeds the Games Table
            modelBuilder.Entity<Game>().HasData(
                new Game {Id = Guid.Parse("d1bd1bfa-4d38-4d69-a68d-0fd1599095ef"),Title = "Catan", MaxPlayers = 4, PlaytimeMin = 60, Complexity = Domain.Enums.GameComplexity.Complex, Description = "A game of trading and building."}
            );

            // Seeds the GameCopies Table
            modelBuilder.Entity<GameCopy>().HasData(
                new GameCopy {Id = Guid.Parse("b1b1b1b1-4d38-4d69-a68d-0fd1599095ef"), GameId = Guid.Parse("d1bd1bfa-4d38-4d69-a68d-0fd1599095ef")} // GameCopy of Catan seed
            );

            // Seeds users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Admin User",
                    EmailAddress = "admin@test.com",
                    PasswordHash = "admin",
                    Role = Domain.Enums.UserRole.Admin
                },
                new User
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Customer User",
                    EmailAddress = "customer@test.com",
                    PasswordHash = "customer",
                    Role = Domain.Enums.UserRole.Customer
                },
                new User
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Steward User",
                    EmailAddress = "steward@test.com",
                    PasswordHash = "steward",
                    Role = Domain.Enums.UserRole.Steward
                }
            );
        }
    }
}