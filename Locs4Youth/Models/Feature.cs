using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Locs4Youth.Models
{
    public partial class Feature
    {
        /// <summary>
        /// Gets features as CheckBoxListItem
        /// </summary>
        /// <returns>List of CheckBoxListItems</returns>
        public static List<CheckBoxListItem> GetFeaturesAsCheckBoxListItem()
        {
            using (var db = new BookingDataContext())
            {
                return (from f in db.Features
                        select new CheckBoxListItem() {
                            ID = f.Id,
                            Display = f.Name,
                            IsChecked = false
                        }).ToList();
            }
        }
    }
}