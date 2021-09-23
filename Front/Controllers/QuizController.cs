using Microsoft.AspNetCore.Mvc;

namespace Proftaak.Controllers
{
    public class QuizController : ControllerBase
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}