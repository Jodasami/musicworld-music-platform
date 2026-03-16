using Rotativa;
using System;
using System.Web.Mvc;
using VentaMusical.Areas.Ventas.Helpers;
using VentaMusical.Models.ViewModels.Ventas;
using EmailSettings = VentaMusical.Helpers.EmailSettings;

namespace VentaMusical.Helpers
{
    public class FacturaCorreoService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ControllerContext _controllerContext;

        public FacturaCorreoService(EmailSettings emailSettings, ControllerContext context)
        {
            _emailSettings = emailSettings;
            _controllerContext = context;
        }

        public bool EnviarFactura(Venta venta)
        {
            try
            {
                // Generar PDF con Rotativa
                var pdf = new ViewAsPdf("FacturaPDF", venta);
                byte[] pdfBytes = pdf.BuildFile(_controllerContext);

                // Usar el helper existente
                var correoHelper = new CorreoHelper(_emailSettings);
                return correoHelper.EnviarFactura(venta, pdfBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en FacturaCorreoService: {ex.Message}");
                return false;
            }
        }
    }
}