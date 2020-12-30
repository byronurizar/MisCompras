using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MisCompras.Models
{
    public class CategoriaPresupuesto
    {
        public int categoriaId { get; set; }
        public string categoria { get; set; }
        public decimal presupuesto { get; set; }
        public decimal montoUtilizado { get; set; }
        public decimal montoPendiente { get; set; }
        public int incluir { get; set; }
    }
}
