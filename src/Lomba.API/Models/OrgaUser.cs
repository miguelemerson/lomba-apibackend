using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lomba.API.Models
{
    public class OrgaUser : Base
    {
        [Required]
        public Orga Orga { get; set; }
        [Required]
        public User User { get; set; }
        public List<Role> Roles { get; set; } = new List<Role>(); 
    }
}
