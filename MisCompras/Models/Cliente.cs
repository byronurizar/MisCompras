using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MisCrompras.Models
{
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Prompt = "Ingrese un nombre", Name = "Nombre")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Nombre { get; set; }

        [Display(Prompt = "Ingrese apellidos", Name = "Apellido")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Apellido { get; set; }

        [Display(Name = "Municipio")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string MunicipioId { get; set; }

        [Display(Prompt = "Ingrese una dirección", Name = "Dirección")]
        [MinLength(10, ErrorMessage = "La dirección debe de contener por lo menos 10 caracteres")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Direccion { get; set; }

        [Display(Prompt = "Ingrese un télefono", Name = "Teléfono")]
        [MinLength(8, ErrorMessage = "El télefono debe de contener por lo menos 8 caracteres")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Telefono { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int EstadoId { get; set; }

        [Display(Name = "Fecha Creación")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Usuario Crea")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int UsuarioId { get; set; }

        public Municipio Municipio { get; set; }
        public Usuario Usuario { get; set; }
        public Estado Estado { get; set; }
    }
}
