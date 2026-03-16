using VentaMusical.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VentaMusical.Models;
using VentaMusical.Models.ViewModels.Carritos;
using VentaMusical.Models.ViewModels.Usuarios;
using VentaMusical.Models.ViewModels.Ventas;
using EmailSettings = VentaMusical.Helpers.EmailSettings;
using CorreoHelper = VentaMusical.Helpers.CorreoHelper;


namespace VentaMusical.Controllers
{
    public class CarritoController : Controller
    {
        private readonly EmailSettings _emailSettings;

        public CarritoController()
        {
            _emailSettings = EmailSettingsLoader.CargarDesdeConfig();
        }

        private VentaMusicalEntities db = new VentaMusicalEntities();
        private List<CarritoItem> ObtenerCarrito()
        {
            var carrito = Session["Carrito"] as List<CarritoItem>;
            if (carrito == null)
            {
                carrito = new List<CarritoItem>();
                Session["Carrito"] = carrito;
            }
            return carrito;
        }

       

        public ActionResult Index()
        {
            // Validar sesión
            if (Session["UsuarioId"] == null)
            {
                
                Session["UsuarioId"] = 1;
            }

            int userId = Convert.ToInt32(Session["UsuarioId"]);
            var usuario = db.Usuarios.Find(userId);

            if (usuario == null)
            {
                TempData["Aviso"] = "El usuario no existe.";
                return RedirectToAction("Index", "Home");
            }

            // Mapear usuario a ViewModel
            var usuarioVM = new Usuario
            {
                IDUsuario = usuario.ID,
                Nombre = usuario.Nombre,
                Correo = usuario.CorreoElectronico,
                DineroDisponible = usuario.DineroDisponible,
                NumeroTarjeta = usuario.NumeroTarjeta,
                TipoTarjeta = usuario.TipoTarjeta
            };

            // Obtener carrito desde sesión
            var carrito = Session["Carrito"] as List<CarritoItem> ?? new List<CarritoItem>();

            // Si el carrito está vacío, mostrar vista vacía
            if (!carrito.Any())
            {
                var vistaVacia = new VaciarCarrito
                {
                    Icono = "🛒",
                    Titulo = "Tu carrito está vacío",
                    Mensaje = "¡Explora nuestra colección musical y encuentra tus canciones favoritas!",
                    Botones = new List<BotonVacioModel>
            {
                new BotonVacioModel
                {
                    Texto = "Ver Canciones",
                    Accion = "Index", 
                    Controlador = "Canciones",
                    RutaParametros = null,
                    ClaseCss = "btn btn-primary",
                    Titulo = "Explorar Canciones",
                }
            }
                };

                var modeloVacio = new VistaCarrito
                {
                    Carrito = carrito,
                    VistaVaciaInfo = vistaVacia,
                    Resumen = new ResumenFactura(),
                    Usuario = usuarioVM
                };

                return View(modeloVacio);
            }

            // Calcular resumen de compra
            string tipoPago = "efectivo"; 
            var resumen = CalcularTotales(carrito, usuarioVM, tipoPago);

            resumen.CodigoTarjeta = !string.IsNullOrWhiteSpace(usuarioVM.NumeroTarjeta)
                ? "****" + usuarioVM.NumeroTarjeta.Substring(usuarioVM.NumeroTarjeta.Length - 4)
                : "****1234";

           
            var model = new VistaCarrito
            {
                Carrito = carrito,
                Resumen = resumen,
                Usuario = usuarioVM,
                VistaVaciaInfo = null 
            };

            return View(model);
        }


        public ActionResult Agregar(int codigoCancion, int cantidad)
        {
            var cancion = db.Canciones.Find(codigoCancion);
            if (cancion == null || cantidad > cancion.CantidadDisponible)
                return RedirectToAction("Index");

            var carrito = ObtenerCarrito();
            var existente = carrito.FirstOrDefault(c => c.CodigoCancion == codigoCancion);

            if (existente != null)
            {
                int disponible = cancion.CantidadDisponible - existente.Cantidad;
                if (cantidad > disponible) cantidad = disponible;
                existente.Cantidad += cantidad;
            }
            else
            {
                carrito.Add(new CarritoItem
                {
                    CodigoCancion = cancion.CodigoCancion,
                    NombreCancion = cancion.NombreCancion,
                    PrecioUnitario = cancion.Precio,
                    Cantidad = cantidad
                });
            }

            return RedirectToAction("Index");
        }

