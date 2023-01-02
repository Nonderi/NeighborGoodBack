using System.ComponentModel.DataAnnotations;

namespace NeighborGoodAPI.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Required]
        public Profile UserProfile { get; set; } = null!;
        public string? ImageName { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Category { get; set; }
        public bool isBorrowed { get; set; }
        public decimal? Rating { get; set; }
        public int BorrowedCount { get; set; }
        public IFormFile? File { get; set; }
    }
}
