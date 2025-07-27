using Project.models.DTOs;
using Project.models;

namespace Server.DAL.Interfaces
{
    public interface ICartDal
    {
        Task<bool> GiftIdExistsAsync(int giftId);
        Task SaveChangesAsync();
        Task<Gift> GetGiftByIdAsync(int giftId);
        Task<GiftCart> GetCartByUserIdAsync(int userId);
        Task DeleteFromCart(int id);
        Task CreateCartAsync(GiftCart cart);
        Task<IEnumerable<GiftCart>> GetAllCartsAsync();
    }
}
