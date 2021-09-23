using Microsoft.AspNetCore.Mvc;

namespace Proftaak.Controllers
{
    public class QuizController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}