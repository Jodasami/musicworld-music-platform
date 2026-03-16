using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VentaMusical.Models.ViewModels.Ventas
{
	public class CrearVenta
	{
        [Display(Name = "Usuario")]
        [Required]
        public int IDUsuario { get; set; }

        [Display(Name = "Subtotal sin IVA")]
        
        [Range(0.01, double.MaxValue)]
        public decimal TotalSinIVA { get; set; }

        [Display(Name = "IVA (13%)")]
       
        public decimal IVA { get; set; }

        [Display(Name = "Recargo por Tarjeta")]
        public decimal? RecargoTarjeta { get; set; }

        [Display(Name = "Total a Pagar")]
        
        public decimal TotalFinal { get; set; }

        [Display(Name = "Método de Pago")]
        [Required]
        public string TipoPago { get; set; }

        [Display(Name = "Últimos 4 dígitos de la tarjeta")]
        [StringLength(4, ErrorMessage = "Debe ingresar los últimos 4 dígitos")]
        public string CodigoTarjeta { get; set; }

        [Display(Name = "Crédito Usado")]
        public decimal? MontoCreditoUsado { get; set; }

        [Display(Name = "Crédito Restante")]
        public decimal? MontoRestante { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Display(Name = "Factura enviada por correo")]
        public bool? EnviadaPorCorreo { get; set; }
    }
}