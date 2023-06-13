using Microsoft.AspNetCore.Mvc;

namespace TheFruityMixologist.Controllers
{
    public class ErrorController : Controller
    {

        public IActionResult Error()
        {
            return View();
        }
    }
}
