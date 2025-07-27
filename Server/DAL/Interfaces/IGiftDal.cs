using Project.models;

namespace Server.DAL.Interfaces
{
    public interface IGiftDal
    {
        Task<List<Gift>> GetAllGifts();
        Task<Gift> AddGift(Gift gift);
        Task<Gift> UpdateGift(Gift gift);
        Task DeleteGift(int id);
        Task<bool> DonorExistsAsync(int donorId);
        Task<bool> CategoryExistsAsync(int categoryId);
        Task<List<Gift>> GetGiftsByName(string name);
        Task<List<Gift>> GetGiftsByDonorName(string name);
        Task<List<Gift>> GetGiftsByBuyers(int sum);
        Task<List<Gift>> GetSortByCategory();
        Task<List<Gift>> GetSortByPrice();
    }
}
