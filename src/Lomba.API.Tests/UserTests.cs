using Xunit;
using Lomba.API.Default;
using Lomba.API.Services;

namespace Lomba.API.Tests
{
    public class UserTests
    {
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
    }
}