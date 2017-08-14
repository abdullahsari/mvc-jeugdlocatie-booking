using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Locs4Youth.Models
{
    /// <summary>
    /// This model is used to populate client-side fullcalendar plugin
    /// </summary>
    public class Fullcalendar
    {
        private const string COLOR = "#008eb2";

        public Fullcalendar()
        {
            overlap = true;
            title = "Verhuurd";
            backgroundColor = COLOR;
            borderColor = COLOR;
        }

        /// <summary>
        /// The color to use with fullcalendar script
        /// </summary>
        public string backgroundColor { get; set; }

        /// <summary>
        /// The color to use with fullcalendar script
        /// </summary>
        public string borderColor { get; set; }

        /// <summary>
        /// End date of the booking
        /// </summary>
        public DateTime end { get; set; }

        /// <summary>
        /// Overlap on display
        /// </summary>
        public bool overlap { get; set; }

        /// <summary>
        /// Start date of the booking
        /// </summary>
        public DateTime start { get; set; }

        /// <summary>
        /// Text to display
        /// </summary>
        public string title { get; set; }
    }

    public class LocationInfo
    {
        private const string COUNTRY = "Belgium";

        /// <summary>
        /// Title/Name of the location
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// City of the location
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// Country of the location
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// Image URI of the location
        /// </summary>
        public Uri image { get; set; }

        /// <summary>
        /// URL to the location
        /// </summary>
        public Uri url { get; set; }

        public LocationInfo()
        {
            country = COUNTRY;
        }
    }
}