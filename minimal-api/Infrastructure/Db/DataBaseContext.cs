using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using minimal_api.Domain.Entities;

namespace minimal_api.Infrastructure.Db
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(IConfiguration configAppSettings)
        {
            _configAppSettings = configAppSettings;
        }
        public DbSet<Admin> Adimins { get; set; } = default!;

        private readonly IConfiguration _configAppSettings;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().HasData(
                new Admin {
                    Id = 1,
                    Email = "adm@adm.com",
                    Password = "123",
                    Rule = "adm"
                }

            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) {
                var stringConnection = _configAppSettings.GetConnectionString("mysql")?.ToString();
                if (!string.IsNullOrEmpty(stringConnection)) {
                    optionsBuilder.UseMySql(stringConnection, 
                    ServerVersion.AutoDetect(stringConnection));
                }
            }
        }
    }
}