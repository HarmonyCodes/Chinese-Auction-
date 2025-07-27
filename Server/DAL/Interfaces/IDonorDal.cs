using Project.models;

namespace Server.DAL.Interfaces
{
    public interface IDonorDal
    {
        Task<List<Donor>> GetAllDonors();

        Task<Donor> AddDonor(Donor donor);

        Task<Donor> UpdateDonor(Donor donor);

        Task DeleteDonor(int id);

        Task<List<Donor>> GetDonorsByGift(string giftName);
    }
}
