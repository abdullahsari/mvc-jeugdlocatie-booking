using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Locs4Youth.Models
{
    public class ActivationEmailModel
    {
        public User User { get; set; }
        public Guid ActivationCode { get; set; }
    }

    public class SummaryEmailModel
    {
        public User User { get; set; }
        public Rental Rental { get; set; }
        public string LocationName { get; set; }
    }

    public class NotificationEmailModel
    {
        public User Owner { get; set; }
        public User Requester { get; set; }
        public string LocationName { get; set; }
        public Rental Rental { get; set; }
    }

    public class DeniedEmailModel
    {
        public User User { get; set; }
        public string LocationName { get; set; }
    }

    public class AcceptedEmailModel
    {
        public User User { get; set; }
        public string LocationName { get; set; }
    }
}