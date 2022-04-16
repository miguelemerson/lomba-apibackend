using Lomba.API.ViewModels;
using Lomba.API.Models;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;


namespace Lomba.API.Services
{
    public class RoleService : IRoleService
    {
        private readonly Contexts.DataContext _db;
        private readonly IConfiguration _config;

        public RoleService(Contexts.DataContext dataContext,
            IConfiguration configuration)
        {
            _db = dataContext;
            _config = configuration;
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            return await _db.Set<Role>().AsNoTracking().ToListAsync();
        }
        public async Task<Role> GetRoleByNameAsync(string name)
        {
            return await _db.Set<Role>().AsNoTracking()
                    .SingleOrDefaultAsync(x =>
                        x.Name == name);
        }
        public async Task<Role> SetEnableAsync(string name, bool setToDisable = false)
        {
            var role = await _db.Set<Role>()
            .SingleOrDefaultAsync(x =>
                    x.Name == name);

            if (role.IsDisabled != setToDisable)
            {
                role.IsDisabled = setToDisable;
                role.UpdatedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }

            return role;
        }
    }

    public interface IRoleService
    {
        Task<List<Role>> GetRolesAsync();
        Task<Role> SetEnableAsync(string name, bool setToDisable = false);
        Task<Role> GetRoleByNameAsync(string name);
    }
}
