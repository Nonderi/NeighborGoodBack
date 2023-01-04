namespace NeighborGoodAPI.Models
{
    public class Address
    {
        public int Id { get; set; }

        public string? Street { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public int ProfileId { get; set; }
        public Profile User { get; set; }
    }
}
