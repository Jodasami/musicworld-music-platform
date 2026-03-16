using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VentaMusical.Models.ViewModels.Carritos
{
    public class ResumenFactura
    {
        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Recargo { get; set; }
        public decimal TotalFinal { get; set; }
        public decimal CreditoUsado { get; set; }
        public decimal TotalConCredito { get; set; }
        public decimal MontoRestante { get; set; }
        public string CodigoTarjeta { get; set; }
        public string TipoTarjeta { get; set; }
        public string TipoPago { get; set; }
        public decimal MontoPagado => TotalFinal - MontoRestante;

    }
}