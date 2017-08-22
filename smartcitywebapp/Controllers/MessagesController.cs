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
    public class MessagesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Messages
        [HttpGet]
        public IEnumerable<Message> Get()
        {
            return db.MessageDB
                .Include(m => m.Reciever)
                .Include(m => m.Sender)
                .Include(m => m.Housing)
                .Include(m => m.Housing.Host)
                .ToList();
        }

        // GET: api/Messages/5
        [ResponseType(typeof(Message))]
        public async Task<IHttpActionResult> GetMessage(int id)
        {
            Message message = await db.MessageDB.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }

        // GET: api/Messages/User/userID
        [ResponseType(typeof(List<Message>))]
        [Route("api/Messages/User/{userID}")]
        public async Task<IHttpActionResult> GetMessagesFromUser(String userID)
        {
            IEnumerable<Message> messages = db.MessageDB.Where(m=>m.Sender.ID==userID || m.Reciever.ID == userID)
                .Include(m => m.Reciever)
                .Include(m => m.Sender)
                .Include(m => m.Housing)
                .Include(m => m.Housing.Host)
                .ToList();

            if (messages == null)
            {
                return NotFound();
            }

            return Ok(messages);
        }

        // GET: api/Messages/Conversation/userB_ID/housingID
        [ResponseType(typeof(List<Message>))]
        [Route("api/Messages/Conversation/{userB_ID}/{housingID}")]
        public async Task<IHttpActionResult> GetConversation(String userB_ID, int housingID)
        {
            User userA = db.UserDB.First(u => u.ID == User.Identity.Name);
            String userA_ID = userA.ID;
            IEnumerable<Message> messages = new List<Message>();

            if (housingID != 0)
            {
                messages = db.MessageDB.Where(m => m.Housing.ID == housingID && 
                                                ((m.Sender.ID == userA_ID && m.Reciever.ID == userB_ID) || 
                                                (m.Sender.ID == userB_ID && m.Reciever.ID == userA_ID)))
                                                                .Include(m => m.Reciever)
                                                                .Include(m => m.Sender)
                                                                .Include(m => m.Housing)
                                                                .Include(m => m.Housing.Host)
                                                                .ToList();
            }
            else
            {
                messages = db.MessageDB.Where(m => m.Housing == null &&
                                                ((m.Sender.ID == userA_ID && m.Reciever.ID == userB_ID) ||
                                                (m.Sender.ID == userB_ID && m.Reciever.ID == userA_ID)))
                                                                .Include(m => m.Reciever)
                                                                .Include(m => m.Sender)
                                                                .Include(m => m.Housing)
                                                                .Include(m => m.Housing.Host)
                                                                .ToList();
            }

            if (messages == null)
            {
                return NotFound();
            }

            return Ok(messages);
        }

        // PUT: api/Messages/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMessage(int id, Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != message.ID)
            {
                return BadRequest();
            }

            db.Entry(message).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(id))
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

        // POST: api/Messages
        [HttpPost]
        [ResponseType(typeof(Message))]
        //[Route("api/Messages")]
        public async Task<IHttpActionResult> PostMessage(MessagePost messagePost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Message message = new Message()
            {
                Sender = db.UserDB.First(u => u.ID == User.Identity.Name),
                //Sender = db.UserDB.First(u => u.ID == messagePost.SenderID),
                Reciever = db.UserDB.First(u => u.ID == messagePost.RecieverID),
                SendDate = DateTime.Now,
                Content = messagePost.Content
            };

            if (messagePost.HousingID != 0)
                message.Housing = db.HousingDB.First(h => h.ID == messagePost.HousingID);
            else
                message.Housing = null;

            db.MessageDB.Add(message);
            await db.SaveChangesAsync();

            return Created("api/Messages", message);
        }

        // DELETE: api/Messages/5
        [ResponseType(typeof(Message))]
        public async Task<IHttpActionResult> DeleteMessage(int id)
        {
            Message message = await db.MessageDB.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            db.MessageDB.Remove(message);
            await db.SaveChangesAsync();

            return Ok(message);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MessageExists(int id)
        {
            return db.MessageDB.Count(e => e.ID == id) > 0;
        }
    }
}