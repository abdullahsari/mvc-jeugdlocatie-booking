using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security;
using Locs4Youth.Utils;
using Locs4Youth.Models;
using System.Net.Mail;
using Locs4Youth.Controllers;
using ActionMailerNext.Mvc5_2;

namespace Locs4Youth.Register
{
    /// <summary>
    /// Source: http://willseitz-code.blogspot.be/2013/06/cross-site-request-forgery-for-web-forms.html
    /// </summary>
    public static class AntiforgeryChecker
    {
        public static void Check(Page page, HiddenField antiforgery)
        {
            if (!page.IsPostBack)
            {
                var antiforgeryToken = Guid.NewGuid();
                page.Session["AntiforgeryToken"] = antiforgeryToken;
                antiforgery.Value = antiforgeryToken.ToString();
            }
            else
            {
                var stored = (Guid)page.Session["AntiforgeryToken"];
                var sent = new Guid(antiforgery.Value);
                if (sent != stored)
                {
                    throw new SecurityException("CSRF Attack Detected!");
                }
            }
        }
    }

    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Do not load page if user is already authenticated
            if (Request.IsAuthenticated) Response.Redirect("/Home/Index", true);

            // Prevent CSRF
            AntiforgeryChecker.Check(this, antiforgery);          
        }

        [ValidateAntiForgeryToken]
        protected async void submit_Click(object sender, EventArgs e)
        {
            lbl_message.Visible = false;
            if (Page.IsValid)
            {
                using (var db = new BookingDataContext())
                {
                    // Check if the user already exists
                    if (db.Users.Any(u => u.Email == Email.Text))
                    {
                        lbl_message.ForeColor = System.Drawing.Color.Red;
                        lbl_message.Text = "De gebruiker bestaat reeds.";
                        lbl_message.Visible = true;
                    }
                    else
                    {
                        // Create user with correct credentials
                        User usr = new User();
                        usr.Firstname = Firstname.Text.Trim().Capitalise(" ");
                        usr.Lastname = Lastname.Text.Trim().Capitalise(" ");
                        usr.Email = Email.Text.Trim().ToLower();

                        // Hash the password
                        usr.Password = PasswordStorage.CreateHash(Password.Text);
                        db.Users.InsertOnSubmit(usr);
                        db.SubmitChanges();

                        // Send an activation e-mail to user's e-mail address
                        await SendActivationEmail(usr).DeliverAsync();

                        // Display status
                        lbl_message.Text = "Registratie succesvol. Gelieve uw mailbox te controleren om activatie te voltooien.";
                        lbl_message.ForeColor = System.Drawing.Color.Green;
                        lbl_message.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// Prepares an activation e-mail
        /// </summary>
        /// <param name="usr">The user the mail belongs to</param>
        /// <returns>The e-mail</returns>
        private EmailResult SendActivationEmail(User usr)
        {
            Guid uniqueIdentifier = Guid.NewGuid();
            using (var db = new BookingDataContext())
            {
                var activation = new Activation();
                activation.UserId = usr.Id;
                activation.Code = uniqueIdentifier;
                db.Activations.InsertOnSubmit(activation);
                db.SubmitChanges();
            }
            return new EmailController().ActivationMail(usr, uniqueIdentifier);
        }
    }
 }
