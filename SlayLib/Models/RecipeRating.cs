using System.ComponentModel.DataAnnotations;

namespace SlayLib.Models
{
    public class RecipeRating
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } // 1-5 stars

        [Required]
        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
