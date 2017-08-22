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
    public class BedsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Beds
        public IQueryable<Bed> GetBeds()
        {
            return db.BedDB;
        }

        // GET: api/Beds/5
        [ResponseType(typeof(Bed))]
        public async Task<IHttpActionResult> GetBed(int id)
        {
            Bed bed = await db.BedDB.FindAsync(id);
            if (bed == null)
            {
                return NotFound();
            }

            return Ok(bed);
        }

        // PUT: api/Beds/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBed(int id, Bed bed)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bed.Code)
            {
                return BadRequest();
            }

            db.Entry(bed).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BedExists(id))
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

        // POST: api/Beds
        [ResponseType(typeof(Bed))]
        public async Task<IHttpActionResult> PostBed(Bed bed)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BedDB.Add(bed);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = bed.Code }, bed);
        }

        // DELETE: api/Beds/5
        [ResponseType(typeof(Bed))]
        public async Task<IHttpActionResult> DeleteBed(int id)
        {
            Bed bed = await db.BedDB.FindAsync(id);
            if (bed == null)
            {
                return NotFound();
            }

            db.BedDB.Remove(bed);
            await db.SaveChangesAsync();

            return Ok(bed);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BedExists(int id)
        {
            return db.BedDB.Count(e => e.Code == id) > 0;
        }
    }
}