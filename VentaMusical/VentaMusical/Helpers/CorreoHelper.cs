using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using VentaMusical.Models.ViewModels.Ventas;

namespace VentaMusical.Helpers
{
    public class CorreoHelper
    {
        private readonly EmailSettings _emailSettings;

        public CorreoHelper(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }      

        public bool EnviarFactura(Venta venta, byte[] pdfAdjunto)
        {
            if (venta?.Usuarios == null || string.IsNullOrWhiteSpace(venta.Usuarios.Correo))
                throw new ArgumentException("Usuario o correo inválido");

            using (var cliente = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
            {
                Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                EnableSsl = _emailSettings.EnableSsl,
                Timeout = _emailSettings.TimeoutSeconds * 1000
            })
            {
                var mensaje = new MailMessage(_emailSettings.FromEmail, venta.Usuarios.Correo)
                {
                    Subject = $"Factura #{venta.NumeroFactura}",
                    Body = GenerarCuerpoFactura(venta),
                    IsBodyHtml = false
                };

                mensaje.Attachments.Add(
                    new Attachment(new MemoryStream(pdfAdjunto), $"Factura_{venta.NumeroFactura}.pdf")
                );

                try
                {
                    cliente.Send(mensaje);
                    return true;
                }
                catch (SmtpException ex)
                {
                    Console.WriteLine($"Error SMTP: {ex.Message}");
                    return false;
                }
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

        }

    }
    
