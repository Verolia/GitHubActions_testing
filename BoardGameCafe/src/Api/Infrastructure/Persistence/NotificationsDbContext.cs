using Microsoft.EntityFrameworkCore;
// using Api.Features.YourFeatureNamespace; // Import your feature namespaces here

namespace Api.Infrastructure.Persistence.Notifications
{
    public class NotificationsDbContext : DbContext
    {
        public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options)
            : base(options)
        {
        }

        // public DbSet<YourEntity> YourTableName => Set<YourEntity>();  - This is how you define a DbSet for your entities. Replace YourEntity and YourTableName with your actual entity and table names.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Defining Keys
            // modelBuilder.Entity<YourEntity>().HasKey(e => e.Id); - This is how you define a primary key for your entity. Replace YourEntity and Id with your actual entity and primary key property.    

            // Seed YourEntity
            // modelBuilder.Entity<YourEntity>().HasData(
            //     new YourEntity
            //     {
            //         Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            //     }
            // );
        }
    }
}