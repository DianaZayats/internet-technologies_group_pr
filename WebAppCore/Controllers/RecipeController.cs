using Microsoft.AspNetCore.Mvc;
using WebAppCore.ViewModels;

namespace WebAppCore.Controllers
{
    
    public class RecipeController : Controller
    {
        private readonly ILogger<RecipeController> _logger;

        public RecipeController(ILogger<RecipeController> logger)
        {
            _logger = logger;
        }

     
        public IActionResult Create()
        {
            return View(new RecipeViewModel());
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RecipeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Форма додавання рецепту містить помилки валідації");
                return View(model);
            }

           
            _logger.LogInformation("Рецепт '{RecipeName}' успішно створено (Складність: {Difficulty}, Час: {CookingTime} хв)", 
                model.Name, model.Difficulty, model.CookingTime);

         
            return RedirectToAction(nameof(Success));
        }

       
        public IActionResult Success()
        {
            return View();
        }
    }
}

