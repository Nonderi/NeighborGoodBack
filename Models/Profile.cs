﻿using Microsoft.EntityFrameworkCore;
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
        public string Address { get; set; } = null!;
        [Required]
        public string PhoneNumber { get; set; } = null!;
        public ICollection<Item> Items { get; set; } = new List<Item>();
        public double? Rating { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        [Precision(8, 6)]
        public decimal Longitude { get; set; }
        [Precision(8, 6)]
        public decimal Latitude { get; set; }
    }
}
