using Project.models;
using Project.models.DTOs;

namespace Project.BLL.Interface
{
    public interface IGiftService
    {
        Task<List<Gift>> GetAllGifts();
        Task<Gift> AddGift(Gift gift);
        Task<Gift> UpdateGift(Gift gift);
        Task DeleteGift(int id);
        Task<List<Gift>> GetGiftsByName(string name);
        Task<List<Gift>> GetGiftsByDonorName(string name);
        Task<List<Gift>> GetGiftsByBuyers(int sum);
        Task<List<Gift>> GetSortByCategory();
        Task<List<Gift>> GetSortByPrice();
    }

}
