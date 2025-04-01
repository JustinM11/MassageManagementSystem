using Microsoft.AspNetCore.Mvc;

namespace MassageManagementSystem.Controllers
{
    public class PromosController : Controller
    {
        public IActionResult Promo()
        {
            // ✅ Make sure the view path exists
            return View("~/Views/Auth/Promo.cshtml");

        }
    }
}
