using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Locs4Youth.Models
{
    public partial class Activation
    {
        /// <summary>
        /// Determines if there is an activation code and deletes it if so
        /// </summary>
        /// <param name="code">Activation code</param>
        /// <param name="userId">The user's ID</param>
        /// <returns>The activation</returns>
        public static bool HasActivation(Guid code, int userId)
        {
            using (var db = new BookingDataContext())
            {
                Activation activation = (from a in db.Activations
                                         where code == a.Code && a.UserId == userId
                                         select a).FirstOrDefault();
                if (activation == null)
                {
                    return false;
                }
                else
                {
                    db.Activations.DeleteOnSubmit(activation);
                    db.SubmitChanges();
                    return true;
                }
            }
        }
    }
}