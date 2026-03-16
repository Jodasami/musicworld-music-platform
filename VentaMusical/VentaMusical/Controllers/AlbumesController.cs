using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VentaMusical.Models;
using VentaMusical.Models.ViewModels.Albumes;

namespace VentaMusical.Controllers
{
    public class AlbumesController : Controller
    {
        // 1. Instancia del contexto de la base de datos
        private VentaMusicalEntities db = new VentaMusicalEntities();

        // GET: Albumes (PANTALLA PRINCIPAL PARA CONSULTAR)
        public ActionResult Index(int? CodigoArtista)
        {
            // Carga la lista de artistas para el dropdown de filtro
            ViewBag.ListaDeArtistas = new SelectList(db.Artistas, "CodigoArtista", "NombreArtistico");

            // Carga la lista de álbumes
            var albumes = db.Albumes.Include(a => a.Artistas); // .Include() trae los datos del artista

            // Si se seleccionó un artista en el filtro, aplica el filtro
            if (CodigoArtista != null)
            {
                albumes = albumes.Where(a => a.CodigoArtista == CodigoArtista);
            }

            return View(albumes.ToList());
        }

        // GET: Albumes/Create (MUESTRA EL FORMULARIO PARA AGREGAR)
        public ActionResult Create()
        {
            // Carga la lista de artistas para el dropdown del formulario
            ViewBag.CodigoArtista = new SelectList(db.Artistas, "CodigoArtista", "NombreArtistico");
            return View();
        }

        // POST: Albumes/Create (GUARDA EL NUEVO ÁLBUM)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CrearAlbum viewModel)
        {
            if (ModelState.IsValid)
            {
                var albumDB = new Albumes
                {
                    CodigoArtista = viewModel.CodigoArtista,
                    NombreAlbum = viewModel.NombreAlbum,
                    AnoLanzamiento = viewModel.AnoLanzamiento,
                    Imagen = viewModel.Portada
                };

                db.Albumes.Add(albumDB);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CodigoArtista = new SelectList(db.Artistas, "CodigoArtista", "NombreArtistico", viewModel.CodigoArtista);
            return View(viewModel);
        }

        // GET: Albumes/Edit/5 (MUESTRA EL FORMULARIO PARA EDITAR)
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var albumDB = db.Albumes.Find(id);
            if (albumDB == null)
                return HttpNotFound();

            var vm = new EditarAlbum
            {
                CodigoAlbum = albumDB.CodigoAlbum,
                NombreAlbum = albumDB.NombreAlbum,
                AnoLanzamiento = albumDB.AnoLanzamiento,
                CodigoArtista = albumDB.CodigoArtista,
                ImagenPortada = albumDB.Imagen,
                Artistas = db.Artistas
             .ToList()
             .Select(a => new SelectListItem
             {
                 Value = a.CodigoArtista.ToString(),
                 Text = a.NombreArtistico
             })
             .ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditarAlbum vm)
        {
            if (!ModelState.IsValid)
            {
                // Si hubo error de validación, volver a cargar la lista para que el dropdown no se vacíe
                vm.Artistas = db.Artistas
                    .ToList()
                    .Select(a => new SelectListItem
                    {
                        Value = a.CodigoArtista.ToString(),
                        Text = a.NombreArtistico,
                        Selected = (a.CodigoArtista == vm.CodigoArtista)
                    })
                    .ToList();

                return View(vm);
            }

            var albumEnDB = db.Albumes.Find(vm.CodigoAlbum);
            if (albumEnDB == null)
                return HttpNotFound();

            albumEnDB.NombreAlbum = vm.NombreAlbum;
            albumEnDB.AnoLanzamiento = vm.AnoLanzamiento;
            albumEnDB.CodigoArtista = vm.CodigoArtista;
            albumEnDB.Imagen = vm.ImagenPortada;

            db.SaveChanges();

            TempData["Success"] = "Álbum actualizado correctamente.";
            return RedirectToAction("Index");
        }



        // GET: Albumes/Delete/5 (MUESTRA LA PÁGINA DE CONFIRMACIÓN)
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Albumes albumes = db.Albumes.Find(id);
            if (albumes == null)
            {
                return HttpNotFound();
            }
            return View(albumes);
        }

        // POST: Albumes/Delete/5 (EJECUTA LA ELIMINACIÓN)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Albumes albumes = db.Albumes.Find(id);

            db.Albumes.Remove(albumes);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Es una buena práctica liberar la conexión a la DB
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
