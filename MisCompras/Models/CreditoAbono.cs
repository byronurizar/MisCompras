using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MisCrompras.Models
{
    public class CreditoAbono
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int ClienteId { get; set; }

        [Display(Name = "Monto", Prompt = "Por favor digite el monto")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public decimal Monto { get; set; }

        //0 Credito
        //1 Abono
        [Display(Name = "Tipo", Prompt = "Seleccione el tipo")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int Tipo { get; set; }

        [Display(Name = "Descripción", Prompt = "Ingrese una descripción")]
        public string Descripcion { get; set; }

       
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
        public Cliente Cliente { get; set; }
    }
}
