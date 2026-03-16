using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VentaMusical.Models.ViewModels.Carritos
{
	public class VentaResumen
	{
        public int IDUsuario { get; set; }
        public string TipoPago { get; set; }
        public string CodigoTarjeta { get; set; }

        public List<CarritoItem> Items { get; set; }

        public decimal Subtotal => Items?.Sum(i => i.SubTotal) ?? 0;
        public decimal IVA => Math.Round(Subtotal * 0.13m, 2);
        public decimal RecargoTarjeta => TipoPago.Contains("tarjeta") ? Math.Round(Subtotal * 0.02m, 2) : 0;
        public decimal TotalFinal => Subtotal + IVA + RecargoTarjeta;
    }
}