using Lomba.API.Services;

namespace Lomba.API.Default
{
    public class Users
    {
        public const string Username_Admin = "admin";
        public const string Username_SuperAdmin = "superadmin";
        public const string Username_System = "system";
        public const string Username_User1 = "user1";
        public const string Username_User2 = "user2";
        public const string Username_User3 = "user3";

        public const string User_Password_Admin = "admin";
        public const string User_Password_SuperAdmin = "superadmin";
        public const string User_Password_System = "system";
        public const string User_Password_User1 = "user1";
        public const string User_Password_User2 = "user2";
        public const string User_Password_User3 = "user3";

        public const string User_Email_Admin = "admin@lomba.com";
        public const string User_Email_SuperAdmin = "superadmin@lomba.com";
        public const string User_Email_System = "system@lomba.com";
        public const string User_Email_User1 = "user1@lomba.com";
        public const string User_Email_User2 = "user2@lomba.com";
        public const string User_Email_User3 = "user3@lomba.com";

        public const string User_Name_Admin = "Administrador";
        public const string User_Name_SuperAdmin = "Súper Administrador";
        public const string User_Name_System = "Sistema";
        public const string User_Name_User1 = "Usuario Uno";
        public const string User_Name_User2 = "Usuario Dos";
        public const string User_Name_User3 = "Usuario Tres";

        public const string User_Id_Admin = "00000002-0000-0000-0000-00000000000A";
        public const string User_Id_SuperAdmin = "00000001-0000-0000-0000-00000000000A";
        public const string User_Id_System = "00000003-0000-0000-0000-00000000000B";
        public const string User_Id_User1 = "01000003-0000-0000-0000-00000000000B";
        public const string User_Id_User2 = "02000003-0000-0000-0000-00000000000B";
        public const string User_Id_User3 = "03000003-0000-0000-0000-00000000000B";

        public static Models.User Admin()
        {
            byte[]? passwordHash, passwordSalt = null;
            UserService.CreatePasswordHash(User_Password_Admin, out passwordHash, out passwordSalt);

            var u = new Models.User()
            {
                Id = Guid.Parse(User_Id_Admin),
                Name = User_Name_Admin,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = User_Email_Admin,
                Username = Username_Admin
            };

            return u;
        }
        public static Models.User SuperAdmin()
        {
            byte[]? passwordHash, passwordSalt = null;
            UserService.CreatePasswordHash(User_Password_SuperAdmin, out passwordHash, out passwordSalt);

            var u = new Models.User()
            {
                Id = Guid.Parse(User_Id_SuperAdmin),
                Name = User_Name_SuperAdmin,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = User_Email_SuperAdmin,
                Username = Username_SuperAdmin
            };

            return u;
        }

        public static Models.User System()
        {
            byte[]? passwordHash, passwordSalt = null;
            UserService.CreatePasswordHash(User_Password_System, out passwordHash, out passwordSalt);

            var u = new Models.User()
            {
                Id = Guid.Parse(User_Id_System),
                Name = User_Name_System,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = User_Email_System,
                Username = Username_System
            };

            return u;
        }

        public static Models.User User1()
        {
            byte[]? passwordHash, passwordSalt = null;
            UserService.CreatePasswordHash(User_Password_User1, out passwordHash, out passwordSalt);

            var u = new Models.User()
            {
                Id = Guid.Parse(User_Id_User1),
                Name = User_Name_User1,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = User_Email_User1,
                Username = Username_User1
            };

            return u;
        }

        public static Models.User User2()
        {
            byte[]? passwordHash, passwordSalt = null;
            UserService.CreatePasswordHash(User_Password_User2, out passwordHash, out passwordSalt);

            var u = new Models.User()
            {
                Id = Guid.Parse(User_Id_User2),
                Name = User_Name_User2,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = User_Email_User2,
                Username = Username_User2
            };

            return u;
        }

        public static Models.User User3()
        {
            byte[]? passwordHash, passwordSalt = null;
            UserService.CreatePasswordHash(User_Password_User3, out passwordHash, out passwordSalt);

            var u = new Models.User()
            {
                Id = Guid.Parse(User_Id_User3),
                Name = User_Name_User3,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = User_Email_User3,
                Username = Username_User3
            };

            return u;
        }

    }
}
