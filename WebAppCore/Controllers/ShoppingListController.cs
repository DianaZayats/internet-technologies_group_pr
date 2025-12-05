using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlayLib.Data;
using SlayLib.Models;
using System.Security.Claims;

namespace WebAppCore.Controllers
{
    /// <summary>
    /// Контролер для управління списком покупок
    /// </summary>
    [Authorize]
    public class ShoppingListController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ShoppingListController> _logger;

        public ShoppingListController(
            ApplicationDbContext context,
            ILogger<ShoppingListController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: ShoppingList
        /// <summary>
        /// Відображає список покупок користувача
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var items = await _context.ShoppingListItems
                .Where(s => s.UserId == userId)
                .OrderBy(s => s.IsBought)
                .ThenBy(s => s.Name)
                .ToListAsync();

            return View(items);
        }

        // POST: ShoppingList/ToggleBought
        /// <summary>
        /// Перемикає статус "куплено" для елемента списку
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleBought(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var item = await _context.ShoppingListItems
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (item == null)
            {
                return NotFound();
            }

            item.IsBought = !item.IsBought;
            await _context.SaveChangesAsync();

            return Json(new { success = true, isBought = item.IsBought });
        }

        // POST: ShoppingList/Remove
        /// <summary>
        /// Видаляє елемент зі списку покупок
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var item = await _context.ShoppingListItems
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (item == null)
            {
                return NotFound();
            }

            _context.ShoppingListItems.Remove(item);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        // POST: ShoppingList/Clear
        /// <summary>
        /// Очищає весь список покупок
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Clear()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var items = await _context.ShoppingListItems
                .Where(s => s.UserId == userId)
                .ToListAsync();

            _context.ShoppingListItems.RemoveRange(items);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

