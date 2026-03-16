using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using VentaMusical.Models.ViewModels.Ventas;
using EmailSettings = VentaMusical.Helpers.EmailSettings;

namespace VentaMusical.Areas.Ventas.Helpers
{
    public class CorreoHelper
    {
        private readonly EmailSettings _emailSettings;

        public CorreoHelper()
        {
            _emailSettings = EmailSettings.CargarDesdeConfig();
        }

        public CorreoHelper(EmailSettings settings)
        {
            _emailSettings = settings;
        }
        

        public bool EnviarFactura(Venta venta, byte[] pdfAdjunto)
        {
            if (venta?.Usuarios == null || string.IsNullOrWhiteSpace(venta.Usuarios.Correo))
                throw new ArgumentException("Usuario o correo inválido");

            Console.WriteLine($"Enviando factura #{venta.NumeroFactura} a {venta.Usuarios.Correo}");
            Console.WriteLine($"Servidor: {_emailSettings.SmtpServer}:{_emailSettings.Port}, SSL: {_emailSettings.EnableSsl}");

            try
            {
                using (var cliente = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
                {
                    UseDefaultCredentials = false, 
                    Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                    EnableSsl = _emailSettings.EnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network, 
                    Timeout = _emailSettings.TimeoutSeconds * 1000
                })
                {
                    var mensaje = new MailMessage(_emailSettings.FromEmail, venta.Usuarios.Correo)
                    {
                        Subject = $"Factura #{venta.NumeroFactura} - {_emailSettings.FromName}",
                        Body = CrearCuerpoEmail(venta),
                        IsBodyHtml = true
                    };

                   
                    using (var pdfStream = new MemoryStream(pdfAdjunto))
                    {
                        mensaje.Attachments.Add(
                            new Attachment(pdfStream, $"Factura_{venta.NumeroFactura}.pdf", "application/pdf")
                        );

                        Console.WriteLine(" Enviando email...");
                        cliente.Send(mensaje);
                        Console.WriteLine("Factura enviada exitosamente");
                    }

                    // Limpiar recursos
                    mensaje.Dispose();
                    return true;
                }
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($" Error SMTP al enviar factura #{venta.NumeroFactura}:");
                Console.WriteLine($"   Código: {smtpEx.StatusCode}");
                Console.WriteLine($"   Mensaje: {smtpEx.Message}");

                if (smtpEx.InnerException != null)
                {
                    Console.WriteLine($"   Detalle: {smtpEx.InnerException.Message}");
                }

                // Almacenar error para mostrar al usuario
                if (HttpContext.Current?.Session != null)
                {
                    HttpContext.Current.Session["ErrorCorreo"] = $"Error SMTP: {smtpEx.Message}";
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error general enviando factura: {ex.Message}");
                return false;
            }
        }


        private string CrearCuerpoEmail(Venta venta)
        {
            return $@"
        <html>
        <body style='font-family: Arial, sans-serif;'>
            <h2>¡Gracias por tu compra!</h2>
            <p>Estimado/a <strong>{venta.Usuarios.Nombre}</strong>,</p>
            <p>Te enviamos adjunta la factura de tu compra:</p>
            <ul>
                <li><strong>Número de factura:</strong> {venta.NumeroFactura}</li>
                <li><strong>Fecha:</strong> {venta.FechaCompra:dd/MM/yyyy}</li>
                <li><strong>Total:</strong> ₡{venta.TotalFinal:N2}</li>
            </ul>
            <p>Si tienes alguna pregunta, no dudes en contactarnos.</p>
            <p>¡Esperamos verte pronto!</p>
            <br>
            <p><strong>{_emailSettings.FromName}</strong></p>
        </body>
        </html>";
        }

        private bool EsEmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private string GenerarCuerpoFactura(Venta venta)
        {
            return $@"Gracias por su compra:

        - Fecha: {venta.FechaCompra:dd/MM/yyyy}
        - Total: ₡{venta.TotalFinal:N2}
        - Método de pago: {venta.TipoPago}

        ¡Esperamos verlo pronto!";
        }


    }

}
