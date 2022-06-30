using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lomba.API.Default;
using Lomba.API.Services;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Lomba.API.Tests
{
    public sealed class Initializer : IDisposable
    {
        private Contexts.DataContext _context;
        private IConfiguration _config;
        private bool _isDatabaseCleaned = false;

        private static Initializer? _init;
        public Initializer()
        {
            string? mergetest = Environment.GetEnvironmentVariable("MERGETEST");
            string env = string.IsNullOrWhiteSpace(mergetest) ? "" : "Merge";

            if (_config == null)
                _config = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile(@$"appsettings.{env}UnitTests.json", false, false)
                   .AddEnvironmentVariables()
                   .Build();

            if (_context == null)
            {
                _context = new Contexts.DataContext(_config);

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
            }
        }

        public static Initializer GetInitializer()
        {
            if (_init == null)
            {
                _init = new Initializer();
            }           

            return _init;
        }

        public void Dispose()
        {
            try
            {
                _context.Database.EnsureDeleted();
            }
            catch (Exception)
            { }
        }

        public IConfiguration Configuration
        {
            get { return _config; }
        }

        public Contexts.DataContext Context
        {
            get { return _context; }
        }
    }

    [CollectionDefinition("InitializerCollection")]
    public class InitializerCollection : ICollectionFixture<Initializer>
    {
    }
}
