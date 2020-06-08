using System.Linq;
using System.Web.Mvc;
using ComputerGamesLibrary.Models;

namespace ComputerGamesLibrary.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private ComputerGamesLibraryContext db = new ComputerGamesLibraryContext();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }
    }
}
