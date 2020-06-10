using System.Collections.Generic;
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
        public ActionResult Index(string searchString, string gameGenre, int? selectedYear, decimal? fromPrice, decimal? toPrice)
        {
            // Upon login set the userId for use throughout
            int currentUserId = (int)Session["CurrentUserId"];

            // Ensure we only get games for the logged in user
            var userComputerGames = db.UserComputerGames
                .Where(game => game.User.ID == currentUserId);

            UserComputerGamesViewModel viewModel = BuildViewModel(userComputerGames, searchString, gameGenre, selectedYear, fromPrice, toPrice);

            return View(viewModel);
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

        private UserComputerGamesViewModel BuildViewModel(IQueryable<UserComputerGame> userComputerGames, string searchString, string gameGenre, int? selectedYear, decimal? fromPrice, decimal? toPrice)
        {
            List<string> distinctGenres = userComputerGames
                .OrderBy(game => game.Genre)
                .Select(game => game.Genre)
                .Distinct()
                .ToList();

            List<int> distinctYears = userComputerGames
                .OrderBy(game => game.YearPublished)
                .Select(game => game.YearPublished)
                .Distinct()
                .ToList();

            if (fromPrice > toPrice)
            {
                ModelState.AddModelError("", "From price cannot be greater than To price");
            }
            else
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    userComputerGames = userComputerGames.Where(game => game.Title.Contains(searchString));
                }

                if (!string.IsNullOrEmpty(gameGenre))
                {
                    userComputerGames = userComputerGames.Where(game => game.Genre == gameGenre);
                }

                if (selectedYear != null)
                {
                    userComputerGames = userComputerGames.Where(game => game.YearPublished == selectedYear);
                }

                if (fromPrice != null && toPrice != null)
                {
                    userComputerGames = userComputerGames.Where(game => game.Price >= fromPrice)
                        .Where(game => game.Price <= toPrice);
                }
            }

            UserComputerGamesViewModel viewModel = new UserComputerGamesViewModel
            {
                Genres = new SelectList(distinctGenres),
                Years = new SelectList(distinctYears),
                Games = userComputerGames.ToList()
            };

            return viewModel;
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
