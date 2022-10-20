using Microsoft.EntityFrameworkCore;
using WebApp.Data.Entities.Classes;
using WebApp.Data.Entities.Interfaces;

namespace WebApp.Data.DbContexts
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<User>? Users { get; set; }
        public DbSet<Role>? Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(x =>
            {
                x.ToTable("Users");
                x.HasKey(x => x.Id);
                x.Property(x => x.Name).IsRequired();
                x.Property(x => x.Surname).IsRequired();
                x.Property(x => x.Email).IsRequired();
                x.HasIndex(x => x.Email).IsUnique();
                x.Property(x => x.PasswordHash).IsRequired();
                x.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId);
            });

            builder.Entity<Role>(x =>
            {
                x.ToTable("Roles");
                x.HasKey(x => x.Id);
                x.Property(x => x.Name).IsRequired();
                x.HasIndex(x => x.Name).IsUnique();
            });

            base.OnModelCreating(builder);
        }

        public async Task<int> SaveChangesAsync()
        {
            this.ChangeTracker.DetectChanges();
            var entities = this.ChangeTracker.Entries()
                        .Where(t => t.State == EntityState.Added || t.State == EntityState.Modified)
                        .Select(t => new { t.State, t.Entity })
                        .ToArray();

            foreach (var stateEntity in entities)
            {
                if (stateEntity.Entity is ITrack entity)
                {
                    DateTime dateTime = DateTime.Now;

                    if (stateEntity.State == EntityState.Added)
                        entity.CreatedDate = dateTime;

                    entity.UpdatedDate = dateTime;
                }
            }

            return await base.SaveChangesAsync();
        }
    }
}
