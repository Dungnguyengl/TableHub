using Core.Extentions;
using Domain.Entities;
using Infrastructure.Database.EntityConfigurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    public class TableHubDbContext(DbContextOptions<TableHubDbContext> options) : IdentityDbContext<User>(options)
    {
        public string Schema { get; set; } = "dbo";

        public DbSet<Table> Tables { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(Schema);
            AddConfiguration(modelBuilder);
            ConfigCollumnName(modelBuilder);
        }

        private static void AddConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EntityTypeConfigurationBase<>).Assembly);
        }

        private static void ConfigCollumnName(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    string propertyName = property.Name;
                    if (propertyName != "Key")
                    {
                        property.SetColumnName(propertyName.ToSnakeCase());
                    }

                    switch (property.GetType())
                    {
                        case Type clrType when clrType == typeof(decimal):
                            property.SetColumnType("decimal(18, 2)");
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
