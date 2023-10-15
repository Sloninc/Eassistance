using Microsoft.EntityFrameworkCore;
using Eassistance.Domain;
namespace Eassistance.Infrastructure
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<EAUser> Users { get; set; }
    }
}
