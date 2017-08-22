using System.Linq;
using System.Web.Http;
using SmartCityWebApp.Models;
using System.Security.Claims;

namespace SmartCityWebApp.Controllers
{
    public class SuperController : ApiController
    {
        public User GetUserIdentity ()
        {
            var userID = User.Identity as ClaimsIdentity;
            Claim IdentityClaim = userID.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            User user = new ApplicationDbContext().UserDB.FirstOrDefault(u => u.ID == IdentityClaim.Value);

            return user;
        }
    }
}
