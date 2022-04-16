using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lomba.API.Models
{
    public class Role
    {

        [Key]
        [Required]
        [Display(Name = "Código del rol")]
        [StringLength(20)]
        public string Name { get; set; }
        [Display(Name = "Descripción del rol")]
        [StringLength(200)]
        public string Description { get; set; }
        [Display(Name = "¿Está desactivado?")]
        public bool IsDisabled { get; set; }
        [Display(Name = "Fecha de última modificación")]
        public DateTime? UpdatedAt { get; set; }
        public Role()
        {
            this.IsDisabled = false;
        }

        [JsonIgnore]
        public List<OrgaUser> OrgaUsers { get; set; }
    }
}
