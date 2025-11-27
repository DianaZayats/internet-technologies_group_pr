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
        private readonly SignInManager<ApplicationUser> _signInManager;

        public TestClaimsController(
            UserManager<ApplicationUser> userManager,
            ILogger<TestClaimsController> logger,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
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
                await _signInManager.RefreshSignInAsync(user);
            }

            // Додаємо нове твердження
            var newClaim = new Claim("WorkingHours", hours.ToString());
            var addResult = await _userManager.AddClaimAsync(user, newClaim);
            await _signInManager.RefreshSignInAsync(user);

            if (addResult.Succeeded)
            {
                // Оновлюємо SecurityStamp, щоб примусити користувача вийти та увійти знову
                await _userManager.UpdateSecurityStampAsync(user);
                await _signInManager.RefreshSignInAsync(user);
                TempData["SuccessMessage"] = $"WorkingHours claim оновлено на {hours}. Будь ласка, вийдіть та увійдіть знову, щоб оновити claims.";
            }
            else
            {
                TempData["ErrorMessage"] = "Помилка при додаванні claim: " + string.Join(", ", addResult.Errors.Select(e => e.Description));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AddClaim(string type, string value)
        {
            var user = await _userManager.GetUserAsync(User);

            var existing = (await _userManager.GetClaimsAsync(user))
                .FirstOrDefault(c => c.Type == type);

            if (existing == null)
            {
                await _userManager.AddClaimAsync(user, new Claim(type, value));
                TempData["SuccessMessage"] = $"Claim {type} додано!";
                await _signInManager.RefreshSignInAsync(user);
            }
            else
            {
                TempData["ErrorMessage"] = $"Claim {type} вже існує.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveClaim(string type)
        {
            var user = await _userManager.GetUserAsync(User);
            var claims = await _userManager.GetClaimsAsync(user);

            var claim = claims.FirstOrDefault(c => c.Type == type);
            if (claim != null)
            {
                await _userManager.RemoveClaimAsync(user, claim);
                await _signInManager.RefreshSignInAsync(user);
                TempData["SuccessMessage"] = $"Claim {type} видалено!";
            }
            else
            {
                TempData["ErrorMessage"] = $"Claim {type} не знайдено.";
            }

            return RedirectToAction("Index");
        }
    }
}

