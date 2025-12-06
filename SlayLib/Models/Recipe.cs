using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlayLib.Models
{
    /// <summary>
    /// Модель рецепту, який може бути відредагований тільки автором
    /// </summary>
    public class Recipe
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty; // Breakfast, Lunch, Dinner, Dessert

        [Required]
        [StringLength(20)]
        public string Difficulty { get; set; } = string.Empty; // Easy, Medium, Hard

        [Required]
        [Range(1, 600)]
        public int PreparationTime { get; set; } // in minutes

        [Range(1, 20)]
        public int Servings { get; set; } = 4; // base number of portions

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [StringLength(50)]
        public string? DietType { get; set; } // vegan, keto, gluten-free, dairy-free, etc.

        [StringLength(50)]
        public string? Cuisine { get; set; } // Italian, Asian, Ukrainian, etc.

        [StringLength(20)]
        public string? BudgetLevel { get; set; } // Cheap, Medium, Expensive

        [Range(0, 5000)]
        public int? Calories { get; set; }

        public bool IsPremium { get; set; } = false;

        public bool IsPublic { get; set; } = true;

        /// <summary>
        /// Ідентифікатор автора рецепту (збігається з AspNetUsers.Id)
        /// </summary>
        [Required]
        public string AuthorId { get; set; } = string.Empty;
        public ApplicationUser? Author { get; set; }

        public List<Ingredient>? Ingredients { get; set; }
        public List<RecipeComment>? Comments { get; set; }
        public List<RecipeRating>? Ratings { get; set; }
        public List<RecipeFavorite>? Favorites { get; set; }

        [NotMapped]
        public double? AverageRating { get; set; }

        [NotMapped]
        public int? RatingCount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}

