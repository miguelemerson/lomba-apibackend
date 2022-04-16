using Lomba.API.ViewModels;
using Lomba.API.Models;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Lomba.API.Services
{
    public class OrgaService : IOrgaService
    {
        private readonly Contexts.DataContext _db;
        private readonly IConfiguration _config;

        public OrgaService(Contexts.DataContext dataContext,
            IConfiguration configuration)
        {
            _db = dataContext;
            _config = configuration;
        }
        public async Task<Orga> GetOrgaByIdAsync(Guid Id)
        {
            return await _db.Set<Orga>().AsNoTracking()
                    .SingleOrDefaultAsync(x =>
                        x.Id == Id);
        }

        public async Task<List<Orga>> GetOrgasAsync()
        {
            return await _db.Set<Orga>().AsNoTracking().ToListAsync();
        }

        public async Task<List<OrgaUser>> GetUsersByOrgaIdAsync(Guid Id)
        {
            return await _db.Set<OrgaUser>().AsNoTracking()
                .Include(u=>u.User)
                .Include(o=>o.Orga)
                .Include(r=>r.Roles)
                .Where(o => o.Orga.Id == Id).ToListAsync();
        }

        public async Task<List<OrgaUser>> RemoveUserAsync(Guid Id, Guid userId)
        {
            var orgauser = await _db.Set<OrgaUser>().SingleOrDefaultAsync(x => x.Orga.Id == Id && x.User.Id == userId);
            _ = _db.Remove<OrgaUser>(orgauser);
            await _db.SaveChangesAsync();
            return await this.GetUsersByOrgaIdAsync(Id);
        }

        public async Task<Orga> SetEnableAsync(Guid Id, bool setToDisable = false)
        {
            var orga = await _db.Set<Orga>()
            .SingleOrDefaultAsync(x =>
                    x.Id == Id);

            if (orga.IsDisabled != setToDisable)
            {
                orga.IsDisabled = setToDisable;
                orga.UpdatedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }

            return orga;
        }

        public async Task<OrgaUser> SetUserEnableAsync(Guid Id, Guid userId, bool setToDisable = false)
        {
            var orgauser = await _db.Set<OrgaUser>()
                .Include(r => r.Roles)
            .SingleOrDefaultAsync(x =>
                    x.Orga.Id == Id && x.User.Id == userId);

            if (orgauser.IsDisabled != setToDisable)
            {
                orgauser.IsDisabled = setToDisable;
                orgauser.UpdatedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }

            return orgauser;
        }

        public async Task<OrgaUser> AssociateOrgaUserAsync(OrgaUserInput orgaUserInput)
        {
            if (orgaUserInput == null)
                throw new ArgumentNullException(nameof(orgaUserInput));

            if (string.IsNullOrWhiteSpace(orgaUserInput.UserId))
                throw new ArgumentNullException(nameof(orgaUserInput.UserId));

            if (string.IsNullOrWhiteSpace(orgaUserInput.OrgaId))
                throw new ArgumentNullException(nameof(orgaUserInput.OrgaId));

            var orga = await _db.Set<Orga>().SingleOrDefaultAsync(x=>x.Id == Guid.Parse(orgaUserInput.OrgaId));
            var user = await _db.Set<User>().SingleOrDefaultAsync(x => x.Id == Guid.Parse(orgaUserInput.UserId));
            var roles = await _db.Set<Role>().Where(x => orgaUserInput.Roles.Contains(x.Name)).ToListAsync();

            var orgauser = await _db.Set<OrgaUser>()
                .Include(u=>u.User)
                .Include(o=>o.Orga)
                .Include(r=>r.Roles)
                .SingleOrDefaultAsync(x => x.Orga.Id == orga.Id && x.User.Id == user.Id);

            if (orgauser == null)
            {
                orgauser = new OrgaUser()
                {
                    Orga = orga,
                    User = user,
                    Roles = roles
                };

                await _db.AddAsync<OrgaUser>(orgauser);
                await _db.SaveChangesAsync();
            }
            else
            {
                orgauser.UpdatedAt = DateTime.UtcNow;
                foreach (var role in roles)
                {
                    if (!orgauser.Roles.Any(r => r.Name == role.Name))
                        orgauser.Roles.Add(role);
                }

                var toDelete = new List<Role>();
                foreach (var role in orgauser.Roles)
                {
                    if (!roles.Any(r => r.Name == role.Name))
                        toDelete.Add(role);
                }

                foreach (var role in toDelete)
                    orgauser.Roles.Remove(role);

                _db.Update(orgauser);
                await _db.SaveChangesAsync();
            }

            return orgauser;
        }
    }

    public interface IOrgaService
    {
        Task<Orga> GetOrgaByIdAsync(Guid Id);
        Task<List<Orga>> GetOrgasAsync();
        Task<List<OrgaUser>> GetUsersByOrgaIdAsync(Guid Id);
        Task<Orga> SetEnableAsync(Guid Id, bool setToDisable = false);
        Task<List<OrgaUser>> RemoveUserAsync(Guid Id, Guid UserId);
        Task<OrgaUser> SetUserEnableAsync(Guid Id, Guid userId, bool setToDisable = false);
        Task<OrgaUser> AssociateOrgaUserAsync(OrgaUserInput orgaUserInput);
    }
}
