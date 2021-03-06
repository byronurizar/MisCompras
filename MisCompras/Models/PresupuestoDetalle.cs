﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MisCrompras.Models
{
    public class PresupuestoDetalle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Presupuesto")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int PresupuestoId { get; set; }

        [Display(Prompt = "Seleccione una categoria", Name = "Categoría")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int CategoriaId { get; set; }


        [Display(Prompt = "Monto de presupuesto mensual", Name = "Monto")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public decimal Monto { get; set; }

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
        public Presupuesto Presupuesto { get; set; }
        public Categoria Categoria { get; set; }
    }
}
