using ActionMailerNext.Mvc5_2;
using Locs4Youth.Models;
using Locs4Youth.Utils;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Useraccount = Locs4Youth.Models.User;

namespace Locs4Youth.Controllers
{
    /// <summary>
    /// This controller handles everything related to renting locations
    /// </summary>
    [Authorize]
    public class RentalController : Controller
    {
        #region GET
        // GET: /Rental/ApproveRequest/{id}
        public ActionResult ApproveRequest(int? id)
        {
            try
            {
                // Check if there is an ID
                if (!id.HasValue) return RedirectToAction("NotFound", "Error");

                // Check if request exists
                var rental = Rental.GetRentalById(id.Value);
                if (rental == null) return RedirectToAction("NotFound", "Error");

                // Check if the location belongs to the user
                User usr = Useraccount.GetUserByEmail(User.Identity.Name);
                var loc = Location.GetLocationById(rental.LocationId);
                if (usr.Id != loc.UserId) return RedirectToAction("NotFound", "Error");

                // Check if it is already approved
                if (rental.Approved) return RedirectToAction("NotFound", "Error");

                // Check if user has already approved another request that is conflicting with this one
                if (!Rental.CheckRentalAvailability(rental.LocationId, rental.DateFrom, rental.DateTo))
                {
                    int requesterId = rental.UserId;

                    // Automatically delete request because it is not possible
                    Rental.DeleteRental(rental.Id);

                    // Send e-mail to requester
                    EmailResult denied = new EmailController().DeniedMail(Useraccount.GetUserById(requesterId), loc.Title);
                    Task.Run(() => denied.Deliver());

                    // Give the user a heads up
                    Message.Flash(TempData, "Dit verzoek kon niet goedgekeurd worden omdat de datums overlappen met een reeds geaccepteerde aanvraag.");
                    return RedirectToAction("Requests");
                }

                // Everything OK, Approve the request
                Rental.Approve(rental.Id);

                // Send e-mail to requester
                EmailResult congrats = new EmailController().AcceptedMail(Useraccount.GetUserById(rental.UserId), loc.Title);
                Task.Run(() => congrats.Deliver());

                return RedirectToAction("Requests");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: /Rental/RejectRequest/{id}
        public ActionResult RejectRequest(int? id)
        {
            try
            {
                // Check if there is an ID
                if (!id.HasValue) return RedirectToAction("NotFound", "Error");

                // Check if request exists
                var rental = Rental.GetRentalById(id.Value);
                if (rental == null) return RedirectToAction("NotFound", "Error");

                // Check if the location belongs to the user
                User usr = Useraccount.GetUserByEmail(User.Identity.Name);
                var loc = Location.GetLocationById(rental.LocationId);
                if (usr.Id != loc.UserId) return RedirectToAction("NotFound", "Error");

                // Everything OK, Reject the request
                int uid = rental.UserId;
                Rental.DeleteRental(rental.Id);

                // Send e-mail to requester
                EmailResult denied = new EmailController().DeniedMail(Useraccount.GetUserById(uid), loc.Title);
                Task.Run(() => denied.Deliver());

                return RedirectToAction("Requests");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: /Rental/Rent/{id}      
        public ActionResult Rent(int? id)
        {
            try
            {
                // Check if there is an ID
                if (!id.HasValue) return RedirectToAction("Index", "Home");

                // Check if location exists
                var location = Location.GetLocationById(id.Value);
                if (location == null) return RedirectToAction("NotFound", "Error");

                // Check if user is trying to rent his/her own location
                User usr = Useraccount.GetUserByEmail(User.Identity.Name);
                if (location.UserId == usr.Id)
                {
                    Message.Flash(TempData, "U kunt uw eigen locatie niet huren...", MessageType.Error);
                    return RedirectToAction("Detail", "Locations", new { @id = location.Id });
                }

                // Check if location has been approved
                if (!location.Approved) return RedirectToAction("NotFound", "Error");

                // Check if user already has a pending request
                var rental = Rental.GetPendingRental(usr.Id, location.Id);
                if (rental != null)
                {
                    Message.Flash(TempData, "Voor deze locatie heeft u nog een aanvraag die in afwachting is. U moet wachten tot deze locatie goed- of afgekeurd wordt vooraleer u een nieuwe aanvraag kunt sturen.", MessageType.Error);
                    return RedirectToAction("Detail", "Locations", new { @id = location.Id });
                }

                // Create model and populate with default values
                var rent = new RentModel()
                {
                    LocationID = location.Id,
                    From = DateTime.Today,
                    To = DateTime.Today.AddDays(1)
                };

                // Set Location name
                TempData["Location"] = location.Title;

                return View(rent);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: /Rental/Requests
        public ActionResult Requests()
        {
            try
            {
                ViewBag.Pending = Rental.GetPendingRentalRequestsForUser(User.Identity.Name);
                ViewBag.Approved = Rental.GetApprovedRentalRequestsForUser(User.Identity.Name);
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }
        #endregion

        #region POST
        // POST: /Rental/Rent
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rent(RentModel model)
        {
            try
            {
                // Make sure there are no model errors
                if (ModelState.IsValid)
                {
                    // Check if there is an accepted rental that is conflicting with this one
                    if (!Rental.CheckRentalAvailability(model.LocationID, model.From, model.To))
                    {
                        ModelState.AddModelError(string.Empty, "De datums overlappen met een andere aanvraag die reeds geaccepteerd is door de aanbieder.");
                        return View(model);
                    }

                    // Add rental
                    Rental rent = Rental.InsertRental(Useraccount.GetUserByEmail(User.Identity.Name).Id, model);

                    // Summary e-mail to requesting person
                    User requester = Useraccount.GetUserByEmail(User.Identity.Name);
                    Location loc = Location.GetLocationById(rent.LocationId);
                    EmailResult summary = new EmailController().SummaryMail(requester, loc.Title, rent);

                    // Notification e-mail to owner
                    EmailResult notification = new EmailController().NotificationMail(Useraccount.GetUserById(loc.UserId), requester, loc.Title, rent);

                    // Send mails
                    Task.Run(() =>
                    {
                        summary.Deliver();
                        notification.Deliver();
                    });

                    // Display success
                    Message.Flash(TempData, "Aanvraag is succesvol verzonden. Kijk uw mailbox na voor een samenvatting van uw aanvraag.", MessageType.Success);
                    return RedirectToAction("Detail", "Locations", new { @id = model.LocationID });
                }

                // There are some errors
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }
        #endregion
    }
}