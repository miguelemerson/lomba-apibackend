using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

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

        public Base()
        {
            this.Id = Guid.NewGuid();
            this.CreatedAt = DateTime.UtcNow;
        }

    }
    public interface IBase
    {
        Guid Id { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}
