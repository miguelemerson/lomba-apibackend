using Lomba.API.ViewModels;
using Lomba.API.Models;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Lomba.API.Services
{
    public class UserService : IUserService
    {
        private readonly Contexts.DataContext _db;
        private readonly IConfiguration _config;

        public UserService(Contexts.DataContext dataContext,
            IConfiguration configuration)
        {
            _db = dataContext;
            _config = configuration;
        }
        public async Task<UserLogged> AuthenticateAsync(UserAuth loginRequest)
        {
            if (loginRequest == null)
                throw new ArgumentNullException(nameof(loginRequest));

            if (string.IsNullOrWhiteSpace(loginRequest.Username))
                throw new ArgumentNullException(nameof(loginRequest.Username));

            if (string.IsNullOrWhiteSpace(loginRequest.Username))
                throw new ArgumentNullException(nameof(loginRequest.Password));

            try
            {
                //busca usuario habilitado por username o correo
                var user = await _db.Set<User>().AsNoTracking()
                    .SingleOrDefaultAsync(x => (
                        x.Username == loginRequest.Username ||
                        x.Email == loginRequest.Username) &&
                        x.IsDisabled == false);

                if (user == null)
                {
                    throw new Exception("Usuario no existe, no es válido o contraseña es incorrecta");
                }

                //verifica password
                if (!VerifyPasswordHash(loginRequest.Password, user.PasswordHash, user.PasswordSalt))
                {
                    throw new Exception("Usuario no existe, no es válido o contraseña es incorrecta");
                }

                Guid orgaId = Guid.Parse(Default.Orgas.Org_Id_Lomba); //predeterminado.

                if (!string.IsNullOrWhiteSpace(loginRequest.OrgaId))
                    orgaId = Guid.Parse(loginRequest.OrgaId);

                var orgaUsers = await _db.Set<OrgaUser>().AsNoTracking()
                    .Include(u => u.User)
                    .Include(o => o.Orga)
                    .Include(r => r.Roles.Where(f=>f.IsDisabled==false))
                    .Where(x => x.User.Id == user.Id &&
                        x.IsDisabled == false).ToListAsync();

                if (orgaUsers == null || orgaUsers.Count < 1)
                {
                    throw new Exception("Usuario no existe, no es válido o contraseña es incorrecta");
                }

                var roles = new List<string>();

                if (orgaUsers.Count > 1 &&
                    string.IsNullOrWhiteSpace(loginRequest.OrgaId))
                {
                    throw new Exception("Usuario no existe, no es válido o contraseña es incorrecta");
                }

                if (orgaUsers.Any(x => x.Orga.Id == orgaId))
                {
                    roles = orgaUsers.SingleOrDefault(x => x.Orga.Id == orgaId &&
                                x.IsDisabled == false)
                        .Roles.Select(r => r.Name).ToList();
                }
                else if (orgaUsers.Any(x => x.Orga.Id == Guid.Parse(Default.Orgas.Org_Id_Without)) &&
                    string.IsNullOrWhiteSpace(loginRequest.OrgaId))
                {
                    roles = orgaUsers
                        .SingleOrDefault(x => x.Orga.Id == Guid.Parse(Default.Orgas.Org_Id_Without) &&
                                x.IsDisabled == false)
                        .Roles.Select(r => r.Name).ToList();

                    orgaId = Guid.Parse(Default.Orgas.Org_Id_Without);
                }
                else
                {
                    throw new Exception("Usuario no existe, no es válido o contraseña es incorrecta");
                }

                var token = this.GenerateJwtToken(user, DateTime.UtcNow.AddDays(7), roles);

                UserLogged userLogged = new UserLogged();
                userLogged.Token = token;
                userLogged.Username = user.Username;
                userLogged.OrgaId = orgaId.ToString();

                return userLogged;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<User> GetUserByIdAsync(Guid Id)
        {
            return await _db.Set<User>().AsNoTracking()
                    .SingleOrDefaultAsync(x =>
                        x.Id == Id);
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _db.Set<User>().AsNoTracking().ToListAsync();
        }

        public async Task<List<ViewModels.UserItemAll>> GetUsersWithOrgaCountAsync()
        {
            List<User> users = await _db.Set<User>().AsNoTracking().ToListAsync();

            List<OrgaUser> orgaUsers =  await _db.Set<OrgaUser>().AsNoTracking()
                .Include(u => u.User)
                .Include(r => r.Roles)
                .Include(g => g.Orga).ToListAsync();

            var userList = users.Select(
                u => new UserItemAll { 
                    User = u, 
                    OrgaCount = orgaUsers.Where(
                        a => a.User.Id == u.Id).Count() }
                );

            return userList.ToList();
        }
        public async void DeleteUserAsync(Guid Id)
        {
            var user = await _db.Set<User>()
                .SingleOrDefaultAsync(x =>
                        x.Id == Id);

            _ = _db.Remove<User>(user);

            await _db.SaveChangesAsync();
        }

        public async Task<List<OrgaUser>> GetOrgasByUserIdAsync(Guid Id)
        {
            return await _db.Set<OrgaUser>().AsNoTracking()
                .Include(u => u.User)
                .Include(r => r.Roles)
                .Include(g => g.Orga)
                .Where(o => o.User.Id == Id).ToListAsync();
        }

        public async Task<List<Role>> GetRolesByUserOrgaAsync(Guid Id, Guid orgaId)
        {
            var orgauser = await _db.Set<OrgaUser>().AsNoTracking()
                .Include(u => u.User)
                .Include(r => r.Roles)
                .Include(g => g.Orga)
                .SingleOrDefaultAsync(o => o.User.Id == Id && o.Orga.Id == orgaId);

            return orgauser?.Roles ?? null;
        }


        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));

            if (storedHash.Length != 64) throw new ArgumentException("Hash no válido");
            if (storedSalt.Length != 128) throw new ArgumentException("Salt no válido");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }

            }
            return true;
        }
        private string GenerateJwtToken(User user, DateTime expires, List<string> roles = null)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetValue<string>("Security:JWTSecret"));

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.AddClaim(new Claim(ClaimTypes.Name, user.Username.ToString()));

            if (roles != null)
            {
                foreach (string role in roles)
                    claims.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<User> SetEnableAsync(Guid Id, bool setToDisable = false)
        {
            var user = await _db.Set<User>()
            .SingleOrDefaultAsync(x =>
                    x.Id == Id);

            if (user.IsDisabled != setToDisable)
            {
                user.IsDisabled = setToDisable;
                user.UpdatedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }

            return user;
        }
    }

    public interface IUserService
    {
        Task<UserLogged> AuthenticateAsync(UserAuth loginRequest);
        Task<User> GetUserByIdAsync(Guid Id);
        Task<List<User>> GetUsersAsync();
        void DeleteUserAsync(Guid Id);
        Task<User> SetEnableAsync(Guid Id, bool setToDisable = false);
        Task<List<OrgaUser>> GetOrgasByUserIdAsync(Guid Id);

        Task<List<Role>> GetRolesByUserOrgaAsync(Guid Id, Guid orgaId);
        Task<List<ViewModels.UserItemAll>> GetUsersWithOrgaCountAsync();
    }
}
