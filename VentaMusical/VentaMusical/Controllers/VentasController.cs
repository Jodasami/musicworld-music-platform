using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using VentaMusical.Helpers;
using VentaMusical.Models;
using VentaMusical.Models.ViewModels.Carritos;
using VentaMusical.Models.ViewModels.CarritoValidator;
using VentaMusical.Models.ViewModels.DetallesVentas;
using VentaMusical.Models.ViewModels.Usuarios;
using VentaMusical.Models.ViewModels.Ventas;
using EmailSettings = VentaMusical.Helpers.EmailSettings;


namespace VentaMusical.Controllers
{
    public class VentasController : Controller
    {

        private readonly EmailSettings _emailSettings;

        public VentasController()
        {
            _emailSettings = EmailSettingsLoader.CargarDesdeConfig();
        }

        private VentaMusicalEntities db = new VentaMusicalEntities();

        // GET: Ventas/Index
        public ActionResult Index(string busquedaFactura, string estado, string tipoPago, DateTime? fechaInicio, DateTime? fechaFin, int page = 1, int pageSize = 10)
        {
            try
            {
                var ventasFiltradas = db.Ventas.AsQueryable();

                // Filtro por usuario normal
                if (Session["Usuario"] != null && Session["Rol"].Equals("Usuario"))
                {
                    // Supongo que Session["UsuarioID"] contiene el ID del usuario logueado
                    int usuarioLogueadoId = Convert.ToInt32(Session["UsuarioID"]);
                    ventasFiltradas = ventasFiltradas.Where(v => v.IDUsuario == usuarioLogueadoId);
                }

                // Aplicar filtros adicionales
                if (!string.IsNullOrEmpty(busquedaFactura))
                {
                    if (int.TryParse(busquedaFactura, out int numero))
                    {
                        ventasFiltradas = ventasFiltradas.Where(v => v.NumeroFactura == numero);
                    }
                    else
                    {
                        // Buscar por IDUsuario como cadena
                        ventasFiltradas = ventasFiltradas.Where(v => v.IDUsuario.ToString().Contains(busquedaFactura));
                    }
                }

                if (!string.IsNullOrEmpty(estado))
                {
                    ventasFiltradas = ventasFiltradas.Where(v => v.Estado == estado);
                }

                if (!string.IsNullOrEmpty(tipoPago))
                {
                    ventasFiltradas = ventasFiltradas.Where(v => v.TipoPago == tipoPago);
                }

                if (fechaInicio.HasValue)
                {
                    ventasFiltradas = ventasFiltradas.Where(v => v.FechaCompra >= fechaInicio.Value);
                }

                if (fechaFin.HasValue)
                {
                    ventasFiltradas = ventasFiltradas.Where(v => v.FechaCompra <= fechaFin.Value);
                }

                // Ordenar y paginar
                ventasFiltradas = ventasFiltradas.OrderByDescending(v => v.FechaCompra);
                var totalRecords = ventasFiltradas.Count();
                var ventas = ventasFiltradas.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var usuariosIds = ventas.Select(v => v.IDUsuario).Distinct().ToList();
                var usuarios = db.Usuarios.Where(u => usuariosIds.Contains(u.ID))
                                          .ToDictionary(u => u.ID, u => u.Nombre);

                var viewModel = ventas.Select(v => new ListarVentas
                {
                    NumeroFactura = v.NumeroFactura,
                    IDUsuario = v.IDUsuario,
                    FechaCompra = v.FechaCompra,
                    TotalSinIVA = v.TotalSinIVA,
                    IVA = v.IVA,
                    RecargoTarjeta = v.RecargoTarjeta,
                    TotalFinal = v.TotalFinal,
                    TipoPago = v.TipoPago,
                    CodigoTarjeta = v.CodigoTarjeta,
                    Estado = v.Estado,
                    EnviadaPorCorreo = v.EnviadaPorCorreo ?? false,
                    NombreUsuario = usuarios.ContainsKey(v.IDUsuario) ? usuarios[v.IDUsuario] : "N/A"
                }).ToList();

                // ViewBag para filtros y paginación
                ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                ViewBag.CurrentPage = page;
                ViewBag.BusquedaFactura = busquedaFactura;
                ViewBag.Estado = estado;
                ViewBag.TipoPago = tipoPago;
                ViewBag.FechaInicio = fechaInicio?.ToString("yyyy-MM-dd");
                ViewBag.FechaFin = fechaFin?.ToString("yyyy-MM-dd");

                return View(viewModel);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en Index: {ex.Message}");
                TempData["Error"] = "Error al cargar las ventas: " + ex.Message;
                return View(new List<ListarVentas>());
            }
        }


