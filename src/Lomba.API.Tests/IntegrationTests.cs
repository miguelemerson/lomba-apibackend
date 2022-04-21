using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Lomba.API;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Lomba.API.Tests
{
    public class IntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        public IntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData(Default.Users.Username_SuperAdmin, Default.Users.User_Password_SuperAdmin, "/api/v1/User", true)]
        [InlineData(Default.Users.Username_Admin, Default.Users.User_Password_Admin, "/api/v1/User", true)]
        [InlineData(Default.Users.Username_System, Default.Users.User_Password_System, "/api/v1/User", false)]
        [InlineData(Default.Users.Username_User3, Default.Users.User_Password_User3, "/api/v1/User", false)]
        [InlineData(Default.Users.Username_SuperAdmin, Default.Users.User_Password_SuperAdmin, $"/api/v1/User/{Default.Users.User_Id_User1}", true)]
        [InlineData(Default.Users.Username_Admin, Default.Users.User_Password_Admin, $"/api/v1/User/{Default.Users.User_Id_User1}", true)]
        [InlineData(Default.Users.Username_System, Default.Users.User_Password_System, $"/api/v1/User/{Default.Users.User_Id_User1}", true)]
        [InlineData(Default.Users.Username_User3, Default.Users.User_Password_User3, $"/api/v1/User/{Default.Users.User_Id_User1}", true)]
        [InlineData(Default.Users.Username_SuperAdmin, Default.Users.User_Password_SuperAdmin, $"/api/v1/User/{Default.Users.User_Id_User1}/orgas", true)]
        [InlineData(Default.Users.Username_Admin, Default.Users.User_Password_Admin, $"/api/v1/User/{Default.Users.User_Id_User1}/orgas", true)]
        [InlineData(Default.Users.Username_System, Default.Users.User_Password_System, $"/api/v1/User/{Default.Users.User_Id_User1}/orgas", true)]
        [InlineData(Default.Users.Username_User3, Default.Users.User_Password_User3, $"/api/v1/User/{Default.Users.User_Id_User1}/orgas", true)]
        [InlineData(Default.Users.Username_SuperAdmin, Default.Users.User_Password_SuperAdmin, "/api/v1/Role", true)]
        [InlineData(Default.Users.Username_Admin, Default.Users.User_Password_Admin, "/api/v1/Role", true)]
        [InlineData(Default.Users.Username_System, Default.Users.User_Password_System, "/api/v1/Role", true)]
        [InlineData(Default.Users.Username_User3, Default.Users.User_Password_User3, "/api/v1/Role", true)]
        [InlineData(Default.Users.Username_SuperAdmin, Default.Users.User_Password_SuperAdmin, $"/api/v1/Role/{Default.Roles.Role_Name_Basic}", true)]
        [InlineData(Default.Users.Username_Admin, Default.Users.User_Password_Admin, $"/api/v1/Role/{Default.Roles.Role_Name_Basic}", true)]
        [InlineData(Default.Users.Username_System, Default.Users.User_Password_System, $"/api/v1/Role/{Default.Roles.Role_Name_Basic}", true)]
        [InlineData(Default.Users.Username_User3, Default.Users.User_Password_User3, $"/api/v1/Role/{Default.Roles.Role_Name_Basic}", true)]
        [InlineData(Default.Users.Username_SuperAdmin, Default.Users.User_Password_SuperAdmin, "/api/Ping", true)]
        [InlineData(Default.Users.Username_Admin, Default.Users.User_Password_Admin, "/api/Ping", true)]
        [InlineData(Default.Users.Username_System, Default.Users.User_Password_System, "/api/Ping", true)]
        [InlineData(Default.Users.Username_User3, Default.Users.User_Password_User3, "/api/Ping", true)]
        [InlineData(Default.Users.Username_SuperAdmin, Default.Users.User_Password_SuperAdmin, "/api/Ping/endpoints", true)]
        [InlineData(Default.Users.Username_Admin, Default.Users.User_Password_Admin, "/api/Ping/endpoints", true)]
        [InlineData(Default.Users.Username_System, Default.Users.User_Password_System, "/api/Ping/endpoints", true)]
        [InlineData(Default.Users.Username_User3, Default.Users.User_Password_User3, "/api/Ping/endpoints", true)]
        [InlineData(Default.Users.Username_SuperAdmin, Default.Users.User_Password_SuperAdmin, "/api/v1/Orga", true)]
        [InlineData(Default.Users.Username_Admin, Default.Users.User_Password_Admin, "/api/v1/Orga", true)]
        [InlineData(Default.Users.Username_System, Default.Users.User_Password_System, "/api/v1/Orga", true)]
        [InlineData(Default.Users.Username_User3, Default.Users.User_Password_User3, "/api/v1/Orga", true)]
        [InlineData(Default.Users.Username_SuperAdmin, Default.Users.User_Password_SuperAdmin, $"/api/v1/Orga/{Default.Orgas.Org_Id_Lomba}", true)]
        [InlineData(Default.Users.Username_Admin, Default.Users.User_Password_Admin, $"/api/v1/Orga/{Default.Orgas.Org_Id_Lomba}", true)]
        [InlineData(Default.Users.Username_System, Default.Users.User_Password_System, $"/api/v1/Orga/{Default.Orgas.Org_Id_Lomba}", true)]
        [InlineData(Default.Users.Username_User3, Default.Users.User_Password_User3, $"/api/v1/Orga/{Default.Orgas.Org_Id_Lomba}", true)]
        [InlineData(Default.Users.Username_SuperAdmin, Default.Users.User_Password_SuperAdmin, $"/api/v1/Orga/{Default.Orgas.Org_Id_Lomba}/users", true)]
        [InlineData(Default.Users.Username_Admin, Default.Users.User_Password_Admin, $"/api/v1/Orga/{Default.Orgas.Org_Id_Lomba}/users", true)]
        [InlineData(Default.Users.Username_System, Default.Users.User_Password_System, $"/api/v1/Orga/{Default.Orgas.Org_Id_Lomba}/users", false)]
        [InlineData(Default.Users.Username_User3, Default.Users.User_Password_User3, $"/api/v1/Orga/{Default.Orgas.Org_Id_Lomba}/users", false)]
        public async Task Get_ReturnSuccess(string userName, string userPassword, string url, bool statusOk)
        {
            var client = _factory.CreateClient();

            var userLogged = await TestLoginUserAsync(client, userName, userPassword);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + userLogged.Token);

            var response = await client.GetAsync(url);
            Assert.Equal(statusOk, response.IsSuccessStatusCode);

        }

        public async Task<ViewModels.UserLogged> TestLoginUserAsync(HttpClient client, string userName, string passWord)
        {
            var auth = new ViewModels.UserAuth()
            {
                Username = userName,
                Password = passWord
            };

            StringContent stringContent = null;
            HttpResponseMessage resMsg = null;
            stringContent = GetStringContent(auth);
            resMsg = await client.PostAsync("/api/v1/User/authenticate", stringContent);
            Assert.True(resMsg.IsSuccessStatusCode);

            var userModel = JsonConvert
                .DeserializeObject<ViewModels.UserLogged>(await resMsg.Content.ReadAsStringAsync());

            Assert.NotNull(userModel);

            return userModel;
        }

        private static StringContent GetStringContent(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj)
                , System.Text.Encoding.UTF8
                , "application/json");
        }

    }
}
