using Microsoft.EntityFrameworkCore.Migrations;

namespace Lomba.API.Migrations
{
        public partial class SeedData : Migration
        {
            protected override void Up(MigrationBuilder migrationBuilder)
            {
                //Inserta relación usuarios con roles
                migrationBuilder.InsertData(
                    table: "Users",
                    columns: new[] { "Id", "Username", "Name", "Email", "PasswordHash", "PasswordSalt", "CreatedAt", "UpdatedAt" },
                    values: new object[,] {
                        { Default.Users.SuperAdmin().Id, Default.Users.SuperAdmin().Username, Default.Users.SuperAdmin().Name,
                            Default.Users.SuperAdmin().Email, Default.Users.SuperAdmin().PasswordHash, Default.Users.SuperAdmin().PasswordSalt,
                            Default.Users.SuperAdmin().CreatedAt, Default.Users.SuperAdmin().UpdatedAt},
                        { Default.Users.Admin().Id, Default.Users.Admin().Username, Default.Users.Admin().Name,
                            Default.Users.Admin().Email, Default.Users.Admin().PasswordHash, Default.Users.Admin().PasswordSalt,
                            Default.Users.Admin().CreatedAt, Default.Users.Admin().UpdatedAt},
                        { Default.Users.System().Id, Default.Users.System().Username, Default.Users.System().Name,
                            Default.Users.System().Email, Default.Users.System().PasswordHash, Default.Users.System().PasswordSalt,
                            Default.Users.System().CreatedAt, Default.Users.System().UpdatedAt},
                        { Default.Users.User1().Id, Default.Users.User1().Username, Default.Users.User1().Name,
                            Default.Users.User1().Email, Default.Users.User1().PasswordHash, Default.Users.User1().PasswordSalt,
                            Default.Users.User1().CreatedAt, Default.Users.User1().UpdatedAt},
                        { Default.Users.User2().Id, Default.Users.User2().Username, Default.Users.User2().Name,
                            Default.Users.User2().Email, Default.Users.User2().PasswordHash, Default.Users.User2().PasswordSalt,
                            Default.Users.User2().CreatedAt, Default.Users.User2().UpdatedAt},
                        { Default.Users.User3().Id, Default.Users.User3().Username, Default.Users.User3().Name,
                            Default.Users.User3().Email, Default.Users.User3().PasswordHash, Default.Users.User3().PasswordSalt,
                            Default.Users.User3().CreatedAt, Default.Users.User3().UpdatedAt}
                    }
                );
            }

            protected override void Down(MigrationBuilder migrationBuilder)
            {
                
            }
        }

}
