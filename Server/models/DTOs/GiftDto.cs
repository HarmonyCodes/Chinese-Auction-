namespace Project.models.DTOs
{
    public class GiftDto
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int DonorId { get; set; }
        public int GiftWinnerId { get; set; }
    }

}
