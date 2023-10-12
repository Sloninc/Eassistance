using Microsoft.EntityFrameworkCore;
namespace Eassistance.Infrastructure
{
    public class DataContext : DbContext
    {
        DbConnectionOptions _dbConnection;
        public DbSet<Co> Warehouses { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<ChangesProducts> ChangesProducts { get; set; }
        public DbSet<Dishes> Dishes { get; set; }
        public DbSet<Recipes> Recipes { get; set; }
        public DbSet<Ingredients> Ingredients { get; set; }

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
