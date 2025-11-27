using System.ComponentModel.DataAnnotations;

namespace SlayLib.Models
{
    /// <summary>
    /// Модель поста, який може бути відредагований тільки автором
    /// </summary>
    public class Post
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Ідентифікатор автора поста (збігається з AspNetUsers.Id)
        /// </summary>
        [Required]
        public string AuthorId { get; set; } = string.Empty;
        public ApplicationUser? Author { get; set; }

        public List<Comment>? Comments { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}

