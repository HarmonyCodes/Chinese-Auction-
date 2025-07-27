using Server.models;

namespace Project.models
{
    public class Gift
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public double Price { get; set; }
        public int DonorId { get; set; }
        public Donor Donor { get; set; }
        public int GiftWinnerId { get; set; }
        public GiftWinner GiftWinner { get; set; }
        public bool Raffled { get; set; } = false;
        public ICollection<PurchaseGift> PurchaseGifts { get; set; }
        //public ICollection<GiftCartItem>? GiftCartItems { get; set; }
    }
}
