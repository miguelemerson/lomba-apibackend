using Xunit;
using Lomba.API.Default;
using Lomba.API.Services;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Lomba.API.Tests
{
    [Collection("InitializerCollection")]
    public class UserTests
    {
        private UserService _userService;
        public UserTests(Initializer ini)
        {
            if (_userService == null)
            {
                _userService = new UserService(ini.Context,
                    ini.Configuration);
            }
        }

        [Theory]
        [InlineData(Users.User_Password_SuperAdmin, true)]
        [InlineData(Users.User_Password_Admin, true)]
        [InlineData(Users.User_Password_System, true)]
        [InlineData(Users.User_Password_User1, true)]
        [InlineData(Users.User_Password_User2, true)]
        [InlineData(Users.User_Password_User3, true)]
        public void CreatePasswordAndVerify(string passWord, bool result)
        {
            byte[] passwordHash, passwordSalt = null;
            UserService.CreatePasswordHash(passWord, out passwordHash, out passwordSalt);

            Assert.Equal<bool>(result, UserService.VerifyPasswordHash(passWord, passwordHash, passwordSalt));
        }

        [Theory]
        [InlineData(Users.User_Id_SuperAdmin, Users.Username_SuperAdmin, true)]
        [InlineData(Users.User_Id_Admin, Users.Username_Admin, true)]
        [InlineData(Users.User_Id_System, Users.Username_System, true)]
        [InlineData(Users.User_Id_User1, Users.Username_User1, true)]
        [InlineData(Users.User_Id_User2, Users.Username_User2, true)]
        [InlineData(Users.User_Id_User3, Users.Username_User3, true)]
        public async void GetUserById(string Id, string userName, bool result)
        {
            var user = await _userService.GetUserByIdAsync(System.Guid.Parse(Id));    
            Assert.NotNull(user);
            Assert.Equal<bool>(result, actual: user.Username.Equals(userName));
        }

        [Fact]
        public async void GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            Assert.NotNull(users);
            Assert.Equal(6, users.Count);
        }

        [Theory]
        [InlineData(Users.Username_SuperAdmin, Users.User_Password_SuperAdmin, null, true)]
        [InlineData(Users.Username_SuperAdmin, Users.User_Password_SuperAdmin, Orgas.Org_Id_Without, true)]
        [InlineData(Users.Username_SuperAdmin, Users.User_Password_SuperAdmin, Orgas.Org_Id_Lomba, false)]
        [InlineData(Users.Username_Admin, Users.User_Password_Admin, null, true)]
        [InlineData(Users.Username_System, Users.User_Password_System, null, true)]
        [InlineData(Users.Username_User1, Users.User_Password_User1, null, true)]
        [InlineData(Users.Username_User2, Users.User_Password_User2, null, true)]
        [InlineData(Users.Username_User3, Users.User_Password_User3, null, true)]
        [InlineData(Users.Username_User3, Users.User_Password_User3, Orgas.Org_Id_Without, false)]
        [InlineData("nouser", "nopass", null, false)]
        public async void AuthenticateUsers(string userName, string userPassword, string orgaId, bool result)
        {
            var userAuth = new ViewModels.UserAuth() { Username = userName, Password = userPassword, OrgaId = orgaId };

            ViewModels.UserLogged? session = null;

            try
            {
                session = await _userService.AuthenticateAsync(userAuth);
            }
            catch { }

            if(result)
                Assert.NotNull(session);
            else
                Assert.Null(session);

            if (session != null)
            {
                Assert.Equal(userAuth.Username, session.Username);
                Assert.NotNull(session.Token);
                Assert.NotEmpty(session.Token);
            }
        }

        [Theory]
        [InlineData(Users.User_Id_SuperAdmin)]
        [InlineData(Users.User_Id_Admin)]
        [InlineData(Users.User_Id_System)]
        [InlineData(Users.User_Id_User1)]
        [InlineData(Users.User_Id_User2)]
        [InlineData(Users.User_Id_User3)]
        public async void SetEnableDisableById(string Id)
        {
            var user = await _userService.GetUserByIdAsync(System.Guid.Parse(Id));
            Assert.NotNull(user);
            Assert.Null(user.UpdatedAt);
            Assert.False(user.IsDisabled);

            user = await _userService.SetEnableAsync(System.Guid.Parse(Id), true);
            Assert.NotNull(user);
            Assert.True(user.IsDisabled);

            user = await _userService.SetEnableAsync(System.Guid.Parse(Id), false);
            Assert.NotNull(user);
            Assert.False(user.IsDisabled);

            user = await _userService.GetUserByIdAsync(System.Guid.Parse(Id));
            Assert.NotNull(user);
            Assert.NotNull(user.UpdatedAt);
            Assert.False(user.IsDisabled);
        }

        [Theory]
        [InlineData(Users.User_Id_SuperAdmin, 1, 2)]
        [InlineData(Users.User_Id_Admin, 1, 1)]
        [InlineData(Users.User_Id_System, 1, 1)]
        [InlineData(Users.User_Id_User1, 1, 1)]
        [InlineData(Users.User_Id_User2, 0, 1)]
        [InlineData(Users.User_Id_User3, 1, 1)]
        public async void GetOrgasByUser(string Id, int low, int high)
        {
            var orgas = await _userService.GetOrgasByUserIdAsync(System.Guid.Parse(Id));
            Assert.NotNull(orgas);

            Assert.False(orgas.Exists(u => u.User == null));
            Assert.False(orgas.Exists(r => r.Roles == null));
            Assert.False(orgas.Exists(o => o.Orga == null));

            Assert.InRange<int>(orgas.Count, low, high);
        }

        [Theory]
        [InlineData(Users.User_Id_SuperAdmin, Orgas.Org_Id_Without, 1, 1)]
        [InlineData(Users.User_Id_Admin, Orgas.Org_Id_Lomba, 1, 2)]
        [InlineData(Users.User_Id_System, Orgas.Org_Id_Lomba, 1, 1)]
        [InlineData(Users.User_Id_User1, Orgas.Org_Id_Lomba, 1, 1)]
        [InlineData(Users.User_Id_User2, Orgas.Org_Id_Lomba, 1, 1)]
        [InlineData(Users.User_Id_User3, Orgas.Org_Id_Lomba, 1, 1)]
        public async void GetRolesByUserOrga(string Id, string orgaId, int low, int high)
        {
            var roles = await _userService.GetRolesByUserOrgaAsync(System.Guid.Parse(Id), System.Guid.Parse(orgaId));
            Assert.NotNull(roles);

            Assert.InRange<int>(roles.Count, low, high);
        }

    }
}