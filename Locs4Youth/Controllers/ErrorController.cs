using System.Web.Mvc;

namespace Locs4Youth.Controllers
{
    /// <summary>
    /// This controller handles everything related to displaying errors
    /// </summary>
    public class ErrorController : Controller
    {
        #region GET
        // GET: /Error/Index
        public ViewResult Index()
        {
            Response.TrySkipIisCustomErrors = true;
            return View("Error");
        }

        // GET: /Error/NotFound
        public ViewResult NotFound()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            return View("NotFound");
        }
        #endregion
    }
}