using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlayLib.Data;
using SlayLib.Models;
using System.Security.Claims;
using WebAppCore.ViewModels;

namespace WebAppCore.Controllers
{
    /// <summary>
    /// Контролер для управління рецептами з ресурсною авторизацією
    /// </summary>
    public class RecipeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RecipeController> _logger;

        public RecipeController(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager,
            ILogger<RecipeController> logger)
        {
            _context = context;
            _authorizationService = authorizationService;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Recipe
        /// <summary>
        /// Відображає список рецептів з фільтрацією та пошуком
        /// </summary>
        [AllowAnonymous]
        public async Task<IActionResult> Index(
            string? category,
            string? difficulty,
            string? dietType,
            string? cuisine,
            string? budgetLevel,
            int? maxTime,
            int? minTime,
            int? maxCalories,
            int? minCalories,
            string? search,
            string? hasIngredients,
            string? excludeIngredients)
        {
            var query = _context.Recipes
                .Include(r => r.Author)
                .Include(r => r.Ingredients)
                .Include(r => r.Ratings)
                .Where(r => r.IsPublic) // Тільки публічні рецепти
                .AsQueryable();

            // Фільтр за категорією
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(r => r.Category == category);
            }

            // Фільтр за складністю
            if (!string.IsNullOrEmpty(difficulty))
            {
                query = query.Where(r => r.Difficulty == difficulty);
            }

            // Фільтр за типом дієти
            if (!string.IsNullOrEmpty(dietType))
            {
                query = query.Where(r => r.DietType == dietType);
            }

            // Фільтр за кухнею
            if (!string.IsNullOrEmpty(cuisine))
            {
                query = query.Where(r => r.Cuisine == cuisine);
            }

            // Фільтр за бюджетом
            if (!string.IsNullOrEmpty(budgetLevel))
            {
                query = query.Where(r => r.BudgetLevel == budgetLevel);
            }

            // Фільтр за часом приготування
            if (minTime.HasValue)
            {
                query = query.Where(r => r.PreparationTime >= minTime.Value);
            }
            if (maxTime.HasValue)
            {
                query = query.Where(r => r.PreparationTime <= maxTime.Value);
            }

            // Фільтр за калоріями
            if (minCalories.HasValue)
            {
                query = query.Where(r => r.Calories.HasValue && r.Calories >= minCalories.Value);
            }
            if (maxCalories.HasValue)
            {
                query = query.Where(r => r.Calories.HasValue && r.Calories <= maxCalories.Value);
            }

            // Пошук за назвою або описом
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(r =>
                    r.Name.Contains(search) ||
                    r.Description.Contains(search));
            }

            // Фільтр за наявними інгредієнтами
            if (!string.IsNullOrEmpty(hasIngredients))
            {
                var ingredientNames = hasIngredients.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(i => i.ToLower())
                    .ToList();
                query = query.Where(r => r.Ingredients != null && r.Ingredients.Any(i =>
                    ingredientNames.Contains(i.Name.ToLower())));
            }

            // Фільтр за виключенням інгредієнтів
            if (!string.IsNullOrEmpty(excludeIngredients))
            {
                var excludeNames = excludeIngredients.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(i => i.ToLower())
                    .ToList();
                query = query.Where(r => r.Ingredients == null || !r.Ingredients.Any(i =>
                    excludeNames.Contains(i.Name.ToLower())));
            }

            var recipes = await query
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            // Обчислюємо середні рейтинги
            foreach (var recipe in recipes)
            {
                if (recipe.Ratings != null && recipe.Ratings.Any())
                {
                    ViewData[$"Rating_{recipe.Id}"] = recipe.Ratings.Average(r => r.Rating);
                    ViewData[$"RatingCount_{recipe.Id}"] = recipe.Ratings.Count;
                }
            }

            // Перевіряємо favorites для поточного користувача
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                var favoriteRecipeIds = await _context.RecipeFavorites
                    .Where(f => f.UserId == userId)
                    .Select(f => f.RecipeId)
                    .ToListAsync();

