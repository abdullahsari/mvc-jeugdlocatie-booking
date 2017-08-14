using Locs4Youth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Locs4Youth.Controllers.Api
{
    /// <summary>
    /// Provides information about locations
    /// </summary>
    public class SharedLocationsController : ApiController
    {
        /// <summary>
        /// Gets all locations that match the querystring (does not return anything if none of the parameters are set)
        /// </summary>
        /// <param name="location">(Optional) The name of the location</param>
        /// <param name="address">(Optional) The address (street + number) of the location</param>
        /// <param name="start_date">(Optional) Availability of location as of start_date</param>
        /// <param name="end_date">(Optional) Availability of location until end_date</param>
        /// <param name="guests">(Optional) The minimum amount of guests (capacity of the location)</param>
        /// <returns>Matched locations</returns>
        [HttpGet]
        [ResponseType(typeof(List<LocationInfo>))]
        public IHttpActionResult Get(string location = null, string address = null, DateTime? start_date = null, DateTime? end_date = null, int? guests = null)
        {
            // Never return all locations
            if (location == null && address == null && (!start_date.HasValue || !end_date.HasValue) && !guests.HasValue) return NotFound();

            // Fetch locations
            List<Location> locs = Location.GetAllApprovedLocations();

            // Filter by address
            if (address != null) locs = locs.Where(l => l.Address.ToLower().Contains(address.ToLower())).ToList();

            // Filter by guests
            if (guests.HasValue) locs = locs.Where(l => guests.Value <= l.Capacity).ToList();

            // Filter by location
            if (location != null) locs = locs.Where(l => l.Title.ToLower().Contains(location.ToLower())).ToList();

            // Filter by availability
            if (start_date.HasValue && end_date.HasValue)
            {
                var temp = new List<Location>();
                foreach (Location loc in locs)
                {
                    if (Rental.CheckRentalAvailability(loc.Id, start_date.Value, end_date.Value))
                        temp.Add(loc);
                }
                locs = temp;
            }

            // Check if there are any locations left
            if (locs.Count == 0) return NotFound();

            // Put the locations in correct response format
            var info = new List<LocationInfo>();
            string img_path = Url.Content("~/Content/img/locations/");
            foreach (Location loc in locs)
            {
                info.Add(new LocationInfo()
                {
                    name = loc.Title,
                    city = loc.City,
                    image = new Uri($"{img_path}{loc.Id}.jpg"),
                    url = new Uri(Url.Link("Default", new { @controller = "Locations", @action = "Detail", @id = loc.Id }))
                });
            }

            // Respond
            return Json(info);
        }
    }
}