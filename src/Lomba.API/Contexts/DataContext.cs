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
            options.UseSqlServer(connectionString: Configuration.GetConnectionString("SQLServerDatabase"),
                options => options.EnableRetryOnFailure());
        }

        public DbSet<User>? Users { get; set; }
        public DbSet<Orga>? Orgas { get; set; }
        public DbSet<OrgaUser>? OrgasUsers { get; set; }
        public DbSet<Role>? Roles{ get; set; }


    }
}