                ViewData["FavoriteRecipeIds"] = favoriteRecipeIds;
            }

            // Передаємо параметри фільтрів у ViewBag
            ViewBag.Category = category;
            ViewBag.Difficulty = difficulty;
            ViewBag.DietType = dietType;
            ViewBag.Cuisine = cuisine;
            ViewBag.BudgetLevel = budgetLevel;
            ViewBag.MinTime = minTime;
            ViewBag.MaxTime = maxTime;
            ViewBag.MinCalories = minCalories;
            ViewBag.MaxCalories = maxCalories;
            ViewBag.Search = search;
            ViewBag.HasIngredients = hasIngredients;
            ViewBag.ExcludeIngredients = excludeIngredients;

            return View(recipes);
        }

        // GET: Recipe/Details/5
        /// <summary>
        /// Відображає деталі рецепту
        /// </summary>
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Author)
                .Include(r => r.Ingredients)
                .Include(r => r.Comments)
                    .ThenInclude(c => c.Author)
                .Include(r => r.Ratings)
                    .ThenInclude(rt => rt.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null)
            {
                return NotFound();
            }

            // Обчислюємо середній рейтинг
            if (recipe.Ratings != null && recipe.Ratings.Any())
            {
                ViewData["AverageRating"] = recipe.Ratings.Average(r => r.Rating);
                ViewData["RatingCount"] = recipe.Ratings.Count;
            }

            // Перевіряємо, чи користувач додав рецепт у favorites
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                var isFavorite = await _context.RecipeFavorites
                    .AnyAsync(f => f.RecipeId == recipe.Id && f.UserId == userId);
                ViewData["IsFavorite"] = isFavorite;

                // Перевіряємо рейтинг користувача
                var userRating = await _context.RecipeRatings
                    .FirstOrDefaultAsync(r => r.RecipeId == recipe.Id && r.UserId == userId);
                ViewData["UserRating"] = userRating?.Rating;
            }

            // Знаходимо схожі рецепти
            var similarRecipes = await _context.Recipes
                .Include(r => r.Author)
                .Include(r => r.Ratings)
                .Where(r => r.Id != recipe.Id && r.IsPublic &&
                    (r.Category == recipe.Category ||
                     r.Cuisine == recipe.Cuisine ||
                     r.Difficulty == recipe.Difficulty))
                .OrderByDescending(r => r.CreatedAt)
                .Take(6)
                .ToListAsync();

            ViewData["SimilarRecipes"] = similarRecipes;

            return View(recipe);
        }

        // GET: Recipe/Create
        /// <summary>
        /// Відображає форму створення нового рецепту
        /// </summary>
        public IActionResult Create()
        {
            return View(new RecipeViewModel());
        }

        // POST: Recipe/Create
        /// <summary>
        /// Обробляє створення нового рецепту
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecipeViewModel model)
        {
            ModelState.Remove(nameof(RecipeViewModel.Id));

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Форма додавання рецепту містить помилки валідації");
                return View(model);
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("Спроба створити рецепт без автентифікованого користувача");
                    return Unauthorized();
                }

                var recipe = new Recipe
                {
                    Name = model.Name,
                    Description = model.Description,
                    Category = model.Category,
                    Difficulty = model.Difficulty,
                    PreparationTime = model.PreparationTime,
                    Servings = model.Servings,
                    ImageUrl = model.ImageUrl,
                    DietType = model.DietType,
                    Cuisine = model.Cuisine,
                    BudgetLevel = model.BudgetLevel,
                    Calories = model.Calories,
                    AuthorId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Recipes.Add(recipe);
                await _context.SaveChangesAsync();

                // Парсимо інгредієнти
                var ingredients = ParseIngredients(model.Ingredients);
                foreach (var ingredient in ingredients)
                {
                    ingredient.RecipeId = recipe.Id;
                    _context.Ingredients.Add(ingredient);
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation("Рецепт '{RecipeName}' успішно створено з ID: {RecipeId}", recipe.Name, recipe.Id);
                return RedirectToAction(nameof(Details), new { id = recipe.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при створенні рецепту");
                ModelState.AddModelError("", "Сталася помилка при створенні рецепту. Спробуйте ще раз.");
                return View(model);
            }
        }

        // GET: Recipe/Edit/5
        /// <summary>
        /// Відображає форму редагування рецепту (тільки для автора)
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null)
            {
                return NotFound();
            }

            // Перевіряємо авторизацію
            var authorizationResult = await _authorizationService.AuthorizeAsync(
                User, recipe, "CanEditResource");

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            // Конвертуємо Recipe в RecipeViewModel
            var model = new RecipeViewModel
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                Category = recipe.Category,
                Difficulty = recipe.Difficulty,
                PreparationTime = recipe.PreparationTime,
                Servings = recipe.Servings,
                ImageUrl = recipe.ImageUrl,
                DietType = recipe.DietType,
                Cuisine = recipe.Cuisine,
                BudgetLevel = recipe.BudgetLevel,
                Calories = recipe.Calories,
                Ingredients = string.Join("\n", recipe.Ingredients?.Select(i =>
                    string.IsNullOrEmpty(i.Quantity)
                        ? i.Name
                        : $"{i.Quantity} {i.Unit ?? ""} {i.Name}".Trim()) ?? new List<string>())
            };

            return View(model);
        }

        // POST: Recipe/Edit/5
        /// <summary>
        /// Обробляє оновлення рецепту (тільки для автора)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RecipeViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null)
            {
                return NotFound();
            }

            // Перевіряємо авторизацію
            var authorizationResult = await _authorizationService.AuthorizeAsync(
                User, recipe, "CanEditResource");

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Оновлюємо рецепт
                recipe.Name = model.Name;
                recipe.Description = model.Description;
                recipe.Category = model.Category;
                recipe.Difficulty = model.Difficulty;
                recipe.PreparationTime = model.PreparationTime;
                recipe.Servings = model.Servings;
                recipe.ImageUrl = model.ImageUrl;
                recipe.DietType = model.DietType;
                recipe.Cuisine = model.Cuisine;
                recipe.BudgetLevel = model.BudgetLevel;
                recipe.Calories = model.Calories;
                recipe.UpdatedAt = DateTime.UtcNow;

                // Видаляємо старі інгредієнти
                if (recipe.Ingredients != null)
                {
                    _context.Ingredients.RemoveRange(recipe.Ingredients);
                }

                // Додаємо нові інгредієнти
                var ingredients = ParseIngredients(model.Ingredients);
                foreach (var ingredient in ingredients)
                {
                    ingredient.RecipeId = recipe.Id;
                    _context.Ingredients.Add(ingredient);
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { id = recipe.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при редагуванні рецепту");
                ModelState.AddModelError("", "Сталася помилка при редагуванні рецепту. Спробуйте ще раз.");
                return View(model);
            }
        }

        // POST: Recipe/Delete/5
        /// <summary>
        /// Видаляє рецепт (тільки для автора)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            // Перевіряємо авторизацію
            var authorizationResult = await _authorizationService.AuthorizeAsync(
                User, recipe, "CanEditResource");

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Recipe/ToggleFavorite
        /// <summary>
        /// Додає або видаляє рецепт з favorites
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleFavorite(int recipeId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var existingFavorite = await _context.RecipeFavorites
                .FirstOrDefaultAsync(f => f.RecipeId == recipeId && f.UserId == userId);

            if (existingFavorite != null)
            {
                _context.RecipeFavorites.Remove(existingFavorite);
            }
            else
            {
                _context.RecipeFavorites.Add(new RecipeFavorite
                {
                    RecipeId = recipeId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, isFavorite = existingFavorite == null });
        }

        // POST: Recipe/Rate
        /// <summary>
        /// Додає або оновлює рейтинг рецепту
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rate(int recipeId, int rating)
        {
            if (rating < 1 || rating > 5)
            {
                return BadRequest("Rating must be between 1 and 5");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var existingRating = await _context.RecipeRatings
                .FirstOrDefaultAsync(r => r.RecipeId == recipeId && r.UserId == userId);

            if (existingRating != null)
            {
                existingRating.Rating = rating;
                existingRating.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                _context.RecipeRatings.Add(new RecipeRating
                {
                    RecipeId = recipeId,
                    UserId = userId,
                    Rating = rating,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();

            // Обчислюємо новий середній рейтинг
            var averageRating = await _context.RecipeRatings
                .Where(r => r.RecipeId == recipeId)
                .AverageAsync(r => (double?)r.Rating) ?? 0;

            var ratingCount = await _context.RecipeRatings
                .CountAsync(r => r.RecipeId == recipeId);

            return Json(new { success = true, averageRating, ratingCount });
        }

        // POST: Recipe/AddComment
        /// <summary>
        /// Додає коментар до рецепту
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int recipeId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return BadRequest("Comment content is required");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var comment = new RecipeComment
            {
                RecipeId = recipeId,
                AuthorId = userId,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            _context.RecipeComments.Add(comment);
            await _context.SaveChangesAsync();

            // Завантажуємо коментар з автором для відображення
            var commentWithAuthor = await _context.RecipeComments
                .Include(c => c.Author)
                .FirstOrDefaultAsync(c => c.Id == comment.Id);

            return PartialView("_CommentPartial", commentWithAuthor);
        }

        // GET: Recipe/Favorites
        /// <summary>
        /// Відображає список улюблених рецептів користувача
        /// </summary>
        public async Task<IActionResult> Favorites()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var favoriteRecipeIds = await _context.RecipeFavorites
                .Where(f => f.UserId == userId)
                .Select(f => f.RecipeId)
                .ToListAsync();

            var recipes = await _context.Recipes
                .Include(r => r.Author)
                .Include(r => r.Ingredients)
                .Include(r => r.Ratings)
                .Where(r => favoriteRecipeIds.Contains(r.Id))
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return View(recipes);
        }

        // POST: Recipe/AddToShoppingList
        /// <summary>
        /// Додає інгредієнти рецепту до списку покупок
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToShoppingList(int recipeId, int servings = 0)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.Id == recipeId);

            if (recipe == null)
            {
                return NotFound();
            }

            var multiplier = servings > 0 ? (double)servings / recipe.Servings : 1.0;

            foreach (var ingredient in recipe.Ingredients ?? new List<Ingredient>())
            {
                // Перевіряємо, чи вже є такий інгредієнт у списку
                var existingItem = await _context.ShoppingListItems
                    .FirstOrDefaultAsync(s => s.UserId == userId &&
                        s.Name.ToLower() == ingredient.Name.ToLower() &&
                        s.Unit == ingredient.Unit &&
                        !s.IsBought);

                if (existingItem != null)
                {
                    // Об'єднуємо кількості
                    if (double.TryParse(existingItem.Quantity, out var existingQty) &&
                        double.TryParse(ingredient.Quantity, out var newQty))
                    {
                        var totalQty = existingQty + (newQty * multiplier);
                        existingItem.Quantity = totalQty.ToString("F2");
                    }
                }
                else
                {
                    // Додаємо новий елемент
                    var quantity = ingredient.Quantity;
                    if (multiplier != 1.0 && double.TryParse(ingredient.Quantity, out var qty))
                    {
                        quantity = (qty * multiplier).ToString("F2");
                    }

                    _context.ShoppingListItems.Add(new ShoppingListItem
                    {
                        UserId = userId,
                        Name = ingredient.Name,
                        Quantity = quantity,
                        Unit = ingredient.Unit,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Ingredients added to shopping list" });
        }

        /// <summary>
        /// Парсить рядок інгредієнтів у список об'єктів Ingredient
        /// </summary>
        private List<Ingredient> ParseIngredients(string ingredientsText)
        {
            var ingredients = new List<Ingredient>();

            if (string.IsNullOrWhiteSpace(ingredientsText))
                return ingredients;

            var lines = ingredientsText.Split(new[] { ',', '\n', '\r' },
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var ingredient = new Ingredient
                {
                    Name = line.Trim()
                };

                // Спроба розпарсити кількість та одиницю
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2 && double.TryParse(parts[0], out _))
                {
                    ingredient.Quantity = parts[0];
                    ingredient.Name = string.Join(" ", parts.Skip(1));

                    var commonUnits = new[] { "g", "kg", "ml", "l", "pcs", "cups", "tsp", "tbsp", "ст.л.", "ч.л.", "г", "кг", "мл", "л", "шт" };
                    if (parts.Length >= 3 && commonUnits.Contains(parts[1].ToLower()))
                    {
                        ingredient.Unit = parts[1];
                        ingredient.Name = string.Join(" ", parts.Skip(2));
                    }
                }

                ingredients.Add(ingredient);
            }

            return ingredients;
        }
    }
}
