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
using System.Security.Claims;
using SmartCityWebApp.Models;

namespace SmartCityWebApp.Controllers
{
    [Authorize]
    public class HousingsController : ApiController
    {
        //private SmartContext db = new SmartContext();
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Housings
        public IEnumerable<Housing> GetHousingDB()
        {
            return db.HousingDB
                .Include(h => h.Host)
                .Include(h => h.Host.Role)
                .ToList();
        }

        // GET: api/Housings/5
        [ResponseType(typeof(Housing))]
        public async Task<IHttpActionResult> GetHousing(int id)
        {
            Housing housing = await db.HousingDB
                .Include(h => h.Host)
                .Include(h => h.Host.Role)
                .SingleOrDefaultAsync(h => h.ID == id);

            if (housing == null)
            {
                return NotFound();
            }

            return Ok(housing);
        }

        // PUT: api/Housings/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutHousing(int id, HousingPost housingPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userID = User.Identity as ClaimsIdentity;
            Claim IdentityClaim = userID.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            User user = db.UserDB.FirstOrDefault(u => u.ID == IdentityClaim.Value);
            
            Housing updatedHousing = await db.HousingDB.FindAsync(id);
            
            updatedHousing.Host = db.UserDB.First(u => u.ID == User.Identity.Name);

            updatedHousing.EditDate = DateTime.Now;
            updatedHousing.PostBox = housingPost.PostBox;
            updatedHousing.ZipCode = housingPost.ZipCode;
            updatedHousing.BedType = housingPost.BedType;
            updatedHousing.StartDate = housingPost.StartDate;
            updatedHousing.EndDate = housingPost.EndDate;
            updatedHousing.Number = housingPost.Number;
            updatedHousing.Street = housingPost.Street;
            updatedHousing.City = housingPost.City;
            updatedHousing.SpaceLocalization = housingPost.SpaceLocalization;
            updatedHousing.Description = housingPost.Description;
            updatedHousing.Pictures = housingPost.Pictures;
            updatedHousing.Wifi = housingPost.Wifi;
            updatedHousing.Kitchen = housingPost.Kitchen;
            updatedHousing.Office = housingPost.Office;
            updatedHousing.Toilet = housingPost.Toilet;
            updatedHousing.Shower = housingPost.Shower;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HousingExists(id))
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


        // GET: api/Housings/User/userID
        [ResponseType(typeof(List<Housing>))]
        [Route("api/Housings/User/{userID}")]
        public async Task<IHttpActionResult> GetUserHousings(String userID)
        {
            IEnumerable<Housing> housings = db.HousingDB.Where(h => h.Host.ID == userID)
                .Include(h => h.Host)
                .Include(h => h.Host.Role)
                .ToList();

            if (housings == null)
            {
                return NotFound();
            }

            return Ok(housings);
        }

        // POST: api/Housings
        [ResponseType(typeof(Housing))]
        public async Task<IHttpActionResult> PostHousing(HousingPost housingPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Housing housing = new Housing()
            {
                PostBox = housingPost.PostBox,
                ZipCode = housingPost.ZipCode,
                BedType = housingPost.BedType,
                StartDate = housingPost.StartDate,
                EndDate = housingPost.EndDate,
                AddDate = DateTime.Now,
                EditDate = DateTime.Now,
                Number = housingPost.Number,
                Street = housingPost.Street,
                City = housingPost.City,
                SpaceLocalization = housingPost.SpaceLocalization,
                Description = housingPost.Description,
                Pictures = housingPost.Pictures,
                Wifi = housingPost.Wifi,
                Kitchen = housingPost.Kitchen,
                Office = housingPost.Office,
                Toilet = housingPost.Toilet,
                Shower = housingPost.Shower
            };
            housing.Host = db.UserDB.First(u => u.ID == User.Identity.Name);
            db.HousingDB.Add(housing);
            await db.SaveChangesAsync();

            return Created("api/Housings/", housing);
        }


        // DELETE: api/Housings/5
        [ResponseType(typeof(Housing))]
        public async Task<IHttpActionResult> DeleteHousing(int id)
        {
            Housing housing = await db.HousingDB.FindAsync(id);
            if (housing == null)
            {
                return NotFound();
            }
            /*
            IEnumerable<Notation> notations = db.NotationDB.Where(note => note.Housing.ID == id);

            if (notations != null)
            {
                foreach (var note in notations)
                {
                    db.NotationDB.Remove(note);
                }
            }

            IEnumerable<Message> messages = db.MessageDB.Where(mess => mess.Housing != null && mess.Housing.ID == id);

            if (messages != null)
            {
                foreach (var mess in messages)
                {
                    db.MessageDB.Remove(mess);
                }
            }
            */
            db.HousingDB.Remove(housing);
            await db.SaveChangesAsync();

            return Ok(housing);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HousingExists(int id)
        {
            return db.HousingDB.Count(e => e.ID == id) > 0;
        }
    }
}