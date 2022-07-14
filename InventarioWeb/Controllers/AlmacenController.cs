using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using InventarioWeb.Models;

namespace AppInventarioWeb.Controllers
{
    public class AlmacenController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

        // GET: Proveedores
        public ActionResult Index()
        {
            int negocioId = int.Parse(identity.Claims.Where(c => c.Type == ClaimTypes.System).Select(c => c.Value).Single());
            return View(db.Almacenes.Where(x => x.NegocioId == negocioId).ToList());
        }

        // GET: Proveedores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedor proveedor = db.Proveedores.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }
            return View(proveedor);
        }

        // GET: Proveedores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proveedores/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Almacen almacen)
        {
            if (ModelState.IsValid)
            {
                int negocioId = (int)Session["NegocioId"];
                int almacenId = (db.Almacenes
                    .Where(x => x.NegocioId == 1).Max(x => (int?)x.AlmacenId) ?? 0) + 1;

                almacen.NegocioId = (int)Session["NegocioId"];
                almacen.AlmacenId = almacenId;
                almacen.FechaAlta = DateTime.Now;
                almacen.Activo = true;

                db.Almacenes.Add(almacen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(almacen);
        }

        // GET: Proveedores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedor proveedor = db.Proveedores.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }
            return View(proveedor);
        }

        // POST: Proveedores/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProveedorId,Nombre")] Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proveedor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(proveedor);
        }

        // GET: Proveedores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedor proveedor = db.Proveedores.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }
            return View(proveedor);
        }

        // POST: Proveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Proveedor proveedor = db.Proveedores.Find(id);
            db.Proveedores.Remove(proveedor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
