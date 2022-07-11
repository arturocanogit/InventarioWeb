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
    public class AlmacenController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Almacen
        public IQueryable<Almacen> GetAlmacen()
        {
            return db.Almacen;
        }

        // GET: api/Almacen/5
        [ResponseType(typeof(Almacen))]
        public IHttpActionResult GetAlmacen(int id)
        {
            Almacen Almacen = db.Almacen.Find(id);
            if (Almacen == null)
            {
                return NotFound();
            }

            return Ok(Almacen);
        }

        // PUT: api/Almacen/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAlmacen(int id, Almacen Almacen)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Almacen.NegocioId)
            {
                return BadRequest();
            }

            db.Entry(Almacen).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlmacenExists(id))
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

        // POST: api/Almacen
        [ResponseType(typeof(Almacen))]
        public IHttpActionResult PostAlmacen(Almacen Almacen)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Almacen.Add(Almacen);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AlmacenExists(Almacen.NegocioId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = Almacen.NegocioId }, Almacen);
        }

        // DELETE: api/Almacen/5
        [ResponseType(typeof(Almacen))]
        public IHttpActionResult DeleteAlmacen(int id)
        {
            Almacen Almacen = db.Almacen.Find(id);
            if (Almacen == null)
            {
                return NotFound();
            }

            db.Almacen.Remove(Almacen);
            db.SaveChanges();

            return Ok(Almacen);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AlmacenExists(int id)
        {
            return db.Almacen.Count(e => e.NegocioId == id) > 0;
        }
    }
}