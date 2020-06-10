using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ComputerGamesLibrary.Models;

namespace ComputerGamesLibrary.Controllers
{
    [Authorize]
    public class UserComputerGamesController : Controller
    {
        private ComputerGamesLibraryContext db = new ComputerGamesLibraryContext();

        // GET: UserComputerGames
        public ActionResult Index()
        {
            // Upon login set the userId for use throughout
            int currentUserId = (int)Session["CurrentUserId"];

            // Ensure we only get games for the logged in user
            var userComputerGames = db.UserComputerGames
                .Where(game => game.User.ID == currentUserId);

            return View(userComputerGames.ToList());
        }

        // GET: UserComputerGames/Add
        public ActionResult Add()
        {
            return View();
        }

        // POST: UserComputerGames/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "Title,Genre,YearPublished,Price,UserId")] UserComputerGame userComputerGame)
        {
            if (ModelState.IsValid)
            {
                userComputerGame.UserId = (int)Session["CurrentUserId"];
                db.UserComputerGames.Add(userComputerGame);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userComputerGame);
        }

        // GET: UserComputerGames/Update/5
        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserComputerGame userComputerGame = db.UserComputerGames.Find(id);
            if (userComputerGame == null)
            {
                return HttpNotFound();
            }
           
            return View(userComputerGame);
        }

        // POST: UserComputerGames/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include = "ID,Title,Genre,YearPublished,Price,UserId")] UserComputerGame userComputerGame)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userComputerGame).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(userComputerGame);
        }

        // GET: UserComputerGames/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserComputerGame userComputerGame = db.UserComputerGames.Find(id);
            if (userComputerGame == null)
            {
                return HttpNotFound();
            }
            return View(userComputerGame);
        }

        // POST: UserComputerGames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserComputerGame userComputerGame = db.UserComputerGames.Find(id);
            db.UserComputerGames.Remove(userComputerGame);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
