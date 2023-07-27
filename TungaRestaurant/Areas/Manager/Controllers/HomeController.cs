using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TungaRestaurant.Areas.Manager.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin,Branch Manager")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
