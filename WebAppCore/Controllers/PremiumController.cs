using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAppCore.ViewModels;

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

            var metrics = new PremiumMetricsViewModel
            {
                NextReviewDate = DateTime.UtcNow.AddDays(14),
                ProductivityScore = 87.45,
                SubscriptionFee = 249.99m
            };

            ViewData["WorkingHours"] = workingHours;
            return View(metrics);
        }
    }
}

