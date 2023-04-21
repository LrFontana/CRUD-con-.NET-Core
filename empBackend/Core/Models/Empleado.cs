using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Empleado
    {
        //Variables.

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El Nombre del Empleado es Requrido.")]
        [MaxLength(50, ErrorMessage = "El nombre del Empleado no puede ser mayaor a 50 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El Apellido del Empleado es Requrido.")]
        [MaxLength(50, ErrorMessage = "El Apellido  del Empleado no puede ser mayaor a 50 caracteres.")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "La Direccion del Empleado es Requrido.")]
        [MaxLength(50, ErrorMessage = "La Direccion del Empleado no puede ser mayaor a 50 caracteres.")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El Cargo del Empleado es Requrido.")]
        [MaxLength(30, ErrorMessage = "El Cargo del Empleado no puede ser mayaor a 30 caracteres.")]
        public string Cargo { get; set; }

        [Required(ErrorMessage = "El IdCompania del Empleado es Requrido.")]
        public int CompaniaId { get; set; }

        [ForeignKey("CompaniaId")]
        public Compania Compania { get; set; }
    }
}