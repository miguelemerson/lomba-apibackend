using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        /// <summary>
        /// Id de Organización.
        /// </summary>
        [Required]
        [Display(Name = "Identificador Organización")]
        public string OrgaId { get; set; }
    }
}
