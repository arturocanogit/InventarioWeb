using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventarioWeb.Models;
using InventarioWeb.Models.Dtos;
using Global;
using System.Security.Claims;
using System.Threading;

namespace AppInventarioWeb.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
        public int NegocioId
        {
            get
            {
                return int.Parse(identity.Claims.Where(c => c.Type == ClaimTypes.System).Select(c => c.Value).Single());
            }
        }

        // GET: Productos
        public ActionResult Index()
        {
            IEnumerable<ProductoDto> productos = db.Productos
                .Where(x => x.NegocioId == NegocioId)
                .Include(x => x.Proveedor)
                .Select(x => new ProductoDto
                {
                    ProveedorId = x.ProveedorId,
                    ProductoId = x.ProductoId,
                    Nombre = x.Nombre,
                    Costo = x.Costo,
                    Precio = x.Precio,
                    Contenido = x.Contenido,
                    Unidad = x.Unidad,
                    ProveedorNombre = x.Proveedor.Nombre
                });

            ViewBag.ProveedorId = new SelectList(db.Proveedores
                .Where(x => x.NegocioId == NegocioId), "ProveedorId", "Nombre");

            ViewBag.AlmacenId = new SelectList(db.Almacenes
                .Where(x => x.NegocioId == NegocioId), "AlmacenId", "Nombre");

            return View(productos);
        }

        // GET: Productos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // GET: Productos/Create
        public ActionResult Create()
        {
            ViewBag.NegocioId = new SelectList(db.Proveedores, "NegocioId", "Nombre");
            return View();
        }

        // POST: Productos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Producto producto)
        {
            producto.NegocioId = NegocioId;

            if (ModelState.IsValid)
            {
                int productoId = db.Productos
                    .Where(x => x.NegocioId == NegocioId)
                    .Max(x => (int?)x.ProductoId) ?? 0;

                producto.ProductoId = productoId + 1;

                db.Productos.Add(producto);
                db.SaveChanges();

                var inventario = new Inventario
                {
                    NegocioId = NegocioId,
                    ProductoId = producto.ProductoId,
                    Cantidad = 5
                };

                int inventarioId = db.Inventarios
                   .Where(x => x.NegocioId == NegocioId)
                   .Max(x => (int?)x.InventarioId) ?? 0;

                inventario.InventarioId = inventarioId + 1;
                db.Inventarios.Add(inventario);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.NegocioId = new SelectList(db.Proveedores, "NegocioId", "Nombre", producto.NegocioId);
            return View(producto);
        }

        // GET: Productos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.NegocioId = new SelectList(db.Proveedores, "NegocioId", "Nombre", producto.NegocioId);
            return View(producto);
        }

        // POST: Productos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NegocioId,ProveedorId,ProductoId,Nombre,Costo,Precio,UnidadNumero,UnidadLetra")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NegocioId = new SelectList(db.Proveedores, "NegocioId", "Nombre", producto.NegocioId);
            return View(producto);
        }

        // GET: Productos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Productos.Find(id);
            db.Productos.Remove(producto);
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
