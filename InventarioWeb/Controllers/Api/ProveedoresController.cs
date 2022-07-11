using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using InventarioWeb.Models;

namespace AppInventarioWeb.Controllers.Api
{
    public class ProveedoresController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Proveedores
        public IQueryable<Proveedor> GetProveedores()
        {
            return db.Proveedores;
        }

        // GET: api/Proveedores/5
        [ResponseType(typeof(Proveedor))]
        public IHttpActionResult GetProveedor(int id)
        {
            Proveedor proveedor = db.Proveedores.Find(id);
            if (proveedor == null)
            {
                return NotFound();
            }

            return Ok(proveedor);
        }

        // PUT: api/Proveedores/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProveedor(int id, Proveedor proveedor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != proveedor.NegocioId)
            {
                return BadRequest();
            }

            db.Entry(proveedor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProveedorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Proveedores
        [ResponseType(typeof(Proveedor))]
        public IHttpActionResult PostProveedor(Proveedor proveedor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Proveedores.Add(proveedor);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ProveedorExists(proveedor.NegocioId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = proveedor.NegocioId }, proveedor);
        }

        // DELETE: api/Proveedores/5
        [ResponseType(typeof(Proveedor))]
        public IHttpActionResult DeleteProveedor(int id)
        {
            Proveedor proveedor = db.Proveedores.Find(id);
            if (proveedor == null)
            {
                return NotFound();
            }

            db.Proveedores.Remove(proveedor);
            db.SaveChanges();

            return Ok(proveedor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProveedorExists(int id)
        {
            return db.Proveedores.Count(e => e.NegocioId == id) > 0;
        }
    }
}