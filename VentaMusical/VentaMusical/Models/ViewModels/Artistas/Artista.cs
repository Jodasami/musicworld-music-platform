using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VentaMusical.Models.ViewModels.Artistas
{
	public class Artista
	{
        public int CodigoArtista { get; set; }
        public string NombreArtistico { get; set; }
        public string FechaNacimiento { get; set; }
        public string NombreReal { get; set; }
        public string Nacionalidad { get; set; }
        public string Foto { get; set; }
        public string LinkBiografia { get; set; }
    }
}