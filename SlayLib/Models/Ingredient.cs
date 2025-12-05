using System.ComponentModel.DataAnnotations;

namespace SlayLib.Models
{
    public class Ingredient
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Quantity { get; set; } // e.g., "2", "1/2"

        [StringLength(50)]
        public string? Unit { get; set; } // e.g., "g", "ml", "pcs", "cups", "tsp"

        [Required]
        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }
    }
}

