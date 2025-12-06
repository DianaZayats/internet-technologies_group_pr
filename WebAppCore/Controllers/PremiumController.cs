using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlayLib.Data;
using SlayLib.Models;
using System;
using System.Security.Claims;
using WebAppCore.ViewModels;

namespace WebAppCore.Controllers
{
    /// <summary>
    /// Контролер для Premium сторінки, доступної тільки користувачам з мінімум 100 робочими годинами
    /// </summary>
    public class PremiumController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PremiumController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Відображає Premium сторінку з преміум рецептами
        /// </summary>
        [Authorize(Policy = "CanAccessPremium")]
        public async Task<IActionResult> Index(int featuredPage = 1)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            // Отримуємо метрики користувача
            var workingHoursClaim = User.FindFirst("WorkingHours");
            var workingHours = workingHoursClaim != null && int.TryParse(workingHoursClaim.Value, out var hours) ? hours : 0;

            // Створюємо модель метрик
            var metrics = new PremiumMetricsViewModel
            {
                NextReviewDate = DateTime.UtcNow.AddMonths(1),
                ProductivityScore = workingHours > 0 ? Math.Round((double)workingHours / 10.0, 2) : 0.0,
                SubscriptionFee = 29.99m
            };

            ViewData["Metrics"] = metrics;
            int pageSize = 4;
            var featuredQuery = _context.Recipes
                .Include(r => r.Author)
                .Include(r => r.Ingredients)
                .Include(r => r.Ratings)
                .Where(r => r.IsPremium && r.IsPublic)
                .OrderByDescending(r => r.CreatedAt)
                .AsQueryable();

            var featuredRecipes = await PaginateList<Recipe>.CreateAsync(featuredQuery, featuredPage, pageSize);

            // Обчислюємо середні рейтинги
            foreach (var recipe in featuredRecipes)
            {
                if (recipe.Ratings != null && recipe.Ratings.Any())
                {
                    recipe.AverageRating = recipe.Ratings.Average(r => r.Rating);
                    recipe.RatingCount = recipe.Ratings.Count;
                }
            }

            // Перевіряємо favorites для поточного користувача
            if (!string.IsNullOrEmpty(userId))
            {
                var favoriteRecipeIds = await _context.RecipeFavorites
                    .Where(f => f.UserId == userId)
                    .Select(f => f.RecipeId)
                    .ToListAsync();

                ViewData["FavoriteRecipeIds"] = favoriteRecipeIds;
            }

            return View(featuredRecipes);
        }
    }
}

