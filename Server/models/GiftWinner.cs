namespace Project.models
{
    public class GiftWinner
    {
        public int Id { get; set; }
        public int GiftId { get; set; }
        public Gift Gift { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime WinDate { get; set; }
    }

}
