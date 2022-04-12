using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Lomba.API.ViewModels
{
    public class UserAuth
    {
        [Required]
        [Display(Name = "Nombre de usuario")]
        [StringLength(200)]
        public string? Username { get; set; }

        [Required]
        [Display(Name = "Contraseña")]
        [StringLength(200)]
        public string? Password { get; set; }
    }
}
