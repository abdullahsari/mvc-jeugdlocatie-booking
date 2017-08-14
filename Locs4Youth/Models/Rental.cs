using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;

namespace Locs4Youth.Models
{
    public partial class Rental
    {
        /// <summary>
        /// Approves a rental by ID
        /// </summary>
        /// <param name="id">Rental's ID</param>
        public static void Approve(int id)
        {
            using (var db = new BookingDataContext())
            {
                var rent = (from r in db.Rentals
                            where r.Id == id
                            select r).FirstOrDefault();
                rent.Approved = true;
                db.SubmitChanges();
            }
        }

        /// <summary>
        /// Determines if a location is available between two dates
        /// </summary>
        public static bool CheckRentalAvailability(int locationId, DateTime start, DateTime end)
        {
            using (var db = new BookingDataContext())
            {
                return !(from r in db.Rentals
                         where r.LocationId == locationId && r.Approved == true &&
                         (((start >= r.DateFrom && start <= r.DateTo) || (end >= r.DateFrom && end <= r.DateTo)) || (r.DateFrom >= start && r.DateTo <= end))
                         select r).Any();
            }
        }

        /// <summary>
        /// Determines whether user has an approved rental in the past for the given location
        /// </summary>
        public static bool HasApprovedRentalInPast(int locationId, string email)
        {
            using (var db = new BookingDataContext())
            {
                return (from r in db.Rentals
                        where r.Approved == true && r.LocationId == locationId && r.DateTo < DateTime.Now && r.User.Email == email
                        select r).Any();
            }
        }

        /// <summary>
        /// Gets all rentals for a user
        /// </summary>
        /// <param name="email">User's email</param>
        /// <returns>All rental for user</returns>
        public static List<Rental> GetAllRentalsForUser(string email)
        {
            using (var db = new BookingDataContext())
            {
                // eager loading
                DataLoadOptions options = new DataLoadOptions();
                options.LoadWith<Rental>(r => r.Location);
                db.LoadOptions = options;
                return (from r in db.Rentals
                        orderby r.DateFrom ascending
                        where r.User.Email == email
                        select r).ToList();
            }
        }

        /// <summary>
        /// Gets all pending rentals amount for a user
        /// </summary>
        /// <param name="email">User's email</param>
        /// <returns>All pending rentals amount</returns>
        public static int GetPendingRentalsAmountForUser(string email)
        {
            using (var db = new BookingDataContext())
            {
                return (from r in db.Rentals
                        where r.Approved == false && r.Location.User.Email == email
                        select r).Count();
            }
        }

        /// <summary>
        /// Gets all approved rentals as fullcalendar objects
        /// </summary>
        /// <param name="locationId">The location's ID</param>
        /// <returns>List of fullcalendar</returns>
        public static List<Fullcalendar> GetApprovedRentalsForLocationAsFullcalendar(int locationId)
        {
            using (var db = new BookingDataContext())
            {
                return (from r in db.Rentals
                        where r.Approved == true && r.LocationId == locationId
                        select new Fullcalendar()
                        {
                            start = r.DateFrom,
                            end = r.DateTo
                        }).ToList();
            }
        }

        /// <summary>
        /// Gets all approved rentals for a user
        /// </summary>
        /// <param name="email">User's email</param>
        /// <returns>All rental for user</returns>
        public static List<Rental> GetApprovedRentalRequestsForUser(string email)
        {
            using (var db = new BookingDataContext())
            {
                // eager loading
                DataLoadOptions options = new DataLoadOptions();
                options.LoadWith<Rental>(r => r.Location);
                options.LoadWith<Rental>(r => r.User);
                db.LoadOptions = options;
                return (from r in db.Rentals
                        where r.Approved == true && r.Location.User.Email == email
                        select r).ToList();
            }
        }

        /// <summary>
        /// Gets pending location rental for user
        /// </summary>
        /// <param name="userId">User's ID</param>
        /// <param name="locationId">Location ID</param>
        /// <returns>All pending rentals</returns>
        public static Rental GetPendingRental(int userId, int locationId)
        {
            using (var db = new BookingDataContext())
            {
                return (from r in db.Rentals
                        where r.UserId == userId && r.LocationId == locationId && r.Approved == false
                        select r).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets all pending rental requests that belongs to user
        /// </summary>
        /// <param name="email">User's email</param>
        /// <returns>All pending rentals</returns>
        public static List<Rental> GetPendingRentalRequestsForUser(string email)
        {
            using (var db = new BookingDataContext())
            {
                // eager loading
                var options = new DataLoadOptions();
                options.LoadWith<Rental>(r => r.Location);
                options.LoadWith<Rental>(r => r.User);
                db.LoadOptions = options;
                return (from r in db.Rentals
                        where r.Approved == false && r.Location.User.Email == email
                        select r).ToList();
            }
        }

        /// <summary>
        /// Gets a rental by id
        /// </summary>
        /// <param name="id">Rental ID</param>
        /// <returns>Rental</returns>
        public static Rental GetRentalById(int id)
        {
            using (var db = new BookingDataContext())
            {
                return (from r in db.Rentals
                        where r.Id == id
                        select r).FirstOrDefault();
            }
        }

        /// <summary>
        /// Insert a new rental entry
        /// </summary>
        /// <param name="userId">The user's ID</param>
        /// <param name="model">The Rent View Model</param>
        public static Rental InsertRental(int userId, RentModel model)
        {
            using (var db = new BookingDataContext())
            {
                var rent = new Rental()
                {
                    Message = string.IsNullOrWhiteSpace(model.Message) ? null : model.Message.Trim(),
                    DateFrom = model.From,
                    DateTo = model.To,
                    LocationId = model.LocationID,
                    UserId = userId
                };
                db.Rentals.InsertOnSubmit(rent);
                db.SubmitChanges();
                return rent;
            }
        }

        /// <summary>
        /// Deletes a rental by ID
        /// </summary>
        /// <param name="id">Rental ID</param>
        public static void DeleteRental(int id)
        {
            using (var db = new BookingDataContext())
            {
                var rent = (from r in db.Rentals
                            where r.Id == id
                            select r).FirstOrDefault();
                db.Rentals.DeleteOnSubmit(rent);
                db.SubmitChanges();
            }
        }
    }
}