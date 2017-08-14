using Locs4Youth.Models;
using System.Web.Mvc;

namespace Locs4Youth.Controllers
{
    /// <summary>
    /// This controller handles everything related to displaying pages in home
    /// </summary>
    [AllowAnonymous]
    public class HomeController : Controller
    {
        #region GET
        // GET: /Home/Index
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Home/FAQ
        public ActionResult FAQ()
        {
            return View();
        }

        // GET: /Home/Contact
        public ActionResult Contact()
        {
            return View();
        }
        #endregion
    }
}