using Microsoft.EntityFrameworkCore;
using Eassistance.Domain;
namespace Eassistance.Infrastructure
{
    public class DataContext : DbContext
    {
        DbConnectionOptions _dbConnection;
        public DbSet<Equipments> Equipments { get; set; }
        public DbSet<Operations> Operations { get; set; }
        public DbSet<Units> Units { get; set; }
        public DbSet<Users> Users { get; set; }
        public DataContext(IConfiguration configuration)
        {
            _dbConnection = configuration.GetSection(DbConnectionOptions.KeyValue).Get<DbConnectionOptions>();

            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_dbConnection.ConnectionString);
        }
    }
}
