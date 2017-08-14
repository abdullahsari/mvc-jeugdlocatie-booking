using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Locs4Youth.Models
{
    public partial class Location_has_Feature
    {
        /// <summary>
        /// Deletes all features that belong to a location
        /// </summary>
        /// <param name="locationId">The location's ID</param>
        public static void DeleteFeaturesForLocation(int locationId)
        {
            using (var db = new BookingDataContext())
            {
                List<Location_has_Feature> features = (from lf in db.Location_has_Features
                                                       where lf.LocationId == locationId
                                                       select lf).ToList();
                db.Location_has_Features.DeleteAllOnSubmit(features);
                db.SubmitChanges();
            }
        }

        /// <summary>
        /// Populates the many-to-many table with entries associated to a location
        /// </summary>
        /// <param name="locationId">The location's ID</param>
        /// <param name="features">The selected features</param>
        public static void InsertFeaturesForLocation(int locationId, List<CheckBoxListItem> features)
        {
            using (var db = new BookingDataContext())
            {
                foreach (CheckBoxListItem feature in features)
                {
                    if (feature.IsChecked)
                    {
                        db.Location_has_Features.InsertOnSubmit(new Location_has_Feature()
                        {
                            LocationId = locationId,
                            FeatureId = feature.ID
                        });
                    }
                }
                db.SubmitChanges();
            }
        }

        /// <summary>
        /// Gets features for location
        /// </summary>
        /// <param name="locationId">Location ID</param>
        /// <returns>List</returns>
        public static List<string> GetFeaturesForLocation(int locationId)
        {
            using (var db = new BookingDataContext())
            {
                return (from lf in db.Location_has_Features
                        where lf.LocationId == locationId
                        select lf.Feature.Name).ToList();
            }
        }
    }
}