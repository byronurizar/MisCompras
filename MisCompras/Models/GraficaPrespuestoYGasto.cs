using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MisCompras.Models
{
    public class GraficaPrespuestoYGasto
    {
        public int id { get; set; }
        public string nombreMes { get; set; }
        public decimal presupuesto { get; set; }
        public decimal gasto { get; set; }
    }
}
