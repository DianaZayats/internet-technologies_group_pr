using System.ComponentModel.DataAnnotations;

namespace SlayLib.Models
{
    public class RecipeFavorite
    {
        public int Id { get; set; }

        [Required]
        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
