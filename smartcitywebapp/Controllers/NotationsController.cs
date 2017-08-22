using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SmartCityWebApp.Models;

namespace SmartCityWebApp.Controllers
{
    [Authorize]
    public class NotationsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Notations
        public IEnumerable<Notation> GetNotationDB()
        {
            return db.NotationDB
                .Include(h => h.Origin)
                .Include(h => h.Housing)
                .ToList();
        }

        // GET: api/Notations/5
        [ResponseType(typeof(Notation))]
        public async Task<IHttpActionResult> GetNotation(int id)
        {
            Notation notation = await db.NotationDB.FindAsync(id);
            if (notation == null)
            {
                return NotFound();
            }

            return Ok(notation);
        }
        
        // GET: api/Notations/Housing/housingID
        [ResponseType(typeof(List<Notation>))]
        [Route("api/Notations/Housing/{housingID}")]
        public async Task<IHttpActionResult> GetHousingNotations(int housingID)
        {
            IEnumerable<Notation> notations = db.NotationDB.Where(n=>n.Housing.ID == housingID)
                .Include(h => h.Origin)
                .Include(h => h.Housing)
                .ToList();

            if (notations == null)
            {
                return NotFound();
            }

            return Ok(notations);
        }

        // PUT: api/Notations/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutNotation(int id, Notation notation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != notation.ID)
            {
                return BadRequest();
            }

            db.Entry(notation).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotationExists(id))
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
        
        // POST: api/Notations
        [ResponseType(typeof(Notation))]
        public async Task<IHttpActionResult> PostNotation(NotationPost notationPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Notation notation = new Notation()
            {
                Comment = notationPost.Comment,
                Quotation = notationPost.Quotation,
                DateNotation = DateTime.Now
            };
            //notation.Origin = db.UserDB.First(u => u.ID == notationPost.OriginID);
            notation.Origin = db.UserDB.First(u => u.ID == User.Identity.Name);
            notation.Housing = db.HousingDB.First(h => h.ID == notationPost.HousingID);

            db.NotationDB.Add(notation);
            await db.SaveChangesAsync();

            return Created("api/Notations/" + notation.ID, notation);
            //return CreatedAtRoute("DefaultApi", new { id = notation.ID }, notation);
        }

        // DELETE: api/Notations/5
        [ResponseType(typeof(Notation))]
        public async Task<IHttpActionResult> DeleteNotation(int id)
        {
            Notation notation = await db.NotationDB.FindAsync(id);
            if (notation == null)
            {
                return NotFound();
            }

            db.NotationDB.Remove(notation);
            await db.SaveChangesAsync();

            return Ok(notation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NotationExists(int id)
        {
            return db.NotationDB.Count(e => e.ID == id) > 0;
        }
    }
}