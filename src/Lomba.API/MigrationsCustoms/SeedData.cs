using Microsoft.EntityFrameworkCore.Migrations;

namespace Lomba.API.Migrations
{
        public partial class SeedData : Migration
        {
            protected override void Up(MigrationBuilder migrationBuilder)
            {
            //Inserta usuarios

            var userSuperAdmin = Default.Users.SuperAdmin();
            var userAdmin = Default.Users.Admin();
            var userSystem = Default.Users.System();
            var user1 = Default.Users.User1();
            var user2 = Default.Users.User2();
            var user3 = Default.Users.User3();

            _ = migrationBuilder.InsertData(
                    table: "Users",
                    columns: new[] { "Id", "Username", "Name", "Email", "PasswordHash", "PasswordSalt", "CreatedAt", "UpdatedAt" },
                    values: new object[,] {
                        { userSuperAdmin.Id, userSuperAdmin.Username, userSuperAdmin.Name,
                            userSuperAdmin.Email, userSuperAdmin.PasswordHash, userSuperAdmin.PasswordSalt,
                            userSuperAdmin.CreatedAt, userSuperAdmin.UpdatedAt},
                        { userAdmin.Id, userAdmin.Username, userAdmin.Name,
                            userAdmin.Email, userAdmin.PasswordHash, userAdmin.PasswordSalt,
                            userAdmin.CreatedAt, userAdmin.UpdatedAt},
                        { userSystem.Id, userSystem.Username, userSystem.Name,
                            userSystem.Email, userSystem.PasswordHash, userSystem.PasswordSalt,
                            userSystem.CreatedAt, userSystem.UpdatedAt},
                        { user1.Id, user1.Username, user1.Name,
                            user1.Email, user1.PasswordHash, user1.PasswordSalt,
                            user1.CreatedAt, user1.UpdatedAt},
                        { user2.Id, user2.Username, user2.Name,
                            user2.Email, user2.PasswordHash, user2.PasswordSalt,
                            user2.CreatedAt, user2.UpdatedAt},
                        { user3.Id, user3.Username, user3.Name,
                            user3.Email, user3.PasswordHash, user3.PasswordSalt,
                            user3.CreatedAt, user3.UpdatedAt}
                    }
                );
            }

            protected override void Down(MigrationBuilder migrationBuilder)
            {
                
            }
        }

}