        // GET: Ventas/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                // 1️⃣ Obtener la venta principal
                var venta = db.Ventas.FirstOrDefault(v => v.NumeroFactura == id.Value);
                if (venta == null)
                    return HttpNotFound();

                // 2️⃣ Obtener el usuario
                var usuario = db.Usuarios.Find(venta.IDUsuario);

                // 3️⃣ Obtener los detalles de la venta y canciones
                var detalles = (from d in db.DetalleVenta
                                join c in db.Canciones on d.CodigoCancion equals c.CodigoCancion
                                where d.NumeroFactura == venta.NumeroFactura
                                select new DetalleVentaViewModel
                                {
                                    IDDetalle = d.IDDetalle,
                                    NumeroFactura = d.NumeroFactura,
                                    CodigoCancion = c.CodigoCancion,
                                    NombreCancion = c.NombreCancion,
                                    Cantidad = d.Cantidad,
                                    PrecioUnitario = c.Precio
                                }).ToList();

                // 4️⃣ Mapear de DetalleVentaViewModel a ListarDetallesVentas
                var detallesParaVenta = detalles.Select(d => new ListarDetallesVentas
                {
                    IDDetalle = d.IDDetalle,
                    NumeroFactura = d.NumeroFactura,
                    CodigoCancion = d.CodigoCancion,
                    NombreCancion = d.NombreCancion,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    SubTotal = d.Subtotal
                }).ToList();

                // 5️⃣ Construir ViewModel Venta
                var ventaVM = new Venta
                {
                    NumeroFactura = venta.NumeroFactura,
                    IDUsuario = venta.IDUsuario,
                    FechaCompra = venta.FechaCompra,
                    TotalSinIVA = venta.TotalSinIVA,
                    IVA = venta.IVA,
                    RecargoTarjeta = venta.RecargoTarjeta,
                    TotalFinal = venta.TotalFinal,
                    TipoPago = venta.TipoPago,
                    CodigoTarjeta = venta.CodigoTarjeta,
                    MontoCreditoUsado = venta.MontoCreditoUsado ?? 0,
                    MontoRestante = venta.MontoRestante ?? 0,
                    Estado = venta.Estado,
                    EnviadaPorCorreo = venta.EnviadaPorCorreo ?? false,
                    FechaEnvioCorreo = venta.FechaEnvioCorreo ?? DateTime.MinValue,
                    Usuarios = usuario != null ? ConstruirUsuarioVM(usuario) : null,

                    // Asignamos la lista de detalles mapeada
                    Detalle = detallesParaVenta
                };

                // 6️⃣ Opcional: pasar el nombre y precio de la primera canción al ViewBag
                if (detallesParaVenta.Any())
                {
                    ViewBag.NombreCancion = detallesParaVenta.First().NombreCancion;
                    ViewBag.PrecioCancion = detallesParaVenta.First().SubTotal;
                }

