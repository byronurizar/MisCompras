﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MisCrompras.Models
{
    public class Marca
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Display(Prompt = "Ingrese el nombre de la marca", Name = "Marca")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Descripcion { get; set; }

        [Display(Name = "Porcentaje descuento")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public decimal PorcentajeDescuento { get; set; }

        [Display(Name = "Moneda")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Moneda { get; set; }

        [Display(Name = "Dividendo")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public decimal Dividendo { get; set; }

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
