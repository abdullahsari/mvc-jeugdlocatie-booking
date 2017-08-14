using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace Locs4Youth.Models
{
    public partial class Rating
    {
        /// <summary>
        /// Approve a rating
        /// </summary>
        /// <param name="id">Rating ID</param>
        public static void Approve(int id)
        {
            using (var db = new BookingDataContext())
            {
                Rating rating = (from r in db.Ratings
                                 where r.Id == id
                                 select r).FirstOrDefault();
                rating.Approved = true;
                db.SubmitChanges();
            }
        }

        /// <summary>
        /// Updates an existing rating
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static void UpdateRating(int userId, RateModel model)
        {
            using (var db = new BookingDataContext())
            {
                Rating rating = (from r in db.Ratings
                              where r.LocationId == model.LocationID && r.UserId == userId
                              select r).FirstOrDefault();
                rating.Message = model.Message;
                rating.Date = DateTime.Now;
                rating.Score = (byte)model.Score;
                rating.Approved = false;
                db.SubmitChanges();
            }
        }

        /// <summary>
        /// Check whether a user already has a rating for a location
        /// </summary>
        /// <param name="email">User's e-mail</param>
        /// <returns>Boolean</returns>
        public static bool HasRating(string email, int locationId)
        {
            using (var db = new BookingDataContext())
            {
                return (from r in db.Ratings
                        where r.LocationId == locationId && r.User.Email == email
                        select r).Any();
            }
        }

        /// <summary>
        /// Adds a rating
        /// </summary>
        /// <param name="userId">The user's ID</param>
        /// <param name="model">RateModel</param>
        public static void AddRatingForLocation(int userId, RateModel model)
        {
            using (var db = new BookingDataContext())
            {
                var rating = new Rating()
                {
                    LocationId = model.LocationID,
                    Message = model.Message,
                    Date = DateTime.Now,
                    UserId = userId,
                    Score = (byte)model.Score
                };
                db.Ratings.InsertOnSubmit(rating);
                db.SubmitChanges();
            }
        }

        public static int GetAverageRatingForLocation(int locationId)
        {
            using (var db = new BookingDataContext())
            {
                return (int)(Math.Round((from r in db.Ratings
                                        where r.LocationId == locationId
                                        select (Double)r.Score).Average()));
            }
        }

        /// <summary>
        /// Gets the amount of confirmed ratings for user
        /// </summary>
        /// <returns>Amount</returns>
        public static int GetConfirmedRatingsAmountForUser(string email)
        {
            using (var db = new BookingDataContext())
            {
                return (from r in db.Ratings
                        where r.Approved == true && r.User.Email == email
                        select r).Count();
            }
        }

        /// <summary>
        /// Gets the amount of unconfirmed ratings
        /// </summary>
        /// <returns>Amount</returns>
        public static int GetUnconfirmedRatingsAmount()
        {
            using (var db = new BookingDataContext())
            {
                return (from r in db.Ratings
                        where r.Approved == false
                        select r).Count();
            }
        }

        /// <summary>
        /// Gets the amount of unconfirmed ratings for user
        /// </summary>
        /// <returns>Amout</returns>
        public static int GetUnconfirmedRatingsAmountForUser(string email)
        {
            using (var db = new BookingDataContext())
            {
                return (from r in db.Ratings
                        where r.Approved == false && r.User.Email == email
                        select r).Count();
            }
        }

        /// <summary>
        /// Gets the unconfirmed ratings
        /// </summary>
        /// <returns>List of unconfirmed ratings</returns>
        public static List<Rating> GetUnconfirmedRatings()
        {
            using (var db = new BookingDataContext())
            {
                // eager loading
                var options = new DataLoadOptions();
                options.LoadWith<Rating>(r => r.User);
                options.LoadWith<Rating>(r => r.Location);
                db.LoadOptions = options;
                return (from r in db.Ratings
                        orderby r.Date ascending
                        where r.Approved == false
                        select r).ToList();
            }
        }

        /// <summary>
        /// Gets all confirmed rating for given location
        /// </summary>
        /// <param name="locationId">The location ID</param>
        /// <returns>All confirmed ratings for location</returns>
        public static List<Rating> GetRatingsForLocation(int locationId)
        {
            using (var db = new BookingDataContext())
            {
                // eager loading
                var options = new DataLoadOptions();
                options.LoadWith<Rating>(r => r.User);
                db.LoadOptions = options;
                return (from r in db.Ratings
                        where r.Approved == true && r.LocationId == locationId
                        select r).ToList();
            }
        }

        /// <summary>
        /// Gets all ratings for given user
        /// </summary>
        /// <param name="email">User's email</param>
        /// <returns>User's all ratings</returns>
        public static List<Rating> GetRatingsForUser(string email)
        {
            using (var db = new BookingDataContext())
            {
                // eager loading
                var options = new DataLoadOptions();
                options.LoadWith<Rating>(r => r.Location);
                db.LoadOptions = options;
                return (from r in db.Ratings
                        where r.User.Email == email
                        select r).ToList();
            }
        }

        /// <summary>
        /// Delete a rating
        /// </summary>
        /// <param name="id">Rating ID</param>
        public static void Delete(int id)
        {
            using (var db = new BookingDataContext())
            {
                Rating rating = (from r in db.Ratings
                                 where r.Id == id
                                 select r).FirstOrDefault();
                db.Ratings.DeleteOnSubmit(rating);
                db.SubmitChanges();
            }
        }
    }
}