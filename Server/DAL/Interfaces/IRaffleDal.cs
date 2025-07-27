using System.Reflection;
using Project.models;

namespace Server.DAL.Interfaces
{
    public interface IRaffleDal
    {
        Task<List<int>> GetUserIdsByGiftIdAsync(int giftId);
        Task<User> GetUserByIdAsync(int userId);

        Task AddWinner(GiftWinner winner);
        Task<Dictionary<string, string>> GetWinnersReportDataAsync();

        Task<decimal> GetRevenue();

        Task<List<GiftWinner>> GetWinners();
        Task<bool> AreAllGiftsRaffledAsync();

        Task GiftRaffled(int giftId);
    }
}
