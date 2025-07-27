namespace Project.models
{
    public class Donor
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public ICollection<Gift> Gifts { get; set; }
    }
}
