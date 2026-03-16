using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VentaMusical.Models.ViewModels.Canciones
{
	public class DetallesCancion
	{
        /* ATRIBUTOS QUE CORRESPONDEN A CANCION */

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


        /* ATRIBUTOS QUE CORRESPONDEN A ALBUMES */
        public string NombreAlbum { get; set; } 
        public string ImagenAlbum{ get; set; }
        public int Anno{ get; set; }


        /* ATRIBUTOS QUE CORRESPONDEN A ARTISTA */
        public int CodigoArtista { get; set; }
        public string NombreArtistico { get; set; }
        public string FechaNacimiento { get; set; }
        public string NombreReal { get; set; }
        public string Nacionalidad { get; set; }
        public string Foto { get; set; }
        public string LinkBiografia { get; set; }

        /* ATRIBUTOS ADICIONALES */
        public string NombreGenero { get; set; }

    }
}