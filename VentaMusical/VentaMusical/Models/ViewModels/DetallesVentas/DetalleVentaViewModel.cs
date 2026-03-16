using System;
using VentaMusical.Models.ViewModels.Canciones;
using VentaMusical.Models.ViewModels.Ventas;

namespace VentaMusical.Models.ViewModels.DetallesVentas
{
	public class DetalleVentaViewModel
	{
        public int IDDetalle { get; set; }
        public int NumeroFactura { get; set; }
        public int CodigoCancion { get; set; }
        public string NombreCancion { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public decimal SubTotal { get; set; }
        public virtual Cancion Cancion { get; set; }

        public decimal Subtotal
        {
            get { return Cantidad * PrecioUnitario; }
        }


    }
}