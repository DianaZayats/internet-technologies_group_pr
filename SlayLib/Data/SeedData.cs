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
                },
                // Premium recipes
                new Recipe
                {
                    Name = "Лобстер термідор",
                    Description = "Розкішна страва з лобстера, запеченого під соусом термідор. Ексклюзивний рецепт для особливих випадків.",
                    Category = "Dinner",
                    Difficulty = "Hard",
                    PreparationTime = 60,
                    Servings = 2,
                    DietType = null,
                    Cuisine = "French",
                    BudgetLevel = "Expensive",
                    Calories = 520,
                    ImageUrl = "https://images.unsplash.com/photo-1559339352-11d035aa65de?w=800",
                    AuthorId = defaultUser.Id,
                    IsPublic = true,
                    IsPremium = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Лобстер", Quantity = "2", Unit = "шт" },
                        new Ingredient { Name = "Вершкове масло", Quantity = "100", Unit = "г" },
                        new Ingredient { Name = "Біле вино", Quantity = "100", Unit = "мл" },
                        new Ingredient { Name = "Пармезан", Quantity = "50", Unit = "г" },
                        new Ingredient { Name = "Часник", Quantity = "2", Unit = "зубчики" },
                        new Ingredient { Name = "Петрушка", Quantity = "30", Unit = "г" }
                    }
                },
                new Recipe
                {
                    Name = "Ваґю стейк з трюфелями",
                    Description = "Премиум стейк з японської яловичини ваґю, приготований до ідеальної ніжності, з трюфельним соусом.",
                    Category = "Dinner",
                    Difficulty = "Hard",
                    PreparationTime = 45,
                    Servings = 2,
                    DietType = null,
                    Cuisine = "Asian",
                    BudgetLevel = "Expensive",
                    Calories = 680,
                    ImageUrl = "https://images.unsplash.com/photo-1546833999-b9f581a1996d?w=800",
                    AuthorId = defaultUser.Id,
                    IsPublic = true,
                    IsPremium = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-12),
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Стейк ваґю", Quantity = "400", Unit = "г" },
                        new Ingredient { Name = "Трюфель", Quantity = "20", Unit = "г" },
                        new Ingredient { Name = "Вершкове масло", Quantity = "50", Unit = "г" },
                        new Ingredient { Name = "Червоне вино", Quantity = "150", Unit = "мл" },
                        new Ingredient { Name = "Розмарин", Quantity = "2", Unit = "гілки" }
                    }
                },
                new Recipe
                {
                    Name = "Фуа-гра з інжиром",
                    Description = "Вишукана французька страва з фуа-гра, подана з карамелізованим інжиром та бальзамічним соусом.",
                    Category = "Dinner",
                    Difficulty = "Hard",
                    PreparationTime = 40,
                    Servings = 4,
                    DietType = null,
                    Cuisine = "French",
                    BudgetLevel = "Expensive",
                    Calories = 450,
                    ImageUrl = "https://images.unsplash.com/photo-1574781330855-d0db8cc4a0d2?w=800",
                    AuthorId = defaultUser.Id,
                    IsPublic = true,
                    IsPremium = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Фуа-гра", Quantity = "200", Unit = "г" },
                        new Ingredient { Name = "Інжир", Quantity = "4", Unit = "шт" },
                        new Ingredient { Name = "Бальзамічний соус", Quantity = "50", Unit = "мл" },
                        new Ingredient { Name = "Мед", Quantity = "2", Unit = "ст.л." },
                        new Ingredient { Name = "Рукола", Quantity = "50", Unit = "г" }
                    }
                },
                new Recipe
                {
                    Name = "Чорна ікра з блинами",
                    Description = "Розкішна закуска з чорної ікри на домашніх блинах зі сметаною. Ексклюзивна страва.",
                    Category = "Breakfast",
                    Difficulty = "Medium",
                    PreparationTime = 30,
                    Servings = 4,
                    DietType = null,
                    Cuisine = null,
                    BudgetLevel = "Expensive",
                    Calories = 320,
                    ImageUrl = "https://images.unsplash.com/photo-1526318896980-cf78c088247c?w=800",
                    AuthorId = defaultUser.Id,
                    IsPublic = true,
                    IsPremium = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-8),
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Чорна ікра", Quantity = "100", Unit = "г" },
                        new Ingredient { Name = "Борошно", Quantity = "150", Unit = "г" },
                        new Ingredient { Name = "Яйця", Quantity = "2", Unit = "шт" },
                        new Ingredient { Name = "Молоко", Quantity = "200", Unit = "мл" },
                        new Ingredient { Name = "Сметана", Quantity = "100", Unit = "г" },
                        new Ingredient { Name = "Цибуля зелена", Quantity = "30", Unit = "г" }
                    }
                },
                new Recipe
                {
                    Name = "Трюфельна паста",
                    Description = "Італійська паста з трюфельним соусом та пармезаном. Неймовірно ароматна та насичена страва.",
                    Category = "Lunch",
                    Difficulty = "Medium",
                    PreparationTime = 25,
                    Servings = 4,
                    DietType = null,
                    Cuisine = "Italian",
                    BudgetLevel = "Expensive",
                    Calories = 480,
                    ImageUrl = "https://images.unsplash.com/photo-1621996346565-e3dbc646d9a9?w=800",
                    AuthorId = defaultUser.Id,
                    IsPublic = true,
                    IsPremium = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Паста тальятеле", Quantity = "400", Unit = "г" },
                        new Ingredient { Name = "Трюфель", Quantity = "30", Unit = "г" },
                        new Ingredient { Name = "Вершкове масло", Quantity = "100", Unit = "г" },
                        new Ingredient { Name = "Пармезан", Quantity = "100", Unit = "г" },
                        new Ingredient { Name = "Вершки", Quantity = "200", Unit = "мл" },
                        new Ingredient { Name = "Часник", Quantity = "2", Unit = "зубчики" }
                    }
                },
                new Recipe
                {
                    Name = "Шоколадний суфле з золотом",
                    Description = "Вишуканий десерт - ніжне шоколадне суфле, прикрашене листками їстівного золота. Ідеальний фінал для особливої вечері.",
                    Category = "Dessert",
                    Difficulty = "Hard",
                    PreparationTime = 35,
                    Servings = 6,
                    DietType = null,
                    Cuisine = "French",
                    BudgetLevel = "Expensive",
                    Calories = 420,
                    ImageUrl = "https://images.unsplash.com/photo-1578985545062-69928b1d9587?w=800",
                    AuthorId = defaultUser.Id,
                    IsPublic = true,
                    IsPremium = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Темний шоколад", Quantity = "200", Unit = "г" },
                        new Ingredient { Name = "Яйця", Quantity = "6", Unit = "шт" },
                        new Ingredient { Name = "Цукор", Quantity = "100", Unit = "г" },
                        new Ingredient { Name = "Вершкове масло", Quantity = "50", Unit = "г" },
                        new Ingredient { Name = "Їстівне золото", Quantity = "1", Unit = "г" },
                        new Ingredient { Name = "Ваніль", Quantity = "1", Unit = "ч.л." }
                    }
                }
            };

            context.Recipes.AddRange(recipes);
            await context.SaveChangesAsync();
        }
    }
}

