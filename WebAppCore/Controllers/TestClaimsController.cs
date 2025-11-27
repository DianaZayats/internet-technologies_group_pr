using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SlayLib.Models;
using System.Security.Claims;

namespace WebAppCore.Controllers
{
    /// <summary>
    /// Контролер для тестування зміни claims (тільки для розробки)
    /// </summary>
    [Authorize]
    public class TestClaimsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<TestClaimsController> _logger;

        public TestClaimsController(
            UserManager<ApplicationUser> userManager,
            ILogger<TestClaimsController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Відображає поточні claims користувача
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Користувач не знайдений");
            }

            var claims = await _userManager.GetClaimsAsync(user);
            ViewBag.Claims = claims;
            ViewBag.UserEmail = user.Email;
            return View();
        }

        /// <summary>
        /// Змінює WorkingHours claim для поточного користувача
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateWorkingHours([FromForm] int hours)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Користувач не знайдений");
            }

            // Видаляємо існуюче твердження, якщо воно є
            var existingClaim = (await _userManager.GetClaimsAsync(user))
                .FirstOrDefault(c => c.Type == "WorkingHours");

            if (existingClaim != null)
            {
                var removeResult = await _userManager.RemoveClaimAsync(user, existingClaim);
                if (!removeResult.Succeeded)
                {
                    TempData["ErrorMessage"] = "Помилка при видаленні старого claim";
                    return RedirectToAction(nameof(Index));
                }
            }

            // Додаємо нове твердження
            var newClaim = new Claim("WorkingHours", hours.ToString());
            var addResult = await _userManager.AddClaimAsync(user, newClaim);

            if (addResult.Succeeded)
            {
                // Оновлюємо SecurityStamp, щоб примусити користувача вийти та увійти знову
                await _userManager.UpdateSecurityStampAsync(user);
                TempData["SuccessMessage"] = $"WorkingHours claim оновлено на {hours}. Будь ласка, вийдіть та увійдіть знову, щоб оновити claims.";
            }
            else
            {
                TempData["ErrorMessage"] = "Помилка при додаванні claim: " + string.Join(", ", addResult.Errors.Select(e => e.Description));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

