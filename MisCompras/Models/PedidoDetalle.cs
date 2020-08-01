using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MisCrompras.Models
{
    public class PedidoDetalle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Descripción", Prompt = "Descripción de producto")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Descripcion { get; set; }

        [Display(Name = "Número de página", Prompt = "Ingrese el número de la pagina")]
        public int Pagina { get; set; }

        [Display(Name = "Talla", Prompt = "Ingrese la talla")]
        public string Talla { get; set; }

        [Display(Name = "Color", Prompt = "Ingrese el color")]
        public string Color { get; set; }

        [Display(Name = "Precio de compra", Prompt = "Ingrese el precio de compra")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public decimal PrecioCompra { get; set; }

        [Display(Name = "Cantidad", Prompt = "Ingrese la cantidad")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int Cantidad { get; set; }

        [Display(Name = "Pedido")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int PedidoId { get; set; }

        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int ClienteId { get; set; }

        [Display(Name = "Marca")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int MarcaId { get; set; }

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
        public Marca Marca { get; set; }
        public Pedido Pedido { get; set; }
        public Cliente Cliente { get; set; }



    }
}
