using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Lomba.API.ViewModels
{
    public class UserInput
    {
        [Display(Name = "Id de usuario")]
        public Guid Id { get; set; } = new Guid();

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

        [Required]
        [Display(Name = "Contraseña")]
        [StringLength(200)]
        public string? Password { get; set; }
    }
}
