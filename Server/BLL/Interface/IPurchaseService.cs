using Project.models;

namespace Project.BLL.Interface
{
    public interface IPurchaseService
    {
        Task<Purchase> ConvertCartToPurchase(int Id);
        Task<List<Purchase>> GetAllPurchases();
        Task<List<PurchaseGift>> GetPurchaseItemByGiftId(int id);
        Task<List<Gift>> SortByPrice();
        Task<List<Gift>> SortByGiftName();
        Task<List<Gift>> SortByPopular();
    }

}
