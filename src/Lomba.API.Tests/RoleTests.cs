using Xunit;
using Lomba.API.Default;
using Lomba.API.Services;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Lomba.API.Tests
{
    [Collection("InitializerCollection")]
    public class RoleTests
    {
        private RoleService _roleService;
        public RoleTests(Initializer ini)
        {
            if (_roleService == null)
            {
                _roleService = new RoleService(ini.Context,
                    ini.Configuration);
            }
        }
        [Fact]
        public async void GetRoles()
        {
            var roles = await _roleService.GetRolesAsync();
            Assert.NotNull(roles);
            Assert.Equal(3, roles.Count);
        }
        [Theory]
        [InlineData(Roles.Role_Name_SuperAdmin)]
        [InlineData(Roles.Role_Name_Admin)]
        [InlineData(Roles.Role_Name_Basic)]
        public async void SetEnableDisableById(string name)
        {
            var role = await _roleService.GetRoleByNameAsync(name);
            Assert.NotNull(role);
            Assert.Null(role.UpdatedAt);
            Assert.False(role.IsDisabled);

            role = await _roleService.SetEnableAsync(name, true);
            Assert.NotNull(role);
            Assert.True(role.IsDisabled);

            role = await _roleService.SetEnableAsync(name, false);
            Assert.NotNull(role);
            Assert.False(role.IsDisabled);

            role = await _roleService.GetRoleByNameAsync(name);
            Assert.NotNull(role);
            Assert.NotNull(role.UpdatedAt);
            Assert.False(role.IsDisabled);
        }
    }
}
