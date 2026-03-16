using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VentaMusical.Models;
using VentaMusical.Models.ViewModels.Generos;
using VentaMusical.Models.ViewModels.Usuarios;

namespace VentaMusical.Controllers
{
    public class GenerosController : Controller
    {
        // GET: Generos
        public ActionResult Index()
        {
            using (var db = new VentaMusicalEntities())
            {
                var lista = db.Generos.ToList();
                return View(lista);
            }
        }


        public ActionResult Details(int id)
        {
            using (var db = new VentaMusicalEntities())
            {
                var generoBD = db.Generos.Find(id);

                if (generoBD == null)
                    return HttpNotFound();

                var vm = new VentaMusical.Models.ViewModels.Generos.Genero
                {
                    CodigoGenero = generoBD.CodigoGenero,
                    Descripcion = generoBD.Descripcion
                };

                return View(vm);
            }
        }

        // GET: Generos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Generos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CrearGenero genero)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(genero);
                }
                using (VentaMusicalEntities db = new VentaMusicalEntities())
                {
                    Generos nuevoGenero = new Generos()
                    {
                        Descripcion = genero.Descripcion
                    };

                    db.Generos.Add(nuevoGenero);
                    db.SaveChanges();


                    ViewBag.MensajeProceso = "Genero creado exitosamente";
                    ViewBag.ValorMensaje = 1;
                    return View(genero);
                }
            }
            catch (Exception ex)
            {
                ViewBag.MensajeProceso = "Error al crear el genero: " + ex.Message;
                if (ex.InnerException != null)
                    ViewBag.MensajeProceso += " Detalle: " + ex.InnerException.Message;
                ViewBag.ValorMensaje = 0;
                return View(genero);
            }
        }

        // GET: Generos/Edit/5
        public ActionResult Edit(int id)
        {
            using (var db = new VentaMusicalEntities())
            {
                var genero = db.Generos.Find(id);
                if (genero == null)
                {
                    return HttpNotFound();
                }
                return View(genero);
            }
        }

        // POST: Generos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VentaMusical.Models.Generos genero)
        {
            if (ModelState.IsValid)
            {
                using (var db = new VentaMusicalEntities())
                {
                    db.Entry(genero).State = System.Data.EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(genero);
        }


        public ActionResult Delete(int id)
        {
            using (var db = new VentaMusicalEntities())
            {
                var generoBD = db.Generos.Find(id);

                if (generoBD == null)
                    return HttpNotFound();

                var vm = new VentaMusical.Models.ViewModels.Generos.Genero
                {
                    CodigoGenero = generoBD.CodigoGenero,
                    Descripcion = generoBD.Descripcion
                };

                return View(vm);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            using (var db = new VentaMusicalEntities())
            {
                var genero = db.Generos.Find(id);

                if (genero == null)
                    return HttpNotFound();

                db.Generos.Remove(genero);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
        }
    }
}
