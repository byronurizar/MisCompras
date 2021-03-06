﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MisCrompras.Models
{
    public class Categoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Prompt = "Nombre de categoria", Name = "Marca")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Nombre { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int EstadoId { get; set; }

        [Display(Name = "Fecha Creación")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Usuario Crea")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int UsuarioId { get; set; }

        [Display(Prompt = "Incluir en Gastos Familiares", Name = "Incluir")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public int Incluir { get; set; }

        public Usuario Usuario { get; set; }
        public Estado Estado { get; set; }
    }
}
