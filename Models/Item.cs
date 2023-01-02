using System.ComponentModel.DataAnnotations;

namespace NeighborGoodAPI.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Required]
        public Profile Owner { get; set; } = null!;
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Category { get; set; }
        public bool isBorrowed { get; set; }
        public double? Rating { get; set; }
        public int BorrowedCount { get; set; }
        public string? ImageName { get; set; }
        public IFormFile? ImageFile { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
