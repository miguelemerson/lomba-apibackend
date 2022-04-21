using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Lomba.API.Contexts;
using Lomba.API.Services;
using Lomba.API;

var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder);
var app = builder.Build();
startup.Configure(app);
app.Run();