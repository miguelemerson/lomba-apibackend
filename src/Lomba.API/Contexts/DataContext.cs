using Microsoft.EntityFrameworkCore;
using Lomba.API.Models;

namespace Lomba.API.Contexts
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            Console.WriteLine($"Conn: {Configuration.GetConnectionString("SQLServerDatabase")}");

            options.UseSqlServer(connectionString: Configuration.GetConnectionString("SQLServerDatabase"));
        }

        public DbSet<User>? Users { get; set; }
    }
}

