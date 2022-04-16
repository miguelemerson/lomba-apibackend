using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lomba.API.Models
{
    public abstract class Base : IBase
    {
        [Key]
        [Display(Name = "Identificador")]
        public virtual Guid Id { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "Fecha de última modificación")]
        public DateTime? UpdatedAt { get; set; }
        [Display(Name = "¿Está desactivado?")]
        public bool IsDisabled { get; set; }

        [Display(Name = "Fecha de expiración")]
        public DateTime? Expires { get; set; }

        public Base()
        {
            this.Id = Guid.NewGuid();
            this.CreatedAt = DateTime.UtcNow;
            this.IsDisabled = false;
            this.Expires = null;
        }
    }
    public interface IBase
    {
        Guid Id { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}
