using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace VentaMusical.Models.ViewModels.Carritos
{
	public class CarritoItem
	{
        public int CodigoCancion { get; set; }
        public string NombreCancion { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public decimal SubTotal => PrecioUnitario * Cantidad;

        public string NombreGenero { get; set; }
        public string NombreAlbum { get; set; }
        public int CantidadDisponible { get; set; }
        
    }
}