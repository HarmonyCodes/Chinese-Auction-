using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Project.models;

namespace Server.BLL.Interface
{
    public interface IDonorService
    {
        Task<List<Donor>> GetAllDonors();

        Task<Donor> AddDonor(Donor donor);

        Task<Donor> UpdateDonor(Donor donor);

        Task DeleteDonor(int id);
        Task<List<Donor>> GetDonorsByGift(string giftName);
    }
}
