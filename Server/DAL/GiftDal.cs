using Project.DAL;
using Project.models;
using Server.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Server.DAL
{
    public class GiftDal : IGiftDal
    {
        private readonly AppDbContext _appDbContext;

        public GiftDal(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Gift>> GetAllGifts()
        {
            return await _appDbContext.Gifts.Include(gift => gift.Donor).
                                                Include(gift => gift.Category).
                                                       ToListAsync();
        }
        public async Task<List<Gift>> GetSortByCategory()
        {
            return await _appDbContext.Gifts
                .Include(g => g.Category)
                .OrderBy(g => g.Category.Name)
               .ToListAsync();
        }
        public async Task<List<Gift>> GetSortByPrice()
        {
            return await _appDbContext.Gifts
                .OrderBy(g => g.Price)
               .ToListAsync();
        }

        public async Task<Gift> AddGift(Gift gift)
        {
            _appDbContext.Gifts.Add(gift);
            await _appDbContext.SaveChangesAsync();
            return gift;
        }
        public async Task<bool> DonorExistsAsync(int donorId)
        {
            return await _appDbContext.Donors.AnyAsync(d => d.Id == donorId);
        }
        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            // return await _appDbContext.Categories.AnyAsync(c => c.Id == categoryId);
            return false;
        }
        public async Task<Gift> UpdateGift(Gift gift)
        {
            _appDbContext.Gifts.Update(gift);
            await _appDbContext.SaveChangesAsync();
            return gift;
        }

        public async Task DeleteGift(int id)
        {
            var gift = await _appDbContext.Gifts.Include(g => g.PurchaseGifts).FirstOrDefaultAsync(g => g.Id == id);
            if (gift == null)
            {
                throw new KeyNotFoundException($"Gift with ID {id} not found.");
            }
            if (gift.PurchaseGifts != null && gift.PurchaseGifts.Any())
            {
                throw new InvalidOperationException("Cannot delete a gift that has purchases.");
            }
            _appDbContext.Gifts.Remove(gift);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<Gift>> GetGiftsByName(string name)
        {
            return await _appDbContext.Gifts.Where(g => g.Name.Contains(name)).ToListAsync();
        }

        public async Task<List<Gift>> GetGiftsByDonorName(string name)
        {
            return await _appDbContext.Gifts
           .Where(g => (g.Donor.FullName) == name)
           .ToListAsync();

        }
        public async Task<List<Gift>> GetGiftsByBuyers(int sum)
        {
            return await _appDbContext.Gifts
                .Where(g => g.PurchaseGifts.Sum(p => p.Quantity) >= sum)
                .ToListAsync();
        }
    }
}
