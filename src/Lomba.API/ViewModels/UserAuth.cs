using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lomba.API.ViewModels
{
    public class UserAuth
    {
        /// <summary>
        /// Nombre de usuario (username).
        /// </summary>
        /// <example>user3</example>
        [Required]
        [Display(Name = "Nombre de usuario")]
        [StringLength(200)]
        public string? Username { get; set; }

        /// <summary>
        /// Contraseña del usuario.
        /// </summary>
        /// <example>user3</example>
        [Required]
        [Display(Name = "Contraseña")]
        [StringLength(200)]
        public string? Password { get; set; }
    }
}
