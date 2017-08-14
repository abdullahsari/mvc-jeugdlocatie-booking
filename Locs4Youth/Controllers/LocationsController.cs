using Locs4Youth.Models;
using Locs4Youth.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Useraccount = Locs4Youth.Models.User;

namespace Locs4Youth.Controllers
{
    /// <summary>
    /// This controller handles everyting related to locations
    /// </summary>
    public class LocationsController : Controller
    {
        #region GET
        // GET: /Locations/Detail/{id}
        [AllowAnonymous]
        public ActionResult Detail(int? id)
        {
            try
            {
                // Redirect if ID is missing
                if (!id.HasValue) return RedirectToAction("NotFound", "Error");
                var location = Location.GetLocationById(id.Value);

                // Check if location exists
                if (location == null) return RedirectToAction("NotFound", "Error");

                // Check if location has been approved
                if (!location.Approved)
                {
                    // Is the user authenticated
                    if (!User.Identity.IsAuthenticated) return RedirectToAction("NotFound", "Error");

                    // Is the request coming from the owner
                    if (location.UserId == Useraccount.GetUserByEmail(User.Identity.Name).Id)
                    {
                        // Inform owner about page not being live
                        Message.Flash(TempData, "Hoewel u uw eigen locatie kunt bekijken, is het nog niet live op de website.");
                    }
                    else
                    {
                        // Is the request coming from an admin
                        if (!User.IsInRole("admin")) return RedirectToAction("NotFound", "Error");
                    }
                }

                // populate with data
                List<Rating> ratings = Rating.GetRatingsForLocation(id.Value);
                ViewBag.Ratings = ratings;
                ViewBag.Location = location;
                ViewBag.Features = Location_has_Feature.GetFeaturesForLocation(id.Value);
                ViewBag.Average = ratings.Any() ? ratings.Select(r => (double)r.Score).Average() : 0;
                RateModel model = User.Identity.IsAuthenticated ? new RateModel() { LocationID = location.Id } : null;
                if (model != null) ViewBag.CanRate = Rental.HasApprovedRentalInPast(location.Id, User.Identity.Name);

                // display
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: /Locations/Edit/{id}
        [Authorize]
        public ActionResult Edit(int? id)
        {
            try
            {
                // Check if there is an ID
                if (!id.HasValue) return RedirectToAction("NotFound", "Error");

                // Check if location exists
                var loc = Location.GetLocationById(id.Value);
                if (loc == null) return RedirectToAction("NotFound", "Error");

                // Check if location belongs to user
                User usr = Useraccount.GetUserByEmail(User.Identity.Name);
                if (loc.UserId != usr.Id) return RedirectToAction("NotFound", "Error");

                // Check if location is in pending state
                if (loc.Approved)
                {
                    Message.Flash(TempData, "Het is niet meer mogelijk om deze locatie te bewerken omdat het al geaccepteerd is door een beheerder.", MessageType.Error);
                    return RedirectToAction("Locations", "Account");
                }

                // Convert region string to appropriate enumeration
                Region region;
                Enum.TryParse(loc.Region.Replace("-", string.Empty), out region);

                // Check the features that are included
                List<CheckBoxListItem> features = Feature.GetFeaturesAsCheckBoxListItem();
                foreach (string name in Location_has_Feature.GetFeaturesForLocation(loc.Id))
                {
                    foreach (CheckBoxListItem feat in features)
                    {
                        if (feat.Display == name) feat.IsChecked = true;
                    }
                }

                // Scaffold model
                var model = new EditModel()
                {
                    Title = loc.Title,
                    Address = loc.Address,
                    City = loc.City,
                    Description = loc.Description,
                    Capacity = loc.Capacity,
                    Region = region,
                    Features = features,
                    LocationID = loc.Id
                };

                // Display
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: /Locations/Index
        [AllowAnonymous]
        public ActionResult Index(string province)
        {
            try
            {
                var model = new FilterModel();
                List<Location> locations;

                // Region not included in querystring
                if (province == null)
                {
                    locations = Location.GetAllApprovedLocations();
                    ViewBag.Locations = locations;
                    return View(model);
                }

                // Region is included in querystring
                Region enumregion;
                if (Enum.TryParse(province.Replace("-", string.Empty), out enumregion))
                {
                    model.Region = enumregion;
                    locations = Location.GetApprovedLocationsByRegion(enumregion);
                }
                else
                {
                    locations = Location.GetAllApprovedLocations();
                }

                // Display
                ViewBag.Locations = locations;
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: /Locations/Offer
        [Authorize]
        public ActionResult Offer()
        {
            try
            {
                var offer = new OfferModel();
                offer.Features = Feature.GetFeaturesAsCheckBoxListItem();
                return View(offer);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }
        #endregion

        #region POST
        // POST: /Locations/Index
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Index(FilterModel model)
        {
            try
            {
                List<Location> locs = Location.GetAllApprovedLocations();

                // Make sure there are no model errors
                if (ModelState.IsValid)
                {
                    // Filter by region
                    if (model.Region.HasValue) locs = locs.Where(l => l.Region == model.Region.Value.GetEnumDisplay()).ToList();

                    // Filter by capacity
                    if (model.Capacity.HasValue) locs = locs.Where(l => model.Capacity.Value <= l.Capacity).ToList();

                    // Filter by title
                    if (model.Title != null) locs = locs.Where(l => l.Title.ToLower().Contains(model.Title.ToLower())).ToList();

                    // Filter by availability
                    if (model.From.HasValue && model.To.HasValue)
                    {
                        var temp = new List<Location>();
                        foreach (Location loc in locs)
                        {
                            if (Rental.CheckRentalAvailability(loc.Id, model.From.Value, model.To.Value))
                                temp.Add(loc);
                        }
                        locs = temp;
                    }

                    // Fetch from partners if no locations available
                    if (locs.Count == 0)
                    {
                        List<SharedLocation> matthias = Partners.Matthias(model);
                        List<SharedLocation> laurens = Partners.Laurens(model);
                        ViewBag.PartnerLocations = matthias.Concat(laurens).ToList();
                    }
                }

                ViewBag.Locations = locs;
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // POST: /Locations/Edit
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditModel model)
        {
            try
            {
                // Make sure there are no model errors
                if (ModelState.IsValid)
                {
                    // Check if location exists
                    var loc = Location.GetLocationById(model.LocationID);
                    if (loc == null) return RedirectToAction("NotFound", "Error");

                    // Check if location belongs to user
                    User usr = Useraccount.GetUserByEmail(User.Identity.Name);
                    if (loc.UserId != usr.Id) return RedirectToAction("NotFound", "Error");

                    // Check if location is in pending state
                    if (loc.Approved)
                    {
                        Message.Flash(TempData, "Het is niet meer mogelijk om deze locatie te bewerken omdat het al geaccepteerd is door een beheerder.", MessageType.Error);
                        return RedirectToAction("Locations", "Account");
                    }

                    // Sanitize description 
                    model.Description = HTMLHelper.Clean(model.Description);

                    // Update location
                    Location.Update(model);

                    // Remove previous features
                    Location_has_Feature.DeleteFeaturesForLocation(model.LocationID);

                    // Insert new ones
                    Location_has_Feature.InsertFeaturesForLocation(model.LocationID, model.Features);

                    // Check if there is a new image
                    if (model.Image != null)
                    {
                        string dir = Server.MapPath("~/Content/img/locations/");

                        // Delete previous one
                        ImageHelper.DeleteImage(model.LocationID, dir);

                        // Resize location image to 500x400 and save
                        ImageHelper.SaveImage(ImageHelper.ResizeImage(System.Drawing.Image.FromStream(model.Image.InputStream, true, true), 500, 400), model.LocationID, dir);
                    }

                    // Flash success
                    Message.Flash(TempData, "Locatie succesvol bewerkt.", MessageType.Success);
                    return RedirectToAction("Locations", "Account");
                }

                // Refresh checkbox names
                var features = Feature.GetFeaturesAsCheckBoxListItem();
                foreach (CheckBoxListItem item in features)
                    model.Features[item.ID - 1].Display = item.Display;

                // There are errors, reload page and display them
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // POST: /Locations/Offer
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Offer(OfferModel model)
        {
            try
            {
                // Make sure there are no model errors
                if (ModelState.IsValid)
                {
                    User usr = Useraccount.GetUserByEmail(User.Identity.Name);

                    // Sanitize description 
                    model.Description = HTMLHelper.Clean(model.Description);

                    // Insert location
                    int locId = Location.InsertLocationOffer(usr.Id, model);

                    // Bind features to location
                    Location_has_Feature.InsertFeaturesForLocation(locId, model.Features);

                    // Save location image to hdd
                    string dir = Server.MapPath("~/Content/img/locations/");

                    // Resize location image to 500x400 and save
                    ImageHelper.SaveImage(ImageHelper.ResizeImage(System.Drawing.Image.FromStream(model.Image.InputStream, true, true), 500, 400), locId, dir);

                    // Flash success
                    Message.Flash(TempData, "Uw locatie is succesvol geplaatst. Een beheerder zal zo snel mogelijk de locatie goed- of afkeuren naargelang de echtheid. Ondertussen kunt u de status van het proces raadplegen op deze pagina.");
                    return RedirectToAction("Locations", "Account");
                }

                // Refresh checkbox names
                var features = Feature.GetFeaturesAsCheckBoxListItem();
                foreach (CheckBoxListItem item in features)
                    model.Features[item.ID - 1].Display = item.Display;

                // There are errors, reload page and display them
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        // POST: /Locations/Rate/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Rate(RateModel model)
        {
            try
            {
                // Make sure there are no model errors
                if (ModelState.IsValid)
                {
                    // Check if user can actually rate
                    bool canRate = Rental.HasApprovedRentalInPast(model.LocationID, User.Identity.Name);
                    if (!canRate) return RedirectToAction("Detail", new { @id = model.LocationID });
                    string message = null;
                    User usr = Useraccount.GetUserByEmail(User.Identity.Name);

                    // Check is user already has a rating
                    if (Rating.HasRating(User.Identity.Name, model.LocationID))
                    {
                        message = "Uw vorige beoordeling is succesvol geüpdatet. Het zal gecontroleerd worden vooraleer het op de pagina verschijnt.";
                        Rating.UpdateRating(usr.Id, model);
                    }
                    else
                    {
                        message = "Beoordeling is succesvol verzonden. Een beheerder zal de beoordeling nakijken vooraleer het op de pagina verschijnt. U kunt de status ervan controleren op uw profiel.";
                        Rating.AddRatingForLocation(usr.Id, model);
                    }

                    // Flash message
                    Message.Flash(TempData, message);
                    return RedirectToAction("Detail", new { @id = model.LocationID });
                }

                // There are errors, reload page and display them
                Message.Flash(TempData, "Beoordeling kon niet verzonden worden wegens validatiefout(en).", MessageType.Error);
                return RedirectToAction("Detail", new { @id = model.LocationID });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Error");
            }
        }
        #endregion
    }
}