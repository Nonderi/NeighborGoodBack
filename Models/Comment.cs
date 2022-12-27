using System.ComponentModel.DataAnnotations;

namespace NeighborGoodAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        [Required]
        public Profile UserProfile { get; set; }
    }
}
