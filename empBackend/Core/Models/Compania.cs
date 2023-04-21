using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Compania
    {
        //Variables.

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El Nombre de la Compañia es Requrido.")]
        [MaxLength(100, ErrorMessage = "El nombre de la compañia no puede ser mayaor a 100 caracteres.")]
        public string NombreCompania { get; set; }

        [Required(ErrorMessage = "La Direccion de la Compañia es Requrido.")]
        [MaxLength(50, ErrorMessage = "La direccion de la compañia no puede ser mayaor a 50 caracteres.")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El Telelfono de la Compañia es Requrido.")]
        [MaxLength(30, ErrorMessage = "El telelfono no puede ser mayaor a 30 caracteres.")]
        public string Telefono { get; set; }

        [MaxLength(30, ErrorMessage = "El telelfono 2 no puede ser mayaor a 30 caracteres.")]
        public string Telefono2 { get; set; }
        
    }
}