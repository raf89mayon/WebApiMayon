using System;
using System.Collections.Generic;
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
    public class UsersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<User> GetUserDB()
        {
            return db.UserDB.Include(u=>u.Role).ToList();
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        [Route("api/Users/{id}")]
        public async Task<IHttpActionResult> GetUser(string id)
        {
            User user = await db.UserDB.Include(u => u.Role).SingleOrDefaultAsync(u => u.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        [Route("api/Users/Login/{id}")]
        public async Task<IHttpActionResult> GetLogedUser(string id)
        {
            User user = await db.UserDB.Include(u => u.Role).SingleOrDefaultAsync(u => u.ID == id);

            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        [Route("api/Users/Login/{id}/{password}")]
        public async Task<IHttpActionResult> GetUserByLoginPassword(string id, string password)
        {
            User user = await db.UserDB.Include(u => u.Role).SingleOrDefaultAsync(u => u.ID == id);

            if (user != null && user.PassWord.Equals(password))
            {
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
        }
        
        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        [Route("api/Users/{id}")]
        public async Task<IHttpActionResult> PutUser(string id, UserPut userPut)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User user = await db.UserDB.Include(u => u.Role).SingleOrDefaultAsync(u => u.ID == id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                user.PassWord = userPut.PassWord;
                user.FirstName = userPut.FirstName;
                user.LastName = userPut.LastName;
                user.Number = userPut.Number;
                user.PostBox = userPut.PostBox;
                user.ZipCode = userPut.ZipCode;
                user.Street = userPut.Street;
                user.City = userPut.City;
                user.Country = userPut.Country;
                user.PhoneNumber = userPut.PhoneNumber;
                user.BirthDate = userPut.BirthDate;
                user.Picture = userPut.Picture;
            }            

            try
            {
                using (var dbTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Entry(user).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        dbTransaction.Commit();
                    }
                    catch (DbUpdateException ex)
                    {
                        dbTransaction.Rollback();
                    }
                }
                
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser(UserPost userPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User user = new User()
            {
                ID = userPost.ID,
                PassWord = userPost.PassWord,
                FirstName = userPost.FirstName,
                LastName = userPost.LastName,
                Number = userPost.Number,
                PostBox = userPost.PostBox,
                ZipCode = userPost.ZipCode,
                Street = userPost.Street,
                City = userPost.City,
                Country = userPost.Country,
                PhoneNumber = userPost.PhoneNumber,
                BirthDate = userPost.BirthDate,
                EmailAddress = userPost.EmailAddress,
                Picture = userPost.Picture,
                RegistrationDate = DateTime.Now,
                LastSignInDate = DateTime.Now
            };
            user.Role = db.RoleDB.First(r => r.ID == userPost.RoleID);
            db.UserDB.Add(user);
            // modif
            await db.SaveChangesAsync();

            return Created("api/Users", user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        [Route("api/Users/{id}")]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {
            User user = await db.UserDB.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            IEnumerable<Housing> houses = db.HousingDB.Where(house => house.Host != null && house.Host.ID == id);

            if (houses != null)
            {
                foreach (var house in houses)
                {
                    db.HousingDB.Remove(house);
                }
            }

            db.UserDB.Remove(user);
            await db.SaveChangesAsync();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(string id)
        {
            return db.UserDB.Count(e => e.ID == id) > 0;
        }
    }
}