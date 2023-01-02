using System.ComponentModel.DataAnnotations;

namespace NeighborGoodAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; } = null!;
        [Required]
        public Profile UserProfile { get; set; } = null!;
    }
}
