using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Lomba.API.ViewModels
{
    public class OrgaUserInput
    {
        [Required]
        public string OrgaId { get; set; }
        [Required]
        public string UserId { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
