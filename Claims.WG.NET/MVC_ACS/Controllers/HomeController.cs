using System.IdentityModel.Services;
using System.Web.Mvc;

namespace MVC_ACS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignOut()
        {
            FederatedAuthentication.SessionAuthenticationModule.SignOut();

            return RedirectToAction("Index");
        }
    }
}
