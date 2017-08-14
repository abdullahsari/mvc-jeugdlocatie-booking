using Locs4Youth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace Locs4Youth.Controllers.Api
{
    /// <summary>
    /// Provides rental data
    /// </summary>
    public class SharedRentalController : ApiController
    {
        /// <summary>
        /// Gets all approved rentals for a location
        /// </summary>
        /// <param name="id">The location's ID to get the rentals for</param>
        /// <returns>JSON of matched rentals</returns>
        [HttpGet]
        [ResponseType(typeof(List<Fullcalendar>))]
        public IHttpActionResult Get(int id)
        {
            // Check if location exists
            var loc = Location.GetLocationById(id);
            if (loc == null) return NotFound();

            // Check if location has been approved
            if (!loc.Approved) return NotFound();

            // Everything OK
            return Json(Rental.GetApprovedRentalsForLocationAsFullcalendar(id));
        }
    }
}