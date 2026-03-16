using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VentaMusical.Models.ViewModels.Canciones
{
	public class CrearCancion
	{
        [Required(ErrorMessage = "Debe seleccionar un genero")]
        [Display(Name = "Cod. Género")]
        public int CodigoGenero { get; set; }


        [Required(ErrorMessage = "Debe seleccionar un album")]
        [Display(Name = "Cod. Album")]
        public int CodigoAlbum { get; set; }


        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        public string NombreCancion { get; set; }


        [Required(ErrorMessage = "El link es obligatorio")]
        [Display(Name = "Link (embed)")]
        [RegularExpression(@".*/embed/.*", ErrorMessage = "El link debe ser un link embebido válido (de YouTube, Spotify, etc.)")]
        public string LinkVideo { get; set; }


        [Required(ErrorMessage = "El precio es obligatorio")]
        [Display(Name = "Precio")]
        public decimal Precio { get; set; }


        [Required(ErrorMessage = "El minimo debe de ser 0")]
        [Display(Name = "Disponible")]
        public int CantidadDisponible { get; set; }


        /* ATRIBUTOS PARA LLAMARLOS A LA VISTA EN FORMA DE LISTA */
        public IEnumerable<SelectListItem> GenerosDisponibles { get; set; }

        public IEnumerable<SelectListItem> AlbumesDisponibles { get; set; }

    }
}