using Microsoft.AspNetCore.Mvc;

namespace Proftaak.Controllers
{
    public class LobbyController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}