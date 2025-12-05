using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SlayLib.Models;

namespace SlayLib.Data
{
    /// <summary>
    /// Клас для ініціалізації початкових даних в базі даних
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// Заповнює базу даних початковими рецептами
        /// </summary>
        public static async Task SeedRecipesAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            // Seed recipes if database is empty
            if (await context.Recipes.AnyAsync())
            {
                return; // Database already has recipes
            }

            var defaultUser = await userManager.FindByEmailAsync("admin@example.com");
            if (defaultUser == null)
            {
                // Create a default user for seeding
                defaultUser = new ApplicationUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    FirstName = "Admin",
                    LastName = "User"
                };
                var result = await userManager.CreateAsync(defaultUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddClaimAsync(defaultUser, new System.Security.Claims.Claim("IsVerifiedClient", "true"));
                    await userManager.AddClaimAsync(defaultUser, new System.Security.Claims.Claim("WorkingHours", "150"));
                }
            }

            var recipes = new List<Recipe>
            {
                new Recipe
                {
                    Name = "Борщ український",
                    Description = "Класичний український борщ з буряком, капустою та сметаною. Традиційна страва, яка завжди радує смаком та ароматом.",
                    Category = "Dinner",
                    Difficulty = "Medium",
                    PreparationTime = 90,
                    Servings = 6,
                    DietType = null,
                    Cuisine = "Ukrainian",
                    BudgetLevel = "Cheap",
                    Calories = 250,
                    ImageUrl = "https://images.unsplash.com/photo-1547592166-23ac45744acd?w=800",
                    AuthorId = defaultUser.Id,
                    IsPublic = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Буряк", Quantity = "300", Unit = "г" },
                        new Ingredient { Name = "Капуста", Quantity = "200", Unit = "г" },
                        new Ingredient { Name = "Картопля", Quantity = "3", Unit = "шт" },
                        new Ingredient { Name = "Морква", Quantity = "1", Unit = "шт" },
                        new Ingredient { Name = "Цибуля", Quantity = "1", Unit = "шт" },
                        new Ingredient { Name = "Сметана", Quantity = "200", Unit = "г" }
                    }
                },
                new Recipe
                {
                    Name = "Омлет з овочами",
                    Description = "Смачний та поживний сніданок з яєць та свіжих овочів. Ідеально підходить для ранку.",
                    Category = "Breakfast",
                    Difficulty = "Easy",
                    PreparationTime = 15,
                    Servings = 2,
                    DietType = null,
                    Cuisine = null,
                    BudgetLevel = "Cheap",
                    Calories = 180,
                    ImageUrl = "https://images.unsplash.com/photo-1615367423057-4b3c1f1e5c8a?w=800",
                    AuthorId = defaultUser.Id,
                    IsPublic = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-8),
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Яйця", Quantity = "4", Unit = "шт" },
                        new Ingredient { Name = "Помідори", Quantity = "2", Unit = "шт" },
                        new Ingredient { Name = "Перець болгарський", Quantity = "1", Unit = "шт" },
                        new Ingredient { Name = "Цибуля зелена", Quantity = "50", Unit = "г" },
                        new Ingredient { Name = "Олія", Quantity = "2", Unit = "ст.л." }
                    }
                },
                new Recipe
                {
                    Name = "Паста карбонара",
                    Description = "Італійська класика з беконом, яйцями та пармезаном. Ніжний та насичений смак.",
                    Category = "Lunch",
                    Difficulty = "Medium",
                    PreparationTime = 25,
                    Servings = 4,
                    DietType = null,
                    Cuisine = "Italian",
                    BudgetLevel = "Medium",
                    Calories = 450,
                    ImageUrl = "https://images.unsplash.com/photo-1621996346565-e3dbc646d9a9?w=800",
                    AuthorId = defaultUser.Id,
                    IsPublic = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-6),
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Спагеті", Quantity = "400", Unit = "г" },
                        new Ingredient { Name = "Бекон", Quantity = "200", Unit = "г" },
                        new Ingredient { Name = "Яйця", Quantity = "4", Unit = "шт" },
                        new Ingredient { Name = "Пармезан", Quantity = "100", Unit = "г" },
                        new Ingredient { Name = "Часник", Quantity = "2", Unit = "зубчики" }
                    }
                },
                new Recipe
                {
                    Name = "Шоколадний торт",
                    Description = "Ніжний шоколадний торт з кремом. Ідеальний десерт для особливих випадків.",
                    Category = "Dessert",
                    Difficulty = "Hard",
                    PreparationTime = 120,
                    Servings = 8,
                    DietType = null,
                    Cuisine = null,
                    BudgetLevel = "Expensive",
                    Calories = 380,
                    ImageUrl = "https://images.unsplash.com/photo-1578985545062-69928b1d9587?w=800",
                    AuthorId = defaultUser.Id,
                    IsPublic = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Борошно", Quantity = "200", Unit = "г" },
                        new Ingredient { Name = "Цукор", Quantity = "200", Unit = "г" },
                        new Ingredient { Name = "Какао", Quantity = "50", Unit = "г" },
                        new Ingredient { Name = "Яйця", Quantity = "4", Unit = "шт" },
                        new Ingredient { Name = "Вершкове масло", Quantity = "150", Unit = "г" },
                        new Ingredient { Name = "Шоколад", Quantity = "200", Unit = "г" }
                    }
                },
                new Recipe
                {
                    Name = "Салат Цезар",
                    Description = "Класичний салат з куркою, сухариками та соусом Цезар. Свіжий та ситний.",
                    Category = "Lunch",
                    Difficulty = "Easy",
                    PreparationTime = 20,
                    Servings = 4,
                    DietType = null,
                    Cuisine = null,
                    BudgetLevel = "Medium",
                    Calories = 320,
                    ImageUrl = "https://images.unsplash.com/photo-1546793665-c74683f339c1?w=800",
                    AuthorId = defaultUser.Id,
                    IsPublic = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Салат Романо", Quantity = "200", Unit = "г" },
                        new Ingredient { Name = "Куряче філе", Quantity = "300", Unit = "г" },
                        new Ingredient { Name = "Сухарики", Quantity = "100", Unit = "г" },
                        new Ingredient { Name = "Пармезан", Quantity = "50", Unit = "г" },
                        new Ingredient { Name = "Соус Цезар", Quantity = "150", Unit = "мл" }
                    }
                }
            };

            context.Recipes.AddRange(recipes);
            await context.SaveChangesAsync();
        }
    }
}

