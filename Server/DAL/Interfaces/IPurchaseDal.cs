using Microsoft.AspNetCore.Mvc;
using Project.models;

namespace Server.DAL.Interfaces
{
    public interface IPurchaseDal
    {
        Task<GiftCart> GetCartByUserIdAsync(int userId);
        Task<Purchase> AddPurchase(Purchase purchase);

        Task DeleteCart(GiftCart cart);

        Task<List<Purchase>> GetAllPurchases();

        Task<Gift> GetGiftById(int id);

        Task<List<PurchaseGift>> GetPurchaseItemByGiftId(int id);

        Task<List<Gift>> SortByPrice();

        Task<List<Gift>> SortByGiftName();

        Task<List<Gift>> SortByPopular();

        Task AddPurchaseItem(PurchaseGift purchaseItem);

        Task SaveChangesAsync();
    }
}
