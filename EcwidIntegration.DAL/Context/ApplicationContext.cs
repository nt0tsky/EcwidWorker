
using EcwidIntegration.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace EcwidIntegration.DAL.Context
{
    class ApplicationContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=orders;Username=postgres;Password=postgres");
        }
    }
}
