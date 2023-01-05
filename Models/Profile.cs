using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeighborGoodAPI.Models
{
    public class Profile
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Auth0Id { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        //public Address Address { get; set; }
        public ICollection<Item> Items { get; set; } = new List<Item>();
        //public double? Rating { get; set; }
        //public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        //[Precision(8, 6)]
        //public decimal Longitude { get; set; }
        //[Precision(8, 6)]
        //public decimal Latitude { get; set; }
    }
}
