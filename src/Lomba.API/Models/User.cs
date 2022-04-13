using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lomba.API.Models
{
    public class User : Base
    {
        [Key]
        [Display(Name = "Id de usuario")]
        public override Guid Id { get; set; }

        [Required]
        [Display(Name = "Nombre de usuario")]
        [StringLength(20)]
        public string? Username { get; set; }

        [StringLength(100)]
        [Display(Name = "Nombre y Apellido")]
        public string? Name { get; set; }

        [StringLength(200)]
        [Display(Name = "Email de usuario")]
        public string? Email { get; set; }

        [JsonIgnore]
        public byte[] PasswordHash { get; set; }
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }
    }
}
