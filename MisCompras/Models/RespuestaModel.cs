using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MisCompras.Models
{
    public class RespuestaModel
    {
        public int Codigo { get; set; }
        public string mensaje { get; set; }
        public string error { get; set; }
        public object valor { get; set; }
    }
}
