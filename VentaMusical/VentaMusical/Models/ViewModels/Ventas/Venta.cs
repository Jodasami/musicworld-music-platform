using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using VentaMusical.Models.ViewModels.Canciones;
using VentaMusical.Models.ViewModels.DetallesVentas;
using VentaMusical.Models.ViewModels.Usuarios;

namespace VentaMusical.Models.ViewModels.Ventas
{
	public class Venta
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NumeroFactura { get; set; }
        [Required]

        [Display(Name = "Usuario")]
        public int IDUsuario { get; set; }
        [Required]

        [Display(Name = "Fecha de Compra")]
        public DateTime FechaCompra { get; set; }
        

        [Display(Name = "Subtotal")]
        public decimal TotalSinIVA { get; set; }

        [Display(Name = "IVA (13%)")]
        public decimal IVA { get; set; }

        [Display(Name = "Recargo por Tarjeta")]
        public decimal RecargoTarjeta { get; set; }
        

        [Display(Name = "Total Final")]
        public decimal TotalFinal { get; set; }

        [NotMapped]
        [Display(Name = "Total con Crédito Aplicado")]
        public decimal TotalConCredito { get; set; }
        

        [Display(Name = "Método de Pago")]
        public string TipoPago { get; set; }
        

        [Display(Name = "Últimos 4 dígitos de la tarjeta")]
        public string CodigoTarjeta { get; set; }

        [Display(Name = "Crédito Usado")]
        public decimal MontoCreditoUsado { get; set; }

        [Display(Name = "Crédito Restante")]
        public decimal MontoRestante { get; set; }
        [NotMapped]

        [Display(Name = "Estado de la Compra")]
        public string Estado { get; set; }

        public DateTime FechaEnvioCorreo { get; set; }

        [Display(Name = "Factura Enviada por Correo")]
        public bool EnviadaPorCorreo { get; set; }
        public virtual Usuario Usuarios { get; set; }
        public string CodigoSeguridad { get; set; }

        public string NumeroTarjeta { get; set; }
        public List<ListarDetallesVentas> Detalle { get; set; }

        public decimal MontoPagado => TotalFinal - MontoRestante;
        [Required]
        
        public int CodigoCancion { get; set; }
        [ForeignKey("CodigoCancion")]
        public virtual VentaMusical.Models.ViewModels.Canciones.Cancion Cancion { get; set; }
        public decimal PrecioCancion { get; set; }

    }
}