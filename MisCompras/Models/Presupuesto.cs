using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MisCrompras.Models
{
    public class Presupuesto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Prompt = "Nombre de presupuesto", Name = "Nombre")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Nombre { get; set; }


        [Display(Prompt = "Año", Name = "Año")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int Anio { get; set; }

        [Display(Prompt = "Mes", Name = "Mes")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int Mes { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int EstadoId { get; set; }

        [Display(Name = "Fecha Creación")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Usuario Crea")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; }
        public Estado Estado { get; set; }
    }
}
