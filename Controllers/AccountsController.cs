using ComputerGamesLibrary.Models;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace ComputerGamesLibrary.Controllers
{
    public class AccountsController : Controller
    {
        public ActionResult Login()
        {
            return View("Login");
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
                        return View("Login");
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
            return View("Register");
        }

        [HttpPost]
        public ActionResult Register(UserModel userModel)
        {
            if (IsUsernameTaken(userModel.Username))
            {
                ModelState.AddModelError("", "Username is taken");
                return View("Register");
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
                return View("Register", userModel);
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            
            return RedirectToAction("Login");
        }

        private bool ValidateInputs(UserModel userModel)
        {
            bool usernameInputIsValid = !(string.IsNullOrEmpty(userModel.Username)) 
                && userModel.Username.Length > Constants.MIN_STRING_LENGTH
                && userModel.Username.Length < Constants.MAX_NAME_LENGTH;

            bool passwordInputIsValid = !(string.IsNullOrEmpty(userModel.Password))
                && userModel.Password.Length > Constants.MIN_PASSWORD_LENGTH
                && userModel.Password.Length < Constants.MAX_PASSWORD_LENGTH;

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