using Locs4Youth.Models;
using Locs4Youth.Utils;
using System;
using System.Web.Mvc;
using Useraccount = Locs4Youth.Models.User;

namespace Locs4Youth.Controllers
{
    /// <summary>
    /// This controller handles everything related to the Administration Portal
    /// </summary>
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        #region GET
        // GET: /Admin/ApproveLocation/{id}
        public ActionResult ApproveLocation(int? id)
        {
            try
            {
                // Check if there is an ID
                if (!id.HasValue) return RedirectToAction("Locations");

                // Approve location
                Location.Approve(id.Value);

                return RedirectToAction("Locations");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: /Admin/ApproveRating/{id}
        public ActionResult ApproveRating(int? id)
        {
            try
            {
                // Check if there is an ID
                if (!id.HasValue) return RedirectToAction("Ratings");

                // Approve location
                Rating.Approve(id.Value);

                return RedirectToAction("Ratings");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: /Admin/Index
        public ActionResult Index()
        {
            try
            {
                ViewBag.Ratings = Rating.GetUnconfirmedRatingsAmount();
                ViewBag.Locations = Location.GetUnconfirmedAmount();
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: /Admin/Locations
        public ActionResult Locations()
        {
            try
            {
                return View(Location.GetUnconfirmedLocations());
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: /Admin/Promote/{id}
        public ActionResult Promote(int? id)
        {
            try
            {
                // Check if there is an ID
                if (!id.HasValue) return RedirectToAction("Users");

                // Promote user
                Useraccount.Promote(id.Value);

                return RedirectToAction("Users");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: /Admin/Ratings
        public ActionResult Ratings()
        {
            try
            {
                return View(Rating.GetUnconfirmedRatings());
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: /Admin/RejectLocation/{id}
        public ActionResult RejectLocation(int? id)
        {
            try
            {
                // Check if there is an ID
                if (!id.HasValue) return RedirectToAction("Locations");

                // First delete all features that belong to the location
                Location_has_Feature.DeleteFeaturesForLocation(id.Value);

                // Reject location
                Location.Delete(id.Value);

                // Remove picture from hdd
                ImageHelper.DeleteImage(id.Value, Server.MapPath("~/Content/img/locations/"));

                return RedirectToAction("Locations");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: /Admin/RejectLocation/{id}
        public ActionResult RejectRating(int? id)
        {
            try
            {
                // Check if there is an ID
                if (!id.HasValue) return RedirectToAction("Ratings");

                // Reject location
                Rating.Delete(id.Value);

                return RedirectToAction("Ratings");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: /Admin/Users
        public ActionResult Users()
        {
            try
            {
                return View(Useraccount.GetAllUsers());
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }
        #endregion
    }
}