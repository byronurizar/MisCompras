using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MisCrompras.Models
{
    public class Municipio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Display(Prompt ="Ingrese el nombre del municipio",Name ="Nombre")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Descripcion { get; set; }

        [Display(Name = "Departamento")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int DepartamentoId { get; set; }


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
        public Departamento Departamento { get; set; }
    }
}