                return View(ventaVM);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en Details: {ex.Message}");
                TempData["Error"] = "Error al cargar los detalles: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: Ventas/Create
        public ActionResult Create()
        {
            try
            {
                CargarUsuarios();
                CargarCanciones();

                var venta = new Venta
                {
                    FechaCompra = DateTime.Now,
                    Estado = "Pendiente",
                    EnviadaPorCorreo = false
                };

                return View(venta);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar el formulario: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Ventas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Venta venta)
        {
            try
            {
                
                if (venta.IDUsuario <= 0)
                {
                    ModelState.AddModelError("IDUsuario", "Debe seleccionar un usuario válido.");
                }

                if (venta.CodigoCancion <= 0)
                {
                    ModelState.AddModelError("CodigoCancion", "Debe seleccionar una canción válida.");
                }

                if (string.IsNullOrWhiteSpace(venta.TipoPago))
                {
                    ModelState.AddModelError("TipoPago", "Debe seleccionar un método de pago.");
                }

                if (!ModelState.IsValid)
                {
                    CargarUsuarios(venta.IDUsuario);
                    CargarCanciones(venta.CodigoCancion);
                    ViewBag.MensajeProceso = "❌ Debe completar todos los campos obligatorios";
                    ViewBag.ValorMensaje = 0;
                    return View(venta);
                }

                // Obtener usuario
                var usuarioEF = db.Usuarios.Find(venta.IDUsuario);
                if (usuarioEF == null)
                {
                    ModelState.AddModelError("IDUsuario", "Usuario no encontrado.");
                    CargarUsuarios();
                    CargarCanciones(venta.CodigoCancion);
                    return View(venta);
                }

                // Obtener canción
                var cancionSeleccionada = db.Canciones.Find(venta.CodigoCancion);
                if (cancionSeleccionada == null)
                {
                    ModelState.AddModelError("CodigoCancion", "Canción no encontrada.");
                    CargarUsuarios(venta.IDUsuario);
                    CargarCanciones();
                    return View(venta);
                }

                // Verificar stock
                if (cancionSeleccionada.CantidadDisponible < 1)
                {
                    ModelState.AddModelError("CodigoCancion", "No hay stock disponible de esta canción.");
                    CargarUsuarios(venta.IDUsuario);
                    CargarCanciones(venta.CodigoCancion);
                    return View(venta);
                }

                // CÁLCULOS AUTOMÁTICOS - SIN VALIDACIONES DE MONTOS
                var usuarioVM = ConstruirUsuarioVM(usuarioEF);
                var carrito = new List<CarritoItem>
                {
                    new CarritoItem
                    {
                        CodigoCancion = cancionSeleccionada.CodigoCancion,
                        NombreCancion = cancionSeleccionada.NombreCancion,
                        PrecioUnitario = cancionSeleccionada.Precio,
                        Cantidad = 1
                    }
                };

                // Calcular totales automáticamente
                var resumen = CalcularTotales(carrito, usuarioVM, venta.TipoPago);

                // Crear venta con valores calculados
                var nuevaVenta = new Ventas
                {
                    IDUsuario = venta.IDUsuario,
                    FechaCompra = DateTime.Now,
                    TotalSinIVA = resumen.Subtotal,
                    IVA = resumen.IVA,
                    RecargoTarjeta = resumen.Recargo,
                    TotalFinal = resumen.TotalFinal,
                    TipoPago = venta.TipoPago,
                    CodigoTarjeta = ExtraerUltimos4Digitos(venta.CodigoTarjeta ?? usuarioEF.NumeroTarjeta),
                    MontoCreditoUsado = resumen.CreditoUsado,
                    MontoRestante = resumen.MontoRestante,
                    Estado = "Completada",
                    EnviadaPorCorreo = venta.EnviadaPorCorreo,
                    FechaEnvioCorreo = venta.EnviadaPorCorreo ? DateTime.Now : (DateTime?)null,
                    CodigoCancion = venta.CodigoCancion
                };

                // Guardar en base de datos
                db.Ventas.Add(nuevaVenta);
                db.SaveChanges();

                // Guardar detalles
                GuardarDetalles(carrito, nuevaVenta.NumeroFactura);

                // Actualizar stock
                ActualizarStock(carrito);

                // Actualizar dinero disponible del usuario
                usuarioEF.DineroDisponible = resumen.MontoRestante;
                db.SaveChanges();

                // Enviar por correo si está marcado
                if (venta.EnviadaPorCorreo)
                {
                    EnviarFacturaPorCorreo(nuevaVenta);
                }

                TempData["Success"] = "✅ Venta registrada exitosamente";
                return RedirectToAction("Details", new { id = nuevaVenta.NumeroFactura });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en Create: {ex.Message}");
                ViewBag.MensajeProceso = "❌ Error al procesar la venta: " + ex.Message;
                ViewBag.ValorMensaje = 0;
                CargarUsuarios(venta.IDUsuario);
                CargarCanciones(venta.CodigoCancion);
                return View(venta);
            }
        }

        
        public ActionResult FacturarDesdeCarrito(int idUsuario, string tipoPago, string codigoTarjeta, string codigoSeguridad)
        {
            try
            {
                var carrito = Session["Carrito"] as List<CarritoItem>;
                if (carrito == null || !carrito.Any())
                {
                    TempData["Error"] = "No hay canciones en el carrito para facturar.";
                    return RedirectToAction("Index", "Carrito");
                }

                var usuarioEF = db.Usuarios.Find(idUsuario);
                if (usuarioEF == null)
                {
                    TempData["Error"] = "Usuario no encontrado.";
                    return RedirectToAction("Index", "Carrito");
                }

                // Validar stock de todas las canciones
                foreach (var item in carrito)
                {
                    var cancion = db.Canciones.Find(item.CodigoCancion);
                    if (cancion == null || item.Cantidad > cancion.CantidadDisponible)
                    {
                        TempData["Error"] = $"Stock insuficiente para la canción: {item.NombreCancion}";
                        return RedirectToAction("Index", "Carrito");
                    }
                }

                var usuarioVM = ConstruirUsuarioVM(usuarioEF);

                
                var resumen = CalcularTotales(carrito, usuarioVM, tipoPago);

                // Crear venta
                var venta = new Ventas
                {
                    IDUsuario = idUsuario,
                    FechaCompra = DateTime.Now,
                    TotalSinIVA = resumen.Subtotal,
                    IVA = resumen.IVA,
                    RecargoTarjeta = resumen.Recargo,
                    TotalFinal = resumen.TotalFinal,
                    TipoPago = tipoPago ?? "Tarjeta",
                    CodigoTarjeta = ExtraerUltimos4Digitos(codigoTarjeta ?? usuarioEF.NumeroTarjeta),
                    MontoCreditoUsado = resumen.CreditoUsado,
                    MontoRestante = resumen.MontoRestante,
                    Estado = "Completada",
                    EnviadaPorCorreo = false
                };

                db.Ventas.Add(venta);
                db.SaveChanges();

                // Guardar detalles y actualizar stock
                GuardarDetalles(carrito, venta.NumeroFactura);
                ActualizarStock(carrito);
                usuarioEF.DineroDisponible = resumen.MontoRestante;
                db.SaveChanges();

                // Generar PDF y enviar correo
                var ventaVM = GenerarFacturaVM(venta, usuarioVM);
                var pdfResult = new Rotativa.ViewAsPdf("FacturaPDF", ventaVM)
                {
                    FileName = $"Factura_{venta.NumeroFactura}.pdf"
                };
                byte[] pdfAdjunto = pdfResult.BuildFile(ControllerContext);

                var correoHelper = new CorreoHelper(_emailSettings);
                bool enviado = correoHelper.EnviarFactura(ventaVM, pdfAdjunto);

                if (enviado)
                {
                    venta.EnviadaPorCorreo = true;
                    venta.FechaEnvioCorreo = DateTime.Now;
                    db.SaveChanges();
                }

                Session["Carrito"] = null;
                TempData["Success"] = "✅ Compra realizada exitosamente";
                return RedirectToAction("Details", new { id = venta.NumeroFactura });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en FacturarDesdeCarrito: {ex.Message}");
                TempData["Error"] = "Error al procesar la compra: " + ex.Message;
                return RedirectToAction("Index", "Carrito");
            }
        }

        // GET: Ventas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                var ventaEntity = db.Ventas.Find(id);
                if (ventaEntity == null)
                {
                    return HttpNotFound();
                }

                
                var ventaVM = new Venta
                {
                    NumeroFactura = ventaEntity.NumeroFactura,
                    IDUsuario = ventaEntity.IDUsuario,
                    FechaCompra = ventaEntity.FechaCompra,
                    TotalSinIVA = ventaEntity.TotalSinIVA,
                    IVA = ventaEntity.IVA,
                    RecargoTarjeta = ventaEntity.RecargoTarjeta,
                    TotalFinal = ventaEntity.TotalFinal,
                    TipoPago = ventaEntity.TipoPago,
                    CodigoTarjeta = ventaEntity.CodigoTarjeta,
                    MontoCreditoUsado = ventaEntity.MontoCreditoUsado ?? 0,
                    MontoRestante = ventaEntity.MontoRestante ?? 0,
                    Estado = ventaEntity.Estado,
                    EnviadaPorCorreo = ventaEntity.EnviadaPorCorreo ?? false,
                    CodigoCancion = ventaEntity.CodigoCancion
                };

                // Cargar listas para dropdowns
                CargarUsuarios(ventaEntity.IDUsuario);
                CargarCanciones(ventaEntity.CodigoCancion);

                return View(ventaVM);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar la venta: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Ventas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Venta venta)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    CargarUsuarios(venta.IDUsuario);
                    CargarCanciones(venta.CodigoCancion);
                    ViewBag.MensajeProceso = "Por favor corrija los errores en el formulario.";
                    ViewBag.ValorMensaje = 0;
                    return View(venta);
                }

