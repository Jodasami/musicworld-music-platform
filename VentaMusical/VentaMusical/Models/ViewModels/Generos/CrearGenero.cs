using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VentaMusical.Models.ViewModels.Generos
{
	public class CrearGenero
	{
        [Required(ErrorMessage = "Debe incluir un nombre")]
        [Display(Name = "Descripción / Nombre")]
        public string Descripcion { get; set; }
    }
}