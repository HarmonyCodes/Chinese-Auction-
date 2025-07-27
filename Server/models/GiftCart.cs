using System.ComponentModel.DataAnnotations.Schema;

namespace Project.models
{
    public class GiftCart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public ICollection<GiftCartItem> GiftCartItems { get; set; } = new List<GiftCartItem>();
    }

}
