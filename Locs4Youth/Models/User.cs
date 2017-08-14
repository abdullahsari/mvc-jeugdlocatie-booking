using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Locs4Youth.Models
{
    public partial class User
    {
        /// <summary>
        /// Activates a user
        /// </summary>
        /// <param name="userId">User's ID</param>
        public static void Activate(int userId)
        {
            using (var db = new BookingDataContext())
            {
                User usr = (from u in db.Users
                            where u.Id == userId
                            select u).FirstOrDefault();
                usr.Activated = true;
                db.SubmitChanges();
            }
        }

        /// <summary>
        /// Changes the user's password
        /// </summary>
        /// <param name="email">User's unique e-mail</param>
        /// <param name="newHash">The new password hash</param>
        public static void ChangePassword(string email, string newHash)
        {
            using (var db = new BookingDataContext())
            {
                User usr = (from u in db.Users
                            where u.Email == email
                            select u).FirstOrDefault();
                usr.Password = newHash;
                db.SubmitChanges();
            }
        }

        /// <summary>
        /// Checks if user is in given role
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="role">User's potential role</param>
        /// <returns>Is in role or not</returns>
        public static bool CheckUserInRole(string email, string role)
        {
            using (var db = new BookingDataContext())
            {
                try
                {
                    return db.Users.Any(u => u.Email == email && u.Role == role);
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Retrieves all users
        /// </summary>
        /// <returns>List of every user</returns>
        public static List<User> GetAllUsers()
        {
            using (var db = new BookingDataContext())
            {
                return (from u in db.Users
                        orderby u.Email ascending
                        select u).ToList();
            }
        }

        /// <summary>
        /// Gets the roles the user belongs to
        /// </summary>
        /// <returns>User's roles</returns>
        public static string[] GetRoles(string email)
        {
            using (var db = new BookingDataContext())
            {
                try
                {
                    string[] roles = db.Users.Where(u => u.Email == email).Select(u => u.Role).ToArray();
                    if (roles[0] == null) return new string[] { "user" };
                    return roles;
                }
                catch (Exception)
                {
                    return new string[] { "user" };
                }
            }
        }

        /// <summary>
        /// Retrieves a user by e-mail
        /// </summary>
        /// <param name="email">The user's e-mail address</param>
        /// <returns>The found user</returns>
        public static User GetUserByEmail(string email)
        {
            using (var db = new BookingDataContext())
            {
                return (from u in db.Users
                        where u.Email == email
                        select u).FirstOrDefault();
            }
        }

        /// <summary>
        /// Retrieves a user by ID
        /// </summary>
        /// <param name="uid">The user's ID</param>
        /// <returns>The found user</returns>
        public static User GetUserById(int uid)
        {
            using (var db = new BookingDataContext())
            {
                return (from u in db.Users
                        where u.Id == uid
                        select u).FirstOrDefault();
            }
        }

        /// <summary>
        /// Get user ProfileModel by e-mail
        /// </summary>
        /// <param name="email">The user's e-mail</param>
        /// <returns>A ProfileModel object</returns>
        public static ProfileModel GetUserProfile(string email)
        {
            using (var db = new BookingDataContext())
            {
                return (from u in db.Users
                        where u.Email == email
                        select new ProfileModel()
                        {
                            Id = u.Id,
                            Name = $"{u.Firstname} {u.Lastname}",
                            Email = email
                        }).FirstOrDefault();
            }
        }


        /// <summary>
        /// Promotes a user to an administrator role
        /// </summary>
        /// <param name="id">The user's ID</param>
        public static void Promote(int id)
        {
            using (var db = new BookingDataContext())
            {
                User usr = (from u in db.Users
                            where u.Id == id
                            select u).FirstOrDefault();
                usr.Role = "admin";
                db.SubmitChanges();
            }
        }
    }
}