using Project.DAL;
using Project.models;
using Server.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Server.DAL
{
    public class DonorDal: IDonorDal
    {
        private readonly AppDbContext _appDbContext;

        public DonorDal(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Donor>> GetAllDonors()
        {


            return await _appDbContext.Donors.Include(d => d.Gifts)
                .ToListAsync();
        }

        public async Task<Donor> AddDonor(Donor donor)
        {

            _appDbContext.Donors.Add(donor);
            await _appDbContext.SaveChangesAsync();
            return donor;
        }

        public async Task<Donor> UpdateDonor(Donor donor)
        {
            _appDbContext.Donors.Update(donor);
            await _appDbContext.SaveChangesAsync();
            return donor;
        }

        public async Task DeleteDonor(int id)
        {
            var donor = await _appDbContext.Donors.FindAsync(id);
            if (donor == null)
            {
                throw new KeyNotFoundException($"Donor with ID {id} not found.");
            }
            _appDbContext.Donors.Remove(donor);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<Donor>> GetDonorsByGift(string giftName)
        {
            return await _appDbContext.Donors.Include(d => d.Gifts)
                    .Where(d => d.Gifts.Any(g => g.Name.Equals(giftName)))
                    .ToListAsync();

        }
    }
}
