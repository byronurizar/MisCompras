using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MisCrompras.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Prompt ="Ingrese un nombre de usuario",Name ="Usuario")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string User { get; set; }

        [Display(Prompt = "Ingrese una contraseña", Name = "Contraseña")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Password { get; set; }

        [Display(Name = "Perfil")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int PerfilId { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int EstadoId { get; set; }

        [Display(Name = "Fecha Creación")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Usuario Crea")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int UsuarioId { get; set; }

        public Usuario Usuarios { get; set; }
        public Estado Estado { get; set; }
        public Perfil Perfil { get; set; }

    }
}