        public ActionResult Eliminar(int codigoCancion)
        {
            var carrito = ObtenerCarrito();
            var item = carrito.FirstOrDefault(c => c.CodigoCancion == codigoCancion);
            if (item != null)
            {
                carrito.Remove(item);
            }
            return RedirectToAction("Index");
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Facturar(int idUsuario, string tipoPago, string codigoTarjeta, string codigoSeguridad)
        {
            return RedirectToAction("FacturarDesdeCarrito", "Ventas", new
            {
                idUsuario,
                tipoPago,
                codigoTarjeta,
                codigoSeguridad
            });
        }

        //  Cancela la compra y limpia sesión
        public ActionResult Cancelar()
        {
            Session["Carrito"] = null;
            return RedirectToAction("Index");
        }        
        public static ResumenFactura CalcularTotales(List<CarritoItem> carrito, Usuario usuario, string tipoPago)
        {
            decimal subtotal = carrito.Sum(c => c.SubTotal);
            decimal iva = Math.Round(subtotal * 0.13m, 2);

            // Calcular recargo tarjeta de crédito tiene recargo del 2%
            decimal recargo = 0;
            if (tipoPago != null && (tipoPago.Equals("Tarjeta de Crédito", StringComparison.OrdinalIgnoreCase) ||
                                    tipoPago.IndexOf("tarjeta", StringComparison.OrdinalIgnoreCase) >= 0))
            {
                // El recargo se aplica sobre el subtotal + IVA
                recargo = Math.Round((subtotal + iva) * 0.02m, 2);
            }

            decimal totalFinal = subtotal + iva + recargo;
            decimal creditoUsado = usuario.DineroDisponible >= totalFinal ? totalFinal : usuario.DineroDisponible;
            decimal montoRestante = usuario.DineroDisponible - creditoUsado;
            decimal totalConCredito = totalFinal - creditoUsado;

            return new ResumenFactura
            {
                Subtotal = subtotal,
                IVA = iva,
                Recargo = recargo,
                TotalFinal = totalFinal,
                CreditoUsado = creditoUsado,
                MontoRestante = montoRestante,
                TotalConCredito = totalConCredito
            };
        }

        [HttpPost]
        public ActionResult ActualizarCantidad(int codigoCancion, int nuevaCantidad)
        {
            try
            {
                if (Session["IDUsuario"] == null)
                {
                    return Json(new { success = false, message = "Debe iniciar sesión" });
                }

                if (nuevaCantidad <= 0)
                {
                    return Json(new { success = false, message = "Cantidad debe ser mayor a 0" });
                }

                // Obtener carrito de la sesión
                var carrito = Session["Carrito"] as List<CarritoItem>;
                if (carrito == null)
                {
                    return Json(new { success = false, message = "Carrito vacío" });
                }

                // Buscar el item en el carrito
                var item = carrito.FirstOrDefault(c => c.CodigoCancion == codigoCancion);
                if (item == null)
                {
                    return Json(new { success = false, message = "Producto no encontrado en carrito" });
                }

                // Verificar disponibilidad en base de datos
                using (var db = new VentaMusicalEntities())
                {
                    var cancion = db.Canciones.FirstOrDefault(c => c.CodigoCancion == codigoCancion);
                    if (cancion == null)
                    {
                        return Json(new { success = false, message = "Canción no encontrada" });
                    }

                    if (nuevaCantidad > cancion.CantidadDisponible)
                    {
                        return Json(new
                        {
                            success = false,
                            message = $"Solo hay {cancion.CantidadDisponible} unidades disponibles"
                        });
                    }

                    // Actualizar cantidad
                    item.Cantidad = nuevaCantidad;

                    // Actualizar sesión
                    Session["Carrito"] = carrito;

                    return Json(new
                    {
                        success = true,
                        message = "Cantidad actualizada",
                        nuevaCantidad = nuevaCantidad,
                        nuevoSubTotal = item.SubTotal.ToString("C")
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
        private bool EnviarFacturaPorCorreo(Venta venta, string correoDestino = null)
        {
            try
            {
                // Generar PDF
                var pdfResult = new Rotativa.ViewAsPdf("FacturaPDF", venta)
                {
                    FileName = $"Factura_{venta.NumeroFactura}.pdf"
                };
                byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);

                
                string emailDestino = correoDestino ?? venta.Usuarios?.Correo;

                if (string.IsNullOrEmpty(emailDestino))
                {
                    throw new ArgumentException("No se pudo determinar el email de destino");
                }

                // Crear objeto temporal para mantener compatibilidad
                var ventaTemp = new Venta
                {
                    NumeroFactura = venta.NumeroFactura,
                    FechaCompra = venta.FechaCompra,
                    TotalFinal = venta.TotalFinal,
                    Usuarios = new Usuario
                    {
                        Correo = emailDestino,
                        Nombre = venta.Usuarios?.Nombre ?? "Cliente"
                    }
                };

                var correoHelper = new CorreoHelper(_emailSettings); 
                return correoHelper.EnviarFactura(ventaTemp, pdfBytes);
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }

    }
}


