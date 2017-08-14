using Locs4Youth.Models;
using Locs4Youth.Utils;
using System;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Useraccount = Locs4Youth.Models.User;

namespace Locs4Youth.Controllers
{
    /// <summary>
    /// This controller handles everything related to a user
    /// </summary>
    public class AccountController : Controller
    {
        #region CONSTANTS
        private const string USER_KEY = "User";
        #endregion

        #region GET
        // GET: /Account/Activate
        [Authorize]
        public ActionResult Avatar()
        {
            return View();
        }

        // GET: /Account/Activate
        [AllowAnonymous]
        public ActionResult Activate(Guid? code, int? userId)
        {
            try
            {
                // Do not load page if user is already logged in, has no use
                if (User.Identity.IsAuthenticated) return RedirectToAction("NotFound", "Error");

                // Check if the GET parameters are both included
                if (!code.HasValue || !userId.HasValue) return RedirectToAction("NotFound", "Error");

                // Code exists and is legitimate
                if (Activation.HasActivation(code.Value, userId.Value))
                {
                    Useraccount.Activate(userId.Value);
                    Message.Flash(TempData, "Account is succesvol geactiveerd. U kunt nu inloggen.", MessageType.Success);
                }
                else
                {
                    Message.Flash(TempData, "Ongeldige actie.", MessageType.Error);
                }
                return RedirectToAction("SignIn");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: /Account/Index
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                ViewBag.PendingLocations = Location.GetUnconfirmedAmountForUser(User.Identity.Name);
                ViewBag.Locations = Location.GetConfirmedAmountForUser(User.Identity.Name);
                ViewBag.PendingRatings = Rating.GetUnconfirmedRatingsAmountForUser(User.Identity.Name);
                ViewBag.Ratings = Rating.GetConfirmedRatingsAmountForUser(User.Identity.Name);
                ViewBag.Requests = Rental.GetPendingRentalsAmountForUser(User.Identity.Name);
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: /Account/Locations
        [Authorize]
        public ActionResult Locations()
        {
            try
            {
                ViewBag.Locations = Location.GetAllLocationsForUser(User.Identity.Name);
                ViewBag.Rentals = Rental.GetAllRentalsForUser(User.Identity.Name);
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: /Account/Password
        [Authorize]
        public ActionResult Password()
        {
            return View();
        }

        // GET: /Account/Ratings
        [Authorize]
        public ActionResult Ratings()
        {
            try
            {
                return View(Rating.GetRatingsForUser(User.Identity.Name));
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: /Account/SignIn
        [AllowAnonymous]
        public ActionResult SignIn(string returnUrl)
        {
            // Do not load page if user is already logged in, has no use
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // GET: /Account/SignOn
        [AllowAnonymous]
        public ActionResult SignOn()
        {
            return Redirect("~/Register/Register.aspx");
        }

        // GET: /Account/SignOut
        [Authorize]
        public ActionResult SignOut()
        {
            // End session
            Session.Clear();
            FormsAuthentication.SignOut();
            Message.Flash(TempData, "U bent succesvol uitgelogd.", MessageType.Success);
            return RedirectToAction("SignIn");
        }
        #endregion

        #region POST
        // POST: /Account/Avatar
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Avatar(AvatarModel model)
        {
            try
            {
                // Make sure there are no model errors
                if (!ModelState.IsValid) return View(model);

                // Fetch user
                User usr = Useraccount.GetUserByEmail(User.Identity.Name);

                // Remove previous avatar if it exists
                string path = Server.MapPath("~/Content/img/avatars/");
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);

                // Resize avatar to 128x128 and save
                ImageHelper.SaveImage(ImageHelper.ResizeImage(Image.FromStream(model.Image.InputStream, true, true), 128, 128), usr.Id, path);

                // Flash message
                Message.Flash(TempData, "Avatar is succesvol gewijzigd.", MessageType.Success);
                return RedirectToAction("Avatar");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // POST: /Account/Password
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Password(PasswordModel model)
        {
            try
            {
                // Make sure there are no model errors
                if (!ModelState.IsValid) return View(model);

                // Check if old password matches current password
                User usr = Useraccount.GetUserByEmail(User.Identity.Name);
                if (!PasswordStorage.VerifyPassword(model.OldPassword, usr.Password))
                {
                    ModelState.AddModelError(string.Empty, "Huidig wachtwoord is onjuist.");
                    return View(model);
                }

                // Check if new password and confirmation match
                if (model.NewPassword != model.ConfirmPassword)
                {
                    ModelState.AddModelError(string.Empty, "Het nieuwe wachtwoord komt niet overeen met de bevestiging.");
                    return View(model);
                }

                // Check if the user actually changed his/her password
                if (PasswordStorage.VerifyPassword(model.NewPassword, usr.Password))
                {
                    ModelState.AddModelError(string.Empty, "Het huidig wachtwoord opgeven als uw nieuw wachtwoord heeft geen nut...");
                    return View(model);
                }

                // Everything is OK, change password
                Useraccount.ChangePassword(User.Identity.Name, PasswordStorage.CreateHash(model.NewPassword));

                // Flash message
                Message.Flash(TempData, "Wachtwoord is succesvol gewijzigd.", MessageType.Success);

                // Redirect
                return RedirectToAction("Password");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // POST: /Account/SignIn
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(SignInModel model, string returnUrl)
        {
            try
            {
                // Do not load page if user is already logged in, has no use
                if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");

                // Make sure there are no model errors
                if (!ModelState.IsValid) return View(model);

                // Check if user exists
                User usr = Useraccount.GetUserByEmail(model.Email);
                if (usr == null)
                {
                    ModelState.AddModelError(string.Empty, "De gebruikersnaam en/of wachtwoord is onjuist.");
                    return View(model);
                }

                // Check if passwords match
                if (!PasswordStorage.VerifyPassword(model.Password, usr.Password))
                {
                    ModelState.AddModelError(string.Empty, "De gebruikersnaam en/of wachtwoord is onjuist.");
                    return View(model);
                }

                // Check if user has activated his/her account
                if (!usr.Activated)
                {
                    ModelState.AddModelError(string.Empty, "Uw account is nog niet geactiveerd. Controleer uw mailbox.");
                    return View(model);
                }

                // Everything OK, establish session
                FormsAuthentication.SetAuthCookie(usr.Email, model.RememberMe);
                Session[USER_KEY] = Useraccount.GetUserProfile(usr.Email);

                // Redirect
                if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");
                else return Redirect(Server.UrlDecode(returnUrl));
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }
        #endregion
    }
}
