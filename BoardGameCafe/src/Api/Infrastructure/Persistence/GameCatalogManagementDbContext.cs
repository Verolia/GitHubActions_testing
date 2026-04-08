using Microsoft.EntityFrameworkCore;
using Api.Domain.Entities;

namespace Api.Infrastructure.Persistence.GameCatalogManagement
{
    public class GameCatalogManagementDbContext : DbContext
    {
        public GameCatalogManagementDbContext(DbContextOptions<GameCatalogManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<Game> Games => Set<Game>();
        public DbSet<GameCopy> GameCopies => Set<GameCopy>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Keys
            modelBuilder.Entity<Game>()
                .HasKey(g => g.Id);

            modelBuilder.Entity<GameCopy>()
                .HasKey(gc => gc.Id);

            // One-to-Many: Game -> GameCopy (VALID here)
            modelBuilder.Entity<GameCopy>()
                .HasOne(gc => gc.Game)
                .WithMany(g => g.GameCopies)
                .HasForeignKey(gc => gc.GameId);

            // Seed Games
            modelBuilder.Entity<Game>().HasData(
                new Game
                {
                    Id = Guid.Parse("d1bd1bfa-4d38-4d69-a68d-0fd1599095ef"),
                    Title = "Catan",
                    MaxPlayers = 4,
                    PlaytimeMin = 60,
                    Complexity = Domain.Enums.GameComplexity.Complex,
                    Description = "A game of trading and building."
                }
            );

            // Seed GameCopies
            modelBuilder.Entity<GameCopy>().HasData(
                new
                {
                    Id = Guid.Parse("b1b1b1b1-4d38-4d69-a68d-0fd1599095ef"),
                    GameId = Guid.Parse("d1bd1bfa-4d38-4d69-a68d-0fd1599095ef")
                }
            );
        }
    }
}