                var ventaActual = db.Ventas.Find(venta.NumeroFactura);
                if (ventaActual == null)
                {
                    ViewBag.MensajeProceso = "La venta no existe o fue eliminada.";
                    ViewBag.ValorMensaje = 0;
                    CargarUsuarios(venta.IDUsuario);
                    CargarCanciones(venta.CodigoCancion);
                    return View(venta);
                }

                // Actualizar campos
                ventaActual.IDUsuario = venta.IDUsuario;
                ventaActual.TotalSinIVA = venta.TotalSinIVA;
                ventaActual.IVA = venta.IVA;
                ventaActual.RecargoTarjeta = venta.RecargoTarjeta;
                ventaActual.TotalFinal = venta.TotalFinal;
                ventaActual.TipoPago = venta.TipoPago;
                ventaActual.CodigoTarjeta = venta.CodigoTarjeta;
                ventaActual.MontoCreditoUsado = venta.MontoCreditoUsado;
                ventaActual.MontoRestante = venta.MontoRestante;
                ventaActual.Estado = venta.Estado;
                ventaActual.EnviadaPorCorreo = venta.EnviadaPorCorreo;
                ventaActual.CodigoCancion = venta.CodigoCancion;

                db.Entry(ventaActual).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Success"] = "Venta actualizada correctamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.MensajeProceso = "Error al actualizar la venta: " + ex.Message;
                ViewBag.ValorMensaje = 0;
                CargarUsuarios(venta.IDUsuario);
                CargarCanciones(venta.CodigoCancion);
                return View(venta);
            }
        }

        // GET: Ventas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                var venta = db.Ventas.Find(id);
                if (venta == null)
                {
                    return HttpNotFound();
                }

                // Verificar si se puede eliminar (menos de 24 horas)
                if ((DateTime.Now - venta.FechaCompra).TotalHours > 24)
                {
                    TempData["Error"] = "No se puede eliminar una venta con más de 24 horas de antigüedad";
                    return RedirectToAction("Index");
                }

                var vm = new Venta
                {
                    NumeroFactura = venta.NumeroFactura,
                    FechaCompra = venta.FechaCompra,
                    TotalFinal = venta.TotalFinal,
                    TipoPago = venta.TipoPago,
                    Estado = venta.Estado
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar la venta: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var venta = db.Ventas.Find(id);
                if (venta == null)
                {
                    TempData["Error"] = "Venta no encontrada";
                    return RedirectToAction("Index");
                }

                // Verificar nuevamente si se puede eliminar
                if ((DateTime.Now - venta.FechaCompra).TotalHours > 24)
                {
                    TempData["Error"] = "No se puede eliminar una venta con más de 24 horas de antigüedad";
                    return RedirectToAction("Index");
                }

                // Eliminar detalles primero
                var detalles = db.DetalleVenta.Where(d => d.NumeroFactura == id).ToList();
                foreach (var detalle in detalles)
                {
                    db.DetalleVenta.Remove(detalle);
                }

                // Eliminar venta
                db.Ventas.Remove(venta);
                db.SaveChanges();

                TempData["Success"] = "Venta eliminada correctamente";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al eliminar la venta: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: Ventas/EnviarFactura/5
        public ActionResult EnviarFactura(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "ID de venta no válido";
                return RedirectToAction("Index");
            }

            try
            {
                var ventaEntity = db.Ventas.FirstOrDefault(v => v.NumeroFactura == id);

                if (ventaEntity == null)
                {
                    TempData["Error"] = "Factura no encontrada.";
                    return RedirectToAction("Index");
                }

                // Verificar que no esté ya enviada
                if (ventaEntity.EnviadaPorCorreo == true)
                {
                    TempData["Error"] = "La factura ya ha sido enviada por correo.";
                    return RedirectToAction("Details", new { id = id });
                }

                // Obtener usuario para verificar correo
                var usuario = db.Usuarios.Find(ventaEntity.IDUsuario);
                if (usuario == null || string.IsNullOrEmpty(usuario.CorreoElectronico))
                {
                    TempData["Error"] = "El usuario no tiene correo electrónico registrado.";
                    return RedirectToAction("Details", new { id = id });
                }

                // Crear ViewModel para la factura
                var usuarioVM = ConstruirUsuarioVM(usuario);
                var ventaViewModel = GenerarFacturaVM(ventaEntity, usuarioVM);

                // Generar PDF
                var pdfResult = new Rotativa.ViewAsPdf("FacturaPDF", ventaViewModel)
                {
                    FileName = $"Factura_{ventaEntity.NumeroFactura}.pdf"
                };

                byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);

                // Enviar correo
                var correoHelper = new CorreoHelper(_emailSettings);
                bool enviado = correoHelper.EnviarFactura(ventaViewModel, pdfBytes);

                if (enviado)
                {
                    ventaEntity.EnviadaPorCorreo = true;
                    ventaEntity.FechaEnvioCorreo = DateTime.Now;
                    db.Entry(ventaEntity).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["Success"] = "Factura enviada correctamente por correo electrónico";
                }
                else
                {
                    TempData["Error"] = "No se pudo enviar la factura por correo";
                }

                return RedirectToAction("Details", new { id = id });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en EnviarFactura: {ex.Message}");
                TempData["Error"] = "Error al enviar la factura: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public ActionResult ObtenerPrecioCancion(int id)
        {
            try
            {
                var cancion = db.Canciones.Find(id);
                if (cancion == null)
                {
                    return Json(new { error = "Canción no encontrada" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    precio = cancion.Precio,
                    nombre = cancion.NombreCancion,
                    disponible = cancion.CantidadDisponible
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        
        private Usuario ConstruirUsuarioVM(Usuarios usuarioEF)
        {
            return new Usuario
            {
                IDUsuario = usuarioEF.ID,
                NumeroIdentificacion = usuarioEF.NumeroIdentificacion,
                Nombre = usuarioEF.Nombre,
                Apellido = usuarioEF.Apellido,
                Genero = usuarioEF.Genero,
                Correo = usuarioEF.CorreoElectronico,
                TipoTarjeta = usuarioEF.TipoTarjeta,
                DineroDisponible = usuarioEF.DineroDisponible,
                NumeroTarjeta = usuarioEF.NumeroTarjeta,
                Contrasena = usuarioEF.Contrasena,
                Perfil = usuarioEF.Perfil
            };
        }

        private string ExtraerUltimos4Digitos(string numeroTarjeta)
        {
            if (string.IsNullOrWhiteSpace(numeroTarjeta))
                return "";

            var soloNumeros = numeroTarjeta.Replace("-", "").Replace(" ", "");
            return soloNumeros.Length >= 4 ? soloNumeros.Substring(soloNumeros.Length - 4) : soloNumeros;
        }

        private void GuardarDetalles(List<CarritoItem> carrito, int numeroFactura)
        {
            foreach (var item in carrito)
            {
                db.DetalleVenta.Add(new DetalleVenta
                {
                    NumeroFactura = numeroFactura,
                    CodigoCancion = item.CodigoCancion,
                    NombreCancion = item.NombreCancion,
                    PrecioUnitario = item.PrecioUnitario,
                    Cantidad = item.Cantidad,
                    Subtotal = item.SubTotal
                });
            }
        }

        private void ActualizarStock(List<CarritoItem> carrito)
        {
            foreach (var item in carrito)
            {
                var cancion = db.Canciones.Find(item.CodigoCancion);
                if (cancion != null)
                {
                    cancion.CantidadDisponible -= item.Cantidad;
                }
            }
        }

        private void EnviarFacturaPorCorreo(Ventas venta)
        {
            try
            {
                var usuario = db.Usuarios.Find(venta.IDUsuario);
                if (usuario == null) return;

                var usuarioVM = ConstruirUsuarioVM(usuario);
                var ventaVM = GenerarFacturaVM(venta, usuarioVM);

                var pdfResult = new Rotativa.ViewAsPdf("FacturaPDF", ventaVM)
                {
                    FileName = $"Factura_{venta.NumeroFactura}.pdf"
                };
                byte[] pdfBytes = pdfResult.BuildFile(ControllerContext);

                var correoHelper = new CorreoHelper(_emailSettings);
                bool enviado = correoHelper.EnviarFactura(ventaVM, pdfBytes);

                if (enviado)
                {
                    venta.EnviadaPorCorreo = true;
                    venta.FechaEnvioCorreo = DateTime.Now;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error enviando correo: {ex.Message}");
            }
        }

        private void CargarUsuarios(int? seleccionado = null)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var usuarios = db.Usuarios
                .Select(u => new { IDUsuario = u.ID, NombreUsuario = u.Nombre })
                .ToList();
            ViewBag.IDUsuario = new SelectList(usuarios, "IDUsuario", "NombreUsuario", seleccionado);
        }

        private void CargarCanciones(int? seleccionada = null)
        {
            var canciones = db.Canciones
                .Where(c => c.CantidadDisponible > 0)
                .Select(c => new { CodigoCancion = c.CodigoCancion, NombreCancion = c.NombreCancion })
                .ToList();
            ViewBag.CodigoCancion = new SelectList(canciones, "CodigoCancion", "NombreCancion", seleccionada);
        }

       
        public static ResumenFactura CalcularTotales(List<CarritoItem> carrito, Usuario usuario, string tipoPago)
        {
            decimal subtotal = carrito.Sum(c => c.SubTotal);
            decimal iva = Math.Round(subtotal * 0.13m, 2);

            // Recargo solo para tarjeta
            decimal recargo = (!string.IsNullOrEmpty(tipoPago) && tipoPago.ToLower().Contains("tarjeta"))
                ? Math.Round(subtotal * 0.02m, 2)
                : 0;

            decimal totalFinal = subtotal + iva + recargo;

            // Cálculo automático de crédito
            decimal creditoUsado = 0;
            decimal montoRestante = usuario.DineroDisponible;

            switch (tipoPago?.Trim().ToLower())
            {
                case "crédito":
                    creditoUsado = usuario.DineroDisponible >= totalFinal ? totalFinal : usuario.DineroDisponible;
                    montoRestante = usuario.DineroDisponible - creditoUsado;
                    break;
                case "tarjeta":
                case "paypal":
                case "transferencia":
                default:
                    creditoUsado = 0;
                    montoRestante = usuario.DineroDisponible; 
                    break;
            }

            return new ResumenFactura
            {
                Subtotal = subtotal,
                IVA = iva,
                Recargo = recargo,
                TotalFinal = totalFinal,
                CreditoUsado = creditoUsado,
                MontoRestante = montoRestante,
                TotalConCredito = totalFinal - creditoUsado,
                TipoPago = tipoPago
            };
        }

        public Venta GenerarFacturaVM(Ventas venta, Usuario usuarioVM)
        {
            var detalles = db.DetalleVenta
                .Where(d => d.NumeroFactura == venta.NumeroFactura)
                .Select(d => new ListarDetallesVentas
                {
                    IDDetalle = d.IDDetalle,
                    NumeroFactura = d.NumeroFactura,
                    CodigoCancion = d.CodigoCancion,
                    NombreCancion = d.NombreCancion,
                    PrecioUnitario = d.PrecioUnitario,
                    Cantidad = d.Cantidad
                    
                }).ToList();

            return new Venta
            {
                NumeroFactura = venta.NumeroFactura,
                IDUsuario = venta.IDUsuario,
                FechaCompra = venta.FechaCompra,
                TotalSinIVA = venta.TotalSinIVA,
                IVA = venta.IVA,
                RecargoTarjeta = venta.RecargoTarjeta,
                TotalFinal = venta.TotalFinal,
                TipoPago = venta.TipoPago,
                CodigoTarjeta = venta.CodigoTarjeta,
                MontoCreditoUsado = venta.MontoCreditoUsado ?? 0,
                MontoRestante = venta.MontoRestante ?? 0,
                Estado = venta.Estado,
                EnviadaPorCorreo = venta.EnviadaPorCorreo ?? false,
                Usuarios = usuarioVM,
                Detalle = detalles
            };
        }

        public ActionResult Reversar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                var venta = db.Ventas.FirstOrDefault(v => v.NumeroFactura == id);
                if (venta == null)
                {
                    TempData["Error"] = "Venta no encontrada";
                    return RedirectToAction("Index");
                }

                // Verificar si se puede reversar (menos de 24 horas)
                if ((DateTime.Now - venta.FechaCompra).TotalHours > 24)
                {
                    TempData["Error"] = "No se puede reversar una venta con más de 24 horas de antigüedad";
                    return RedirectToAction("Index");
                }

                if (venta.Estado == "Revertida")
                {
                    TempData["Error"] = "Esta venta ya ha sido revertida";
                    return RedirectToAction("Index");
                }

                // Obtener detalles para restaurar stock
                var detalles = db.DetalleVenta.Where(d => d.NumeroFactura == id).ToList();

                // Restaurar stock de canciones
                foreach (var detalle in detalles)
                {
                    var cancion = db.Canciones.Find(detalle.CodigoCancion);
                    if (cancion != null)
                    {
                        cancion.CantidadDisponible += detalle.Cantidad;
                    }
                }

                // Restaurar dinero al usuario
                var usuario = db.Usuarios.Find(venta.IDUsuario);
                if (usuario != null)
                {
                    usuario.DineroDisponible += venta.TotalFinal;
                }

                // Cambiar estado de la venta
                venta.Estado = "Revertida";

                db.SaveChanges();

                TempData["Success"] = "Venta revertida correctamente. Stock y dinero restaurados.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al reversar la venta: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
