using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VentaMusical.Models.ViewModels.Albumes
{
    public class EditarAlbum
    {
        public int CodigoAlbum { get; set; }

        [Required(ErrorMessage = "El nombre del álbum es obligatorio.")]
        [Display(Name = "Nombre del Álbum")]
        public string NombreAlbum { get; set; }

        [Required(ErrorMessage = "El año de lanzamiento es obligatorio.")]
        [Display(Name = "Año de Lanzamiento")]
        public int AnoLanzamiento { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un artista.")]
        [Display(Name = "Artista")]
        public int CodigoArtista { get; set; }

        // Enlace (URL) de la portada
        public string ImagenPortada { get; set; }

        // Para renderizar el dropdown SIN usar ViewBag
        public IEnumerable<SelectListItem> Artistas { get; set; }
     }

 }