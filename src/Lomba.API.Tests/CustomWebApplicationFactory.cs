using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lomba.API.Contexts;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.DependencyInjection;

namespace Lomba.API.Tests
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup>, IDisposable where TStartup : class
    {
        private bool _isDatabaseCleaned = false;
        private DataContext? _context;
        public new void Dispose()
        {
            try
            {
                _context?.Database.EnsureDeleted();
            }
            catch (Exception)
            { }
            base.Dispose();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            string? mergetest = Environment.GetEnvironmentVariable("MERGETEST");
            string env = string.IsNullOrWhiteSpace(mergetest) ? "" : "Merge";

            IConfiguration _config = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile(@$"appsettings.{env}IntegrationTests.json", false, false)
                   .AddEnvironmentVariables()
                   .Build();
            
            _context = new Contexts.DataContext(_config);

            builder.UseConfiguration(_config);

            if (!_isDatabaseCleaned)
            {
                _isDatabaseCleaned = true;
                try
                {
                    _context.Database.EnsureDeleted();
                    _context.Database.Migrate();
                }
                catch (Exception)
                { }
            }

            builder.ConfigureServices(services =>
            {
                //// Remove the app's ApplicationDbContext registration.
                //var descriptor = services.SingleOrDefault(
                //    d => d.ServiceType ==
                //        typeof(DbContextOptions<DataContext>));

                ////Se remueve la conexión actual de base de datos para
                ////más abajo incluir la base de Test configurada
                //if (descriptor != null)
                //{
                //    services.Remove(descriptor);
                //}

                //descriptor = services.SingleOrDefault(
                //    d => d.ServiceType ==
                //        typeof(DbContextOptions<DataContext>));

                //services.AddDbContext<DataContext>(options => {
                //    options.UseSqlServer(connectionString: _config.GetConnectionString("SQLServerDatabase"),
                //         options => options.EnableRetryOnFailure());
                //    }
                //);

                var sp = services.BuildServiceProvider();
            });
        }
    }
}
