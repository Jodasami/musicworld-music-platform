using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VentaMusical.Models.ViewModels.Albumes
{
	public class ListarAlbumes
	{
        public int CodigoAlbum { get; set; }

        [Display(Name = "Nombre del Álbum")]
        public string NombreAlbum { get; set; }

        [Display(Name = "Año de Lanzamiento")]
        public int AnoLanzamiento { get; set; }

        [Display(Name = "Artista")]
        public string NombreArtista { get; set; }

        [Display(Name = "Portada")]
        public string ImagenRuta { get; set; }
    }
}