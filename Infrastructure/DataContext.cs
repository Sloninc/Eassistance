using Microsoft.EntityFrameworkCore;
using Eassistance.Domain;
namespace Eassistance.Infrastructure
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        public DbSet<Equipments> Equipments { get; set; }
        public DbSet<Operations> Operations { get; set; }
        public DbSet<Units> Units { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Steps> Steps { get; set; }

    }
}
