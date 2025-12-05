using System.ComponentModel.DataAnnotations;

namespace SlayLib.Models
{
    public class ShoppingListItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Quantity { get; set; }

        [StringLength(50)]
        public string? Unit { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public bool IsBought { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

