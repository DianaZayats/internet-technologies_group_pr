using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using WebAppCore.Resources;

namespace WebAppCore.ViewModels
{
    public class RecipeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "RecipeName_Required")]
        [StringLength(200, MinimumLength = 3, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "RecipeName_StringLength")]
        [Display(Name = "RecipeName", ResourceType = typeof(ValidationMessages))]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "Description_Required")]
        [StringLength(2000, MinimumLength = 10, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "Description_StringLength")]
        [Display(Name = "Description", ResourceType = typeof(ValidationMessages))]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "Category_Required")]
        [Display(Name = "Category", ResourceType = typeof(ValidationMessages))]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "Difficulty_Required")]
        [Display(Name = "Difficulty", ResourceType = typeof(ValidationMessages))]
        public string Difficulty { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "CookingTime_Required")]
        [Range(1, 600, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "CookingTime_Range")]
        [Display(Name = "PreparationTime")]
        public int PreparationTime { get; set; }

        [Range(1, 20)]
        [Display(Name = "Servings", ResourceType = typeof(ValidationMessages))]
        public int Servings { get; set; } = 4;

        [StringLength(500)]
        [Display(Name = "ImageUrl", ResourceType = typeof(ValidationMessages))]
        public string? ImageUrl { get; set; }

        [StringLength(50)]
        [Display(Name = "DietType", ResourceType = typeof(ValidationMessages))]
        public string? DietType { get; set; }

        [StringLength(50)]
        [Display(Name = "Cuisine", ResourceType = typeof(ValidationMessages))]
        public string? Cuisine { get; set; }

        [StringLength(20)]
        [Display(Name = "BudgetLevel", ResourceType = typeof(ValidationMessages))]
        public string? BudgetLevel { get; set; }

        [Range(0, 5000)]
        [Display(Name = "Calories", ResourceType = typeof(ValidationMessages))]
        public int? Calories { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "Ingredients_Required")]
        [StringLength(2000, MinimumLength = 10, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = "Ingredients_StringLength")]
        [Display(Name = "Ingredients", ResourceType = typeof(ValidationMessages))]
        public string Ingredients { get; set; } = string.Empty;
    }
}
