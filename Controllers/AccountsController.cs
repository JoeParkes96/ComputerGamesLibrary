using ComputerGamesLibrary.Models;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace ComputerGamesLibrary.Controllers
{
    public class AccountsController : Controller
    {
        // GET: Accounts
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserModel userModel)
        {
            if (ValidateInputs(userModel))
            {
                using (ComputerGamesLibraryContext context = new ComputerGamesLibraryContext())
                {
                    User foundUser = context.Users
                        .Where(user => user.Username == userModel.Username)
                        .SingleOrDefault();

                    bool isUserValid = foundUser != null
                        && Crypto.VerifyHashedPassword(foundUser.HashedPassword, userModel.Password);

                    if (isUserValid)
                    {
                        FormsAuthentication.SetAuthCookie(foundUser.ID.ToString(), false);
                        Session["CurrentUserId"] = foundUser.ID;
                        return RedirectToAction("Index", "UserComputerGames");
                    }
                    else
                    {
                        ModelState.AddModelError("", "User credentials are incorrect");
                        return View();
                    }
                }
            }
            else
            {
                return View(userModel);
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserModel userModel)
        {
            if (IsUsernameTaken(userModel.Username))
            {
                ModelState.AddModelError("", "Username is taken");
                return View();
            }

            if (ValidateInputs(userModel))
            {
                // Password is hashed for extra security
                string hashedPassword = Crypto.HashPassword(userModel.Password);
                User user = new User
                {
                    Username = userModel.Username,
                    HashedPassword = hashedPassword
                };

                using (ComputerGamesLibraryContext context = new ComputerGamesLibraryContext())
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                }

                return RedirectToAction("Login", "Accounts");
            }
            else
            {
                return View(userModel);
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        private bool ValidateInputs(UserModel userModel)
        {
            bool usernameInputIsValid = !(string.IsNullOrEmpty(userModel.Username));
            bool passwordInputIsValid = !(string.IsNullOrEmpty(userModel.Password));

            return usernameInputIsValid && passwordInputIsValid;
        }

        private bool IsUsernameTaken(string username)
        {
            bool isUsernameTaken;
            using (ComputerGamesLibraryContext context = new ComputerGamesLibraryContext())
            {
                isUsernameTaken = context.Users.Any(user => user.Username == username);
            }

            return isUsernameTaken;
        }
    }
}