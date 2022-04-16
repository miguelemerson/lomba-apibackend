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
                    columns: new[] { "Id", "Username", "Name", "Email", "PasswordHash", "PasswordSalt", 
                        "CreatedAt", "UpdatedAt", "IsDisabled", "Expires" },
                    values: new object[,] {
                        { userSuperAdmin.Id, userSuperAdmin.Username, userSuperAdmin.Name,
                            userSuperAdmin.Email, userSuperAdmin.PasswordHash, userSuperAdmin.PasswordSalt,
                            userSuperAdmin.CreatedAt, userSuperAdmin.UpdatedAt, false, null},
                        { userAdmin.Id, userAdmin.Username, userAdmin.Name,
                            userAdmin.Email, userAdmin.PasswordHash, userAdmin.PasswordSalt,
                            userAdmin.CreatedAt, userAdmin.UpdatedAt, false, null},
                        { userSystem.Id, userSystem.Username, userSystem.Name,
                            userSystem.Email, userSystem.PasswordHash, userSystem.PasswordSalt,
                            userSystem.CreatedAt, userSystem.UpdatedAt, false, null},
                        { user1.Id, user1.Username, user1.Name,
                            user1.Email, user1.PasswordHash, user1.PasswordSalt,
                            user1.CreatedAt, user1.UpdatedAt, false, null},
                        { user2.Id, user2.Username, user2.Name,
                            user2.Email, user2.PasswordHash, user2.PasswordSalt,
                            user2.CreatedAt, user2.UpdatedAt, false, null},
                        { user3.Id, user3.Username, user3.Name,
                            user3.Email, user3.PasswordHash, user3.PasswordSalt,
                            user3.CreatedAt, user3.UpdatedAt, false, null}
                    }
                );

            //Inserta Roles
            _ = migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Name", "Description", "IsDisabled", "UpdatedAt" },
                values: new object[,] {
                { Default.Roles.Role_Name_SuperAdmin, Default.Roles.Role_Description_SuperAdmin, false, null },
                { Default.Roles.Role_Name_Admin, Default.Roles.Role_Description_Admin, false, null },
                { Default.Roles.Role_Name_Basic, Default.Roles.Role_Description_Basic, false, null }});

            //Inserta Organizaciones
            var orgWithout = new Models.Orga()
            {
                Id = Guid.Parse(Default.Orgas.Org_Id_Without),
                Name = Default.Orgas.Org_Name_Without
            };
            var orgLomba = new Models.Orga()
            {
                Id = Guid.Parse(Default.Orgas.Org_Id_Lomba),
                Name = Default.Orgas.Org_Name_Lomba
            };
            _ = migrationBuilder.InsertData(
                table: "Orgas",
                columns: new[] { "Id", "Name", "CreatedAt", "UpdatedAt","IsDisabled", "Expires" },
                values: new object[,] {
                            { orgWithout.Id.ToString(), orgWithout.Name, orgWithout.CreatedAt, null, false, null },
                            { orgLomba.Id.ToString(), orgLomba.Name, orgLomba.CreatedAt, null, false, null }
                });

            //Inserta relación de Organización con Usuarios
            var org_W_SuperAdmin = new Models.OrgaUser()
            {
                Id = Guid.NewGuid(),
                User = userSuperAdmin,
                Orga = orgWithout
            };
            var org_LombaAdmin = new Models.OrgaUser()
            {
                Id = Guid.NewGuid(),
                User = userAdmin,
                Orga = orgLomba
            };
            var org_LombaSystem = new Models.OrgaUser()
            {
                Id = Guid.NewGuid(),
                User = userSystem,
                Orga = orgLomba
            };
            var org_LombaUser1 = new Models.OrgaUser()
            {
                Id = Guid.NewGuid(),
                User = user1,
                Orga = orgLomba
            };
            var org_LombaUser2 = new Models.OrgaUser()
            {
                Id = Guid.NewGuid(),
                User = user2,
                Orga = orgLomba
            };
            var org_LombaUser3 = new Models.OrgaUser()
            {
                Id = Guid.NewGuid(),
                User = user3,
                Orga = orgLomba
            };

            _ = migrationBuilder.InsertData(
                table: "OrgasUsers",
                columns: new[] { "Id", "OrgaId", "UserId", "CreatedAt", "UpdatedAt", "IsDisabled", "Expires" },
                values: new object[,]
                {
                    { org_W_SuperAdmin.Id.ToString(), org_W_SuperAdmin.Orga.Id.ToString(),
                    org_W_SuperAdmin.User.Id.ToString(), org_W_SuperAdmin.CreatedAt, null, false, null },
                    { org_LombaAdmin.Id.ToString(), org_LombaAdmin.Orga.Id.ToString(),
                    org_LombaAdmin.User.Id.ToString(), org_LombaAdmin.CreatedAt, null, false, null },
                    { org_LombaSystem.Id.ToString(), org_LombaSystem.Orga.Id.ToString(),
                    org_LombaSystem.User.Id.ToString(), org_LombaSystem.CreatedAt, null, false, null },
                    { org_LombaUser1.Id.ToString(), org_LombaUser1.Orga.Id.ToString(),
                    org_LombaUser1.User.Id.ToString(), org_LombaUser1.CreatedAt, null, false, null },
                    { org_LombaUser2.Id.ToString(), org_LombaUser2.Orga.Id.ToString(),
                    org_LombaUser2.User.Id.ToString(), org_LombaUser2.CreatedAt, null, false, null },
                    { org_LombaUser3.Id.ToString(), org_LombaUser3.Orga.Id.ToString(),
                    org_LombaUser3.User.Id.ToString(), org_LombaUser3.CreatedAt, null, false, null },
                });

            _ = migrationBuilder.InsertData(
                table: "OrgaUserRole",
                columns: new[] { "OrgaUsersId", "RolesName" },
                values: new object[,] {
                     { org_W_SuperAdmin.Id.ToString(), Default.Roles.Role_Name_SuperAdmin },
                     { org_LombaAdmin.Id.ToString(), Default.Roles.Role_Name_Admin },
                     { org_LombaSystem.Id.ToString(), Default.Roles.Role_Name_Basic},
                     { org_LombaUser1.Id.ToString(), Default.Roles.Role_Name_Basic },
                     { org_LombaUser2.Id.ToString(), Default.Roles.Role_Name_Basic },
                     { org_LombaUser3.Id.ToString(), Default.Roles.Role_Name_Basic }
                });

            }

            protected override void Down(MigrationBuilder migrationBuilder)
            {
                
            }
        }

}
