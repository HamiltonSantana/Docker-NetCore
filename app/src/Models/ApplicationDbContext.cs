using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace ServerSide.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<WeatherForecast> Weather { get; set; }
        public DbSet<User> Users { get; set; }

        //private const string connectionString = @"Server=db,1433; Database=master; User ID=sa; Password=database_12345;";
        private const string connectionString = @"Server=DESKTOP-IARGUAA; Database=master; User ID=sa; Password=database_12345;";

        public ApplicationDbContext()
        {
           
        }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherForecast>()
                .Property(e => e.Id)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(e => e.Id)
                .IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
