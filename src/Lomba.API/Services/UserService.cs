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
                        x.Email == loginRequest.Username));

                if (user == null)
                {
                    throw new Exception("Usuario no existe, no es válido o contraseña es incorrecta");
                }

                //verifica password
                if (!VerifyPasswordHash(loginRequest.Password, user.PasswordHash, user.PasswordSalt))
                {
                    throw new Exception("Usuario no existe, no es válido o contraseña es incorrecta");
                }

                var token = this.GenerateJwtToken(user, DateTime.UtcNow.AddDays(7));

                UserLogged userLogged = new UserLogged();
                userLogged.Token = token;
                userLogged.Username = user.Username;

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

        public async void DeleteUserAsync(Guid Id)
        {
            var user = await _db.Set<User>()
                .SingleOrDefaultAsync(x =>
                        x.Id == Id);

            _ = _db.Remove<User>(user);

            await _db.SaveChangesAsync();
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
        private string GenerateJwtToken(User user, DateTime expires)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetValue<string>("Security:JWTSecret"));

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.AddClaim(new Claim(ClaimTypes.Name, user.Username.ToString()));

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
    }

    public interface IUserService
    {
        Task<UserLogged> AuthenticateAsync(UserAuth loginRequest);
        Task<User> GetUserByIdAsync(Guid Id);
        Task<List<User>> GetUsersAsync();
        void DeleteUserAsync(Guid Id);
    }
}
