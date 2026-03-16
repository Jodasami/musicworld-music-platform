using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using VentaMusical.Models;
using VentaMusical.Models.ViewModels.Canciones;
using VentaMusical.Models.ViewModels.DetallesVentas;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace VentaMusical.Controllers
{
    public class DetallesVentasController : Controller
    {
        private VentaMusicalEntities db = new VentaMusicalEntities();
        // GET: DetallesVentas
        public ActionResult Index()
        {
            var detalles = (from d in db.DetalleVenta
                            join c in db.Canciones on d.CodigoCancion equals c.CodigoCancion
                            select new VentaMusical.Models.ViewModels.DetallesVentas.DetalleVentaViewModel
                            {
                                IDDetalle = d.IDDetalle,
                                NumeroFactura = d.NumeroFactura,
                                CodigoCancion = d.CodigoCancion,
                                NombreCancion = c.NombreCancion,
                                PrecioUnitario = c.Precio,
                                Cantidad = d.Cantidad,
                                SubTotal = d.Cantidad * c.Precio
                            }).ToList();

            return View(detalles);
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            
            var detalleVM = (from d in db.DetalleVenta
                             join c in db.Canciones on d.CodigoCancion equals c.CodigoCancion
                             where d.IDDetalle == id
                             select new DetalleVentaViewModel
                             {
                                 IDDetalle = d.IDDetalle,
                                 NumeroFactura = d.NumeroFactura,
                                 CodigoCancion = d.CodigoCancion,
                                 NombreCancion = c.NombreCancion,
                                 PrecioUnitario = c.Precio,
                                 Cantidad = d.Cantidad,
                                 SubTotal = d.Cantidad * c.Precio
                             }).FirstOrDefault();

            if (detalleVM == null)
            {
                return HttpNotFound();
            }

            return View(detalleVM);
        }

        public ActionResult Create([Bind(Include = "NumeroFactura,CodigoCancion,Cantidad")] Models.DetalleVenta detalle)
        {
            var cancion = db.Canciones.Find(detalle.CodigoCancion);
            if (cancion != null)
            {
                detalle.NombreCancion = cancion.NombreCancion;
                detalle.PrecioUnitario = cancion.Precio;
                detalle.Subtotal = detalle.Cantidad * cancion.Precio;
            }
            if (ModelState.IsValid)
            {
                db.DetalleVenta.Add(detalle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NumeroFactura = new SelectList(db.Ventas, "NumeroFactura", "NumeroFactura", detalle.NumeroFactura);
            ViewBag.CodigoCancion = new SelectList(db.Canciones, "CodigoCancion", "NombreCancion", detalle.CodigoCancion);
            return View(detalle);
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            
            var detalleVM = (from d in db.DetalleVenta
                             join c in db.Canciones on d.CodigoCancion equals c.CodigoCancion
                             where d.IDDetalle == id
                             select new DetalleVentaViewModel
                             {
                                 IDDetalle = d.IDDetalle,
                                 NumeroFactura = d.NumeroFactura,
                                 CodigoCancion = d.CodigoCancion,
                                 NombreCancion = c.NombreCancion,
                                 PrecioUnitario = c.Precio,
                                 Cantidad = d.Cantidad,
                                 SubTotal = d.Cantidad * c.Precio
                             }).FirstOrDefault();

            if (detalleVM == null)
            {
                return HttpNotFound();
            }

            ViewBag.NumeroFactura = new SelectList(db.Ventas, "NumeroFactura", "NumeroFactura", detalleVM.NumeroFactura);
            ViewBag.CodigoCancion = new SelectList(db.Canciones, "CodigoCancion", "NombreCancion", detalleVM.CodigoCancion);
            return View(detalleVM);
        }

        // POST: DetallesVentas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DetalleVentaViewModel detalleVM)
        {
            if (ModelState.IsValid)
            {
                // Obtener la entidad original
                var detalleEntity = db.DetalleVenta.Find(detalleVM.IDDetalle);
                if (detalleEntity == null)
                {
                    return HttpNotFound();
                }

                // Actualizar los campos editables
                detalleEntity.NumeroFactura = detalleVM.NumeroFactura;
                detalleEntity.CodigoCancion = detalleVM.CodigoCancion;
                detalleEntity.Cantidad = detalleVM.Cantidad;

                // Obtener datos de la canción
                var cancion = db.Canciones.Find(detalleVM.CodigoCancion);
                if (cancion != null)
                {
                    detalleEntity.NombreCancion = cancion.NombreCancion;
                    detalleEntity.PrecioUnitario = cancion.Precio;
                    detalleEntity.Subtotal = detalleVM.Cantidad * cancion.Precio;
                }

                db.Entry(detalleEntity).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Si hay errores, recargar los ViewBag
            ViewBag.NumeroFactura = new SelectList(db.Ventas, "NumeroFactura", "NumeroFactura", detalleVM.NumeroFactura);
            ViewBag.CodigoCancion = new SelectList(db.Canciones, "CodigoCancion", "NombreCancion", detalleVM.CodigoCancion);
            return View(detalleVM);
        }

    }
}
