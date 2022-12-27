using System.ComponentModel.DataAnnotations;

namespace NeighborGoodAPI.Models
{
    public class Good
    {
        public int Id { get; set; }
        [Required]
        public int ProfileId { get; set; }
        public string Image { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool isBorrowed { get; set; }
        public string Rating { get; set; }
        public int BorrowedCount { get; set; }
        public IFormFile File { get; set; }
    }
}
