using System.ComponentModel.DataAnnotations;

namespace SlayLib.Models
{
    public class RecipeComment
    {
        public int Id { get; set; }

        [Required]
        [StringLength(2000)]
        public string Content { get; set; } = string.Empty;

        [Required]
        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }

        [Required]
        public string AuthorId { get; set; } = string.Empty;
        public ApplicationUser? Author { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}

