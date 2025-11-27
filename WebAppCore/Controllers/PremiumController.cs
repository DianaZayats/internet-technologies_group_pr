using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAppCore.Controllers
{
    /// <summary>
    /// Контролер для Premium сторінки, доступної тільки користувачам з мінімум 100 робочими годинами
    /// </summary>
    public class PremiumController : Controller
    {
        /// <summary>
        /// Відображає Premium сторінку
        /// </summary>
        [Authorize(Policy = "CanAccessPremium")]
        public IActionResult Index()
        {
            // Отримуємо значення WorkingHours з твердження користувача
            var workingHoursClaim = User.FindFirst("WorkingHours");
            var workingHours = workingHoursClaim?.Value ?? "невідомо";

            ViewData["WorkingHours"] = workingHours;
            return View();
        }
    }
}

