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
        public ActionResult Index(string searchString,
            string gameGenre,
            int? selectedYear,
            decimal? fromPrice,
            decimal? toPrice)
        {
            // Upon login set the userId for use throughout
            int currentUserId = (int)Session["CurrentUserId"];

            // Ensure we only get games for the logged in user
            var userComputerGames = db.UserComputerGames
                .Where(game => game.User.ID == currentUserId);

            UserComputerGamesViewModel viewModel = BuildViewModel(userComputerGames,
                searchString,
                gameGenre,
                selectedYear,
                fromPrice,
                toPrice);

            return View("Index", viewModel);
        }

        // GET: UserComputerGames/Add
        public ActionResult Add()
        {
            return View("Add");
        }

        // POST: UserComputerGames/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "Title,Genre,YearPublished,Price,UserId")] UserComputerGame userComputerGame)
        {
            if (ModelState.IsValid && ValidateUserComputerGame(userComputerGame))
            {
                userComputerGame.UserId = (int)Session["CurrentUserId"];
                db.UserComputerGames.Add(userComputerGame);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Add", userComputerGame);
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
           
            return View("Update", userComputerGame);
        }

        // POST: UserComputerGames/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include = "ID,Title,Genre,YearPublished,Price,UserId")] UserComputerGame userComputerGame)
        {
            if (ModelState.IsValid && ValidateUserComputerGame(userComputerGame))
            {
                db.Entry(userComputerGame).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            
            return View("Update", userComputerGame);
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

            return View("Delete", userComputerGame);
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
            else if (ModelState.IsValid)
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

        private bool ValidateUserComputerGame(UserComputerGame game)
        {
            bool isTitleValid = game.Title.Length > Constants.MIN_STRING_LENGTH 
                && game.Title.Length < Constants.MAX_NAME_LENGTH;
            bool isGenreValid = game.Genre.Length > Constants.MIN_STRING_LENGTH 
                && game.Genre.Length < Constants.MAX_GENRE_LENGTH;
            bool isYearValid = game.YearPublished > 0 
                && game.YearPublished < Constants.UPPER_YEAR_LIMIT;
            bool isPriceValid = game.Price > 0 
                && game.Price < decimal.MaxValue;

            return isTitleValid && isGenreValid && isYearValid && isPriceValid;
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
