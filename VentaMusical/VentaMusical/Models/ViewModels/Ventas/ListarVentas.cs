using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VentaMusical.Models.ViewModels.Ventas
{
	public class ListarVentas : Venta
	{
        public int NumeroFactura { get; set; }
        public string NombreUsuario { get; set; }
        public DateTime FechaCompra { get; set; }
        public decimal TotalSinIVA { get; set; }
        public decimal IVA { get; set; }
        public decimal RecargoTarjeta { get; set; }
        public decimal TotalFinal { get; set; }
        public string TipoPago { get; set; }
        public string CodigoTarjeta { get; set; }
        public string Estado { get; set; }
    }
}