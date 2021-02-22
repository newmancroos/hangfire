using Microsoft.AspNetCore.Mvc;

namespace WithServer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
