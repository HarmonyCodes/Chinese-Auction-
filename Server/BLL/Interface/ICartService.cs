using Project.models;
using Project.models.DTOs;

namespace Project.BLL.Interface
{
    public interface ICartService
    {
        Task<GiftCartItem> AddToCartAsync(int userId, GiftCartItemDto item);
        Task DeleteFromCart(int id);
        Task<GiftCart> GetCartByUserIdAsync(int userId);
        Task<IEnumerable<GiftCart>> GetAllCartsAsync();
    }

}
