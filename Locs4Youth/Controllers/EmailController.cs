using ActionMailerNext.Mvc5_2;
using Locs4Youth.Models;
using System;
using System.Net.Mail;

namespace Locs4Youth.Controllers
{
    /// <summary>
    /// This controller handles everything related to sending e-mails
    /// </summary>
    public class EmailController : MailerBase
    {
        #region CONSTANTS
        private const string EMAIL = "locs4youth@gmail.com";
        #endregion

        #region ACTIVATION
        /// <summary>
        /// Generates a ready-to-be-sent activation e-mail using SMTP
        /// </summary>
        /// <param name="usr">The user the mail belongs to</param>
        /// <param name="activationCode">Unique Activation ID</param>
        /// <returns>E-mail</returns>
        public EmailResult ActivationMail(User usr, Guid activationCode)
        {
            MailAttributes.From = new MailAddress(EMAIL);
            MailAttributes.To.Add(new MailAddress(usr.Email));
            MailAttributes.Subject = "Account activatie - Locs 4 Youth";
            return Email(
                "Activation", 
                new ActivationEmailModel()
                {
                    User = usr,
                    ActivationCode = activationCode
                }
            );
        }
        #endregion

        #region DENIED
        /// <summary>
        /// Generates a ready-to-be-sent denied e-mail using SMTP
        /// </summary>
        /// <param name="usr">The user the mail belongs to</param>
        /// <param name="locationName">Name of denied location</param>
        /// <returns>E-mail</returns>
        public EmailResult DeniedMail(User usr, string locationName)
        {
            MailAttributes.From = new MailAddress(EMAIL);
            MailAttributes.To.Add(new MailAddress(usr.Email));
            MailAttributes.Subject = $"Aanvraag afgekeurd voor {locationName}";
            return Email(
                "Denied",
                new DeniedEmailModel()
                {
                    User = usr,
                    LocationName = locationName
                }
            );
        }
        #endregion

        #region ACCEPTED
        /// <summary>
        /// Generates a ready-to-be-sent accepted e-mail using SMTP
        /// </summary>
        /// <param name="usr">The user the mail belongs to</param>
        /// <param name="locationName">Name of denied location</param>
        /// <returns>E-mail</returns>
        public EmailResult AcceptedMail(User usr, string locationName)
        {
            MailAttributes.From = new MailAddress(EMAIL);
            MailAttributes.To.Add(new MailAddress(usr.Email));
            MailAttributes.Subject = $"Aanvraag goedgekeurd voor {locationName}";
            return Email(
                "Accepted",
                new AcceptedEmailModel()
                {
                    User = usr,
                    LocationName = locationName
                }
            );
        }
        #endregion

        #region SUMMARY
        /// <summary>
        /// Generates a ready-to-be-sent summary e-mail using SMTP
        /// </summary>
        /// <param name="usr">The user the mail belongs to</param>
        ///         /// <param name="locationName">Name of location</param>
        /// <param name="rent">The rent request this email is for</param>
        /// <returns>E-mail</returns>
        public EmailResult SummaryMail(User usr, string locationName, Rental rent)
        {
            MailAttributes.From = new MailAddress(EMAIL);
            MailAttributes.To.Add(new MailAddress(usr.Email));
            MailAttributes.Subject = $"Samenvatting van aanvraag ({locationName})";
            return Email(
                "Summary",
                new SummaryEmailModel()
                {
                    User = usr,
                    LocationName = locationName,
                    Rental = rent
                }
            );
        }
        #endregion

        #region NOTIFICATION
        /// <summary>
        /// Generates a ready-to-be-sent notification e-mail using SMTP
        /// </summary>
        /// <returns>E-mail</returns>
        public EmailResult NotificationMail(User owner, User requester, string locationName, Rental rent)
        {
            MailAttributes.From = new MailAddress(requester.Email);
            MailAttributes.To.Add(new MailAddress(owner.Email));
            MailAttributes.Subject = $"Aanvraag tot verhuur ({locationName})";
            return Email(
                "Notification",
                new NotificationEmailModel()
                {
                    Owner = owner,
                    Requester = requester,
                    LocationName = locationName,
                    Rental = rent
                }
            );
        }
        #endregion
    }
}