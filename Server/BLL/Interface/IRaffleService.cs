using Project.models.DTOs;
using Project.models;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Project.BLL.Interface
{
    public interface IRaffleService
    {
        Task<User> DrawWinnerAsync(int giftId);
        Task<byte[]> CreateFinalWinnersReportAsync();
        Task<byte[]> CreateFinalRevenueReport();
        Task<List<GiftWinner>> GetWinners();
        Task<bool> AreAllGiftsRaffledAsync();
    }

}
