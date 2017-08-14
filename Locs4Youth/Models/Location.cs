using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Locs4Youth.Utils;
using System.Data.Linq;

namespace Locs4Youth.Models
{
    public partial class Location
    {
        /// <summary>
        /// Approves a location by ID
        /// </summary>
        /// <param name="id">Location's ID</param>
        public static void Approve(int id)
        {
            using (var db = new BookingDataContext())
            {
                Location location = (from l in db.Locations
                                     where l.Id == id
                                     select l).FirstOrDefault();
                location.Approved = true;
                db.SubmitChanges();
            }
        }

        /// <summary>
        /// Retrieves all approved locations
        /// </summary>
        /// <returns>List of approved locations</returns>
        public static List<Location> GetAllApprovedLocations()
        {
            using (var db = new BookingDataContext())
            {
                return (from l in db.Locations
                        where l.Approved == true
                        select l).ToList();
            }
        }

        /// <summary>
        /// Retrieves all approved locations by region
        /// </summary>
        /// <returns>List of approved locations</returns>
        public static List<Location> GetApprovedLocationsByRegion(Region region)
        {
            using (var db = new BookingDataContext())
            {
                return (from l in db.Locations
                        where l.Approved == true && l.Region == region.GetEnumDisplay()
                        select l).ToList();
            }
        }

        /// <summary>
        /// Fetches a location entry from DB by id
        /// </summary>
        /// <param name="id">Location's ID</param>
        /// <returns>A location object</returns>
        public static Location GetLocationById(int id)
        {
            using (var db = new BookingDataContext())
            {
                // eager loading
                var options = new DataLoadOptions();
                options.LoadWith<Location>(l => l.User);
                db.LoadOptions = options;
                return (from l in db.Locations
                        where l.Id == id
                        select l).FirstOrDefault();
            }
        }

        /// <summary>
        /// Retrieves all of the unconfirmed locations
        /// </summary>
        /// <returns>Unconfirmed locations</returns>
        public static List<Location> GetUnconfirmedLocations()
        {
            using (var db = new BookingDataContext())
            {
                // eager loading
                var options = new DataLoadOptions();
                options.LoadWith<Location>(l => l.User);
                db.LoadOptions = options;
                return (from l in db.Locations
                        orderby l.Date ascending
                        where l.Approved == false
                        select l).ToList();
            }
        }

        /// <summary>
        /// Calculates the amount of unconfirmed locations
        /// </summary>
        /// <returns>amount of unconfirmed locations</returns>
        public static int GetUnconfirmedAmount()
        {
            using (var db = new BookingDataContext())
            {
                return (from l in db.Locations
                        where l.Approved == false
                        select l).Count();
            }
        }

        /// <summary>
        /// Calculates the amount of confirmed locations for user
        /// </summary>
        /// <returns>amount of unconfirmed locations</returns>
        public static int GetConfirmedAmountForUser(string email)
        {
            using (var db = new BookingDataContext())
            {
                return (from l in db.Locations
                        where l.Approved == true && l.User.Email == email
                        select l).Count();
            }
        }

        /// <summary>
        /// Calculates the amount of unconfirmed locations for user
        /// </summary>
        /// <returns>amount of unconfirmed locations</returns>
        public static int GetUnconfirmedAmountForUser(string email)
        {
            using (var db = new BookingDataContext())
            {
                return (from l in db.Locations
                        where l.Approved == false && l.User.Email == email
                        select l).Count();
            }
        }

        /// <summary>
        /// Fetches all locations that belong to one user
        /// </summary>
        /// <param name="email">The user's e-mail</param>
        /// <returns>List of locations</returns>
        public static List<Location> GetAllLocationsForUser(string email)
        {
            using (var db = new BookingDataContext())
            {
                return (from l in db.Locations
                        orderby l.Date ascending
                        where l.User.Email == email
                        select l).ToList();
            }
        }

        /// <summary>
        /// Creates a new location entry in the database
        /// </summary>
        /// <param name="userId">The user the location belongs to</param>
        /// <param name="model">Information about the location</param>
        /// <returns>The ID of the inserted location</returns>
        public static int InsertLocationOffer(int userId, OfferModel model)
        {
            using (var db = new BookingDataContext())
            {
                string region = model.Region.GetEnumDisplay();
                var loc = new Location()
                {
                    Address = model.Address.Trim().ToLower().Capitalise(" "),
                    Capacity = model.Capacity,
                    City = model.City.Trim().ToLower().Capitalise(" -"),
                    Date = DateTime.Now,
                    Description = model.Description,
                    Region = model.Region.GetEnumDisplay(),
                    UserId = userId,
                    Title = model.Title.Trim().ToLower().Capitalise(" ")
                };
                db.Locations.InsertOnSubmit(loc);
                db.SubmitChanges();
                return loc.Id;
            }
        }

        /// <summary>
        /// Deletes a location by ID
        /// </summary>
        /// <param name="id">Location's ID</param>
        public static void Delete(int id)
        {
            using (var db = new BookingDataContext())
            {
                Location location = (from l in db.Locations
                                     where l.Id == id
                                     select l).FirstOrDefault();
                db.Locations.DeleteOnSubmit(location);
                db.SubmitChanges();
            }
        }

        /// <summary>
        /// Updates a location
        /// </summary>
        /// <param name="model">Edit model</param>
        public static void Update(EditModel model)
        {
            using (var db = new BookingDataContext())
            {
                Location loc = (from l in db.Locations where l.Id == model.LocationID select l).FirstOrDefault();
                loc.Address = model.Address.Trim().ToLower().Capitalise(" ");
                loc.Capacity = model.Capacity;
                loc.City = model.City.Trim().ToLower().Capitalise(" -");
                loc.Date = DateTime.Now;
                loc.Description = model.Description;
                loc.Region = model.Region.GetEnumDisplay();
                loc.Title = model.Title.Trim().ToLower().Capitalise(" ");
                db.SubmitChanges();
            }
        }
    }
}