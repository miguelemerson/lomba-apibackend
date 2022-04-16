using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lomba.API.Models
{
    public class Orga : Base
    {
        [Key]
        [Display(Name = "Id de organización")]
        public override Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Nombre de la Organización")]
        public string? Name { get; set; }

    }
}
