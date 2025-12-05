using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SlayLib.Models;

namespace WebAppCore.Controllers
{
  
    public class ValidationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ValidationController> _logger;

        public ValidationController(
            UserManager<ApplicationUser> userManager,
            ILogger<ValidationController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Json("Email є обов'язковим полем");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return Json("Цей email вже зареєстрований");
            }

            return Json(true);
        }

     

       

       
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyCookingTime(int? cookingTime, string difficulty)
        {
         
            if (!cookingTime.HasValue)
            {
                return Json(true);
            }

        
            if (string.IsNullOrWhiteSpace(difficulty))
            {
                return Json(true);
            }

            var difficultyRules = new Dictionary<string, (int min, int max)>
            {
                { "Easy", (5, 30) },      
                { "Medium", (15, 60) },     
                { "Hard", (30, 180) }        
            };

            
            var normalizedDifficulty = difficulty.Trim();

            if (difficultyRules.TryGetValue(normalizedDifficulty, out var rule))
            {
                if (cookingTime.Value >= rule.min && cookingTime.Value <= rule.max)
                {
                    return Json(true); 
                }
                else if (cookingTime.Value < rule.min)
                {
                    return Json($"Для {GetDifficultyName(normalizedDifficulty)} рецепту час приготування повинен бути не менше {rule.min} хвилин");
                }
                else
                {
                    return Json($"Для {GetDifficultyName(normalizedDifficulty)} рецепту час приготування повинен бути не більше {rule.max} хвилин");
                }
            }

            return Json("Невідома складність рецепту");
        }

    
        private string GetDifficultyName(string difficulty)
        {
            return difficulty switch
            {
                "Easy" => "простого",
                "Medium" => "середнього",
                "Hard" => "складного",
                _ => difficulty
            };
        }
    }
}

