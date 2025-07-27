
namespace Project.models
{
    public class GiftCartItem
    {
        public int Id { get; set; }
        public int GiftCartId { get; set; }
        public GiftCart GiftCart { get; set; }
        public int GiftId { get; set; }
        public Gift Gift { get; set; }
        public int Quantity { get; set; } = 1;
        /// <summary>
        /// האם הפריט בטיוטה (כל עוד לא אושר/שולם)
        /// </summary>
        public bool IsDraft { get; set; } = true;
    }
}
