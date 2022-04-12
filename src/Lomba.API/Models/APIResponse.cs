using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Lomba.API.Models
{
    public class APIResponse
    {
        /// <summary>
        /// Código de respuesta interna.
        /// </summary>
        /// <example>1</example>
        [Display(Name = "Código de respuesta")]
        public int Code { get; set; } = 1;
        /// <summary>
        /// Mensaje o descripción del error.
        /// </summary>
        /// <example>Descripción de la respuesta</example>
        [Display(Name = "Mensaje de respuesta")]
        public string Message { get; set; } = String.Empty;
    }
}
