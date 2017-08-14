using System.Web;
using System.Web.Mvc;

namespace Locs4Youth.Utils
{
    public class Locs4YouthAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated) return false;
            return new Locs4YouthRoleProvider().IsUserInRole(httpContext.User.Identity.Name, Roles);
        }
    }
}