using Microsoft.AspNetCore.Mvc;

namespace MassageManagementSystem.Controllers
{
    public class TherapistController : Controller
    {
        public IActionResult Therapist()
        {
            return View("~/Views/Auth/Therapist.cshtml"); // make sure this file exists
        }
    }
}
