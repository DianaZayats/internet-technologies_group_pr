using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using WebAppCore.Resources;

namespace WebAppCore.ViewModels
{
   
    public class RecipeViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "RecipeName_Required")]
        [StringLength(200, MinimumLength = 3, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "RecipeName_StringLength")]
        [Display(Name = "Назва рецепту")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "Description_Required")]
        [StringLength(2000, MinimumLength = 10, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "Description_StringLength")]
        [Display(Name = "Опис рецепту")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "Category_Required")]
        [Display(Name = "Категорія")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "Difficulty_Required")]
        [Display(Name = "Складність")]
        public string Difficulty { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "CookingTime_Required")]
        [Range(1, 300, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "CookingTime_Range")]
        [Remote(
            action: "VerifyCookingTime", 
            controller: "Validation", 
            AdditionalFields = "Difficulty",
            ErrorMessageResourceType = typeof(ValidationMessages), 
            ErrorMessageResourceName = "CookingTime_Remote")]
        [Display(Name = "Час приготування (хвилин)")]
        public int? CookingTime { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "Ingredients_Required")]
        [StringLength(2000, MinimumLength = 10, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "Ingredients_StringLength")]
        [Display(Name = "Інгредієнти")]
        public string Ingredients { get; set; } = string.Empty;
    }
}

