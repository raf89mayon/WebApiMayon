using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SmartCityWebApp;
using SmartCityWebApp.Models;

namespace SmartCityWebApp.Controllers
{
    [Authorize]
    public class LocalitiesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Localities
        public IQueryable<Locality> GetLocalities()
        {
            return db.LocalityDB;
        }

        // GET: api/Localities/5
        [ResponseType(typeof(Locality))]
        public async Task<IHttpActionResult> GetLocality(string id)
        {
            Locality locality = await db.LocalityDB.FindAsync(id);
            if (locality == null)
            {
                return NotFound();
            }

            return Ok(locality);
        }

        // PUT: api/Localities/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLocality(string id, Locality locality)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != locality.Name)
            {
                return BadRequest();
            }

            db.Entry(locality).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocalityExists(id))
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

        // POST: api/Localities
        [ResponseType(typeof(Locality))]
        public async Task<IHttpActionResult> PostLocality(Locality locality)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LocalityDB.Add(locality);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LocalityExists(locality.Name))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = locality.Name }, locality);
        }

        // DELETE: api/Localities/5
        [ResponseType(typeof(Locality))]
        public async Task<IHttpActionResult> DeleteLocality(string id)
        {
            Locality locality = await db.LocalityDB.FindAsync(id);
            if (locality == null)
            {
                return NotFound();
            }

            db.LocalityDB.Remove(locality);
            await db.SaveChangesAsync();

            return Ok(locality);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LocalityExists(string id)
        {
            return db.LocalityDB.Count(e => e.Name == id) > 0;
        }
    }
}