namespace Project.models
{
    public class Purchase
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<PurchaseGift> PurchaseGifts { get; set; } = new List<PurchaseGift>();
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
    }

}
