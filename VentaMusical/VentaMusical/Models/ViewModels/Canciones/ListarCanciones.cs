using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VentaMusical.Models.ViewModels.Canciones
{
	public class ListarCanciones : Cancion
	{
        [Display(Name = "Cod. Canción")]
        public int CodigoCancion { get; set; }


        [Display(Name = "Cod. Género")]
        public int CodigoGenero { get; set; }


        [Display(Name = "Cod. Album")]
        public int CodigoAlbum { get; set; }


        [Display(Name = "Nombre")]
        public string NombreCancion { get; set; }


        [Display(Name = "Link")]
        public string LinkVideo { get; set; }


        [Display(Name = "Precio")]
        public decimal Precio { get; set; }


        [Display(Name = "Disponible")]
        public int CantidadDisponible { get; set; }

        /* --------------- */

        [Display(Name = "Nombre Género")]
        public string NombreGenero { get; set; }  // Nueva propiedad

        [Display(Name = "Nombre Álbum")]
        public string NombreAlbum { get; set; }   // Nueva propiedad
    }
}