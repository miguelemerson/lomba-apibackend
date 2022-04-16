using Xunit;
using Lomba.API.Default;
using Lomba.API.Services;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Lomba.API.Tests
{
    [Collection("InitializerCollection")]
    public class OrgaTests
    {
        private OrgaService _orgaService;
        public OrgaTests(Initializer ini)
        {
            if (_orgaService == null)
            {
                _orgaService = new OrgaService(ini.Context,
                    ini.Configuration);
            }
        }

        [Theory(DisplayName = "Organización por Id")]
        [InlineData(Orgas.Org_Id_Without, Orgas.Org_Name_Without, true)]
        [InlineData(Orgas.Org_Id_Lomba, Orgas.Org_Name_Lomba, true)]
        public async void GetOrgaById(string Id, string name, bool result)
        {
            var orga = await _orgaService.GetOrgaByIdAsync(System.Guid.Parse(Id));
            Assert.NotNull(orga);
            Assert.Equal<bool>(result, actual: orga.Name.Equals(name));
        }

        [Fact]
        public async void GetOrgas()
        {
            var orgas = await _orgaService.GetOrgasAsync();
            Assert.NotNull(orgas);
            Assert.Equal(2, orgas.Count);
        }

        [Theory]
        [InlineData(Orgas.Org_Id_Without, 1, 1)]
        [InlineData(Orgas.Org_Id_Lomba, 4, 6)]
        public async void GetUsersByOrga(string Id, int low, int high)
        {
            var users = await _orgaService.GetUsersByOrgaIdAsync(System.Guid.Parse(Id));
            Assert.NotNull(users);

            Assert.False(users.Exists(u => u.User == null), "Usuario nulo");
            Assert.False(users.Exists(r => r.Roles == null), "Roles nulo");
            Assert.False(users.Exists(o => o.Orga == null), "Orga nulo");

            Assert.InRange<int>(users.Count, low, high);
        }

        [Theory]
        [InlineData(Orgas.Org_Id_Without)]
        [InlineData(Orgas.Org_Id_Lomba)]
        public async void SetEnableDisableById(string Id)
        {
            var orga = await _orgaService.GetOrgaByIdAsync(System.Guid.Parse(Id));
            Assert.NotNull(orga);
            Assert.Null(orga.UpdatedAt);
            Assert.False(orga.IsDisabled);

            orga = await _orgaService.SetEnableAsync(System.Guid.Parse(Id), true);
            Assert.NotNull(orga);
            Assert.True(orga.IsDisabled);

            orga = await _orgaService.SetEnableAsync(System.Guid.Parse(Id), false);
            Assert.NotNull(orga);
            Assert.False(orga.IsDisabled);

            orga = await _orgaService.GetOrgaByIdAsync(System.Guid.Parse(Id));
            Assert.NotNull(orga);
            Assert.NotNull(orga.UpdatedAt);
            Assert.False(orga.IsDisabled);
        }

        [Theory]
        [InlineData(Orgas.Org_Id_Without, Users.User_Id_SuperAdmin)]
        [InlineData(Orgas.Org_Id_Lomba, Users.User_Id_Admin)]
        [InlineData(Orgas.Org_Id_Lomba, Users.User_Id_System)]
        [InlineData(Orgas.Org_Id_Lomba, Users.User_Id_User1)]
        [InlineData(Orgas.Org_Id_Lomba, Users.User_Id_User2)]
        [InlineData(Orgas.Org_Id_Lomba, Users.User_Id_User3)]
        public async void SetUserEnableDisableById(string Id, string userId)
        {
            var userList = await _orgaService.GetUsersByOrgaIdAsync(System.Guid.Parse(Id));
            Assert.NotNull(userList);
            Assert.True(userList.Exists(x => x.User.Id.Equals(System.Guid.Parse(userId))));

            var orgauser = await _orgaService.SetUserEnableAsync(System.Guid.Parse(Id),
                System.Guid.Parse(userId), true);
            Assert.NotNull(orgauser);
            Assert.NotNull(orgauser.UpdatedAt);

            Assert.True(orgauser.IsDisabled);

            var firstUpdate = orgauser.UpdatedAt;
            orgauser = await _orgaService.SetUserEnableAsync(System.Guid.Parse(Id),
                            System.Guid.Parse(userId), false);
            Assert.NotNull(orgauser);

            Assert.InRange<int>(System.DateTime.Compare(firstUpdate.Value, orgauser.UpdatedAt.Value), -1, 0);
        }

        [Theory]
        [InlineData(Orgas.Org_Id_Lomba, Users.User_Id_User2)]
        public async void RemoveUser(string Id, string userId)
        {
            var userList = await _orgaService.RemoveUserAsync(System.Guid.Parse(Id), System.Guid.Parse(userId));
            Assert.NotNull(userList);

            Assert.False(userList.Exists(x=> x.User.Id.Equals(System.Guid.Parse(userId))));

            Assert.False(userList.Exists(u => u.User == null), "Usuario nulo");
            Assert.False(userList.Exists(r => r.Roles == null), "Roles nulo");
            Assert.False(userList.Exists(o => o.Orga == null), "Orga nulo");

            userList = await _orgaService.GetUsersByOrgaIdAsync(System.Guid.Parse(Id));
            Assert.NotNull(userList);
            Assert.False(userList.Exists(x => x.User.Id.Equals(System.Guid.Parse(userId))));
        }

        [Theory]
        [InlineData(Orgas.Org_Id_Lomba, Users.User_Id_SuperAdmin, Roles.Role_Name_Basic)]
        [InlineData(Orgas.Org_Id_Lomba, Users.User_Id_User1, Roles.Role_Name_Admin)]
        [InlineData(Orgas.Org_Id_Without, Users.User_Id_SuperAdmin, Roles.Role_Name_Basic)]
        [InlineData(Orgas.Org_Id_Lomba, Users.User_Id_System, "")]
        public async void AssociateOrgaUserAsync(string Id, string userId, string roleName)
        {
            var ouInput = new ViewModels.OrgaUserInput()
            {
                OrgaId = Id,
                UserId = userId,
                Roles = new List<string>() { roleName }
            };

            var orgauser = await _orgaService.AssociateOrgaUserAsync(ouInput);
            Assert.NotNull(orgauser);

            var nou = await _orgaService.GetUsersByOrgaIdAsync(System.Guid.Parse(Id));
            Assert.NotNull(nou);

            if(roleName != "")
                Assert.True(nou.Exists(x => x.User.Id == System.Guid.Parse(userId) &&
                    x.Orga.Id == System.Guid.Parse(Id) &&
                    x.Roles.Exists(r=>r.Name == roleName)));
            else
                Assert.True(nou.Exists(x => x.User.Id == System.Guid.Parse(userId) &&
                    x.Orga.Id == System.Guid.Parse(Id) &&
                    x.Roles.Count == 0));
        }
    }
}
