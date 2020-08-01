using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MisCrompras.Models
{
    public class Perfil
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Nombre", Prompt = "Ingrese un nombre para el perfil")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción",Prompt ="Ingrese una descripción para el perfil")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Descripcion { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int EstadoId { get; set; }

        [Display(Name = "Fecha Creación")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public DateTime FechaCreacion { get; set; }
        public Estado Estado { get; set; }
    }
}
