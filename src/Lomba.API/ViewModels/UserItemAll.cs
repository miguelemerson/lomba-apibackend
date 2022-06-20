using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lomba.API.ViewModels
{
    public class UserItemAll
    {
        public Models.User User { get; set; }
        public int OrgaCount { get; set; }
    }
}
