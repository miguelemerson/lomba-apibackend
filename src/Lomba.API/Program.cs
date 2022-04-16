using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Lomba.API.Contexts;
using Lomba.API.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrgaService, OrgaService>();
builder.Services.AddScoped<IRoleService, RoleService>();

var app = builder.Build();

Console.WriteLine($"Enviroment: {app.Environment.EnvironmentName}");

//Aplica la migración de la base de datos.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetService<DataContext>();
    dbContext?.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
