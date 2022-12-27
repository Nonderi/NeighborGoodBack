using System.ComponentModel.DataAnnotations;

namespace NeighborGoodAPI.Models
{
    public class Profile
    {
        [Required]
        public int Id { get; set; }
        //public int Id { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public ICollection<Good> Goods { get; set; }
        public ICollection<Good> BorrowedGoods { get; set; }
        public double Rating { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
    }
}
