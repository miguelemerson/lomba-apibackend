using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
namespace Lomba.API.ViewModels
{
    public class UserLogged
    {
        [Required]
        [Display(Name = "Nombre de usuario")]
        [StringLength(200)]
        public string? Username { get; set; }

        [Required]
        [Display(Name = "Token")]
        public string? Token { get; set; }
    }
}
