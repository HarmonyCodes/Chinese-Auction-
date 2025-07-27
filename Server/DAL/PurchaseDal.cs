using Microsoft.EntityFrameworkCore;
using Project.DAL;
using Project.models;
using Server.DAL.Interfaces;
using System.Linq;

namespace Server.DAL
{
    public class PurchaseDal : IPurchaseDal
    {
        private readonly AppDbContext appDbContext;

        public PurchaseDal(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<GiftCart> GetCartByUserIdAsync(int userId)
        {
            return await appDbContext.GiftCarts
                .Include(c => c.GiftCartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Gift> GetGiftById(int id)
        {
            return await appDbContext.Gifts
                .Include(g => g.PurchaseGifts)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Purchase> AddPurchase(Purchase purchase)
        {
            appDbContext.Purchases.Add(purchase);
            await appDbContext.SaveChangesAsync();
            return purchase;
        }

        public async Task DeleteCart(GiftCart cart)
        {
            appDbContext.GiftCarts.Remove(cart);
            await appDbContext.SaveChangesAsync();
        }
        public async Task<List<Purchase>> GetAllPurchases()
        {
            return await appDbContext.Purchases
                .Include(p => p.User).ToListAsync();

        }
        public async Task<List<PurchaseGift>> GetPurchaseItemByGiftId(int id)
        {

            var PurchaseItem = await appDbContext.PurchaseGifts
                   .Where(p => p.GiftId == id)
                   .Include(pi => pi.Purchase)
                   .ToListAsync();

            if (PurchaseItem == null)
                throw new KeyNotFoundException($"Gift with id {id} not found");

            return PurchaseItem.ToList();
        }
        public async Task<List<Gift>> SortByPrice()
        {
            return await appDbContext.PurchaseGifts

              .Select(pi => pi.Gift)
              .Distinct()
              .OrderByDescending(g => g.Price)
             .ToListAsync();
        }
        public async Task<List<Gift>> SortByPopular()
        {
            return await appDbContext.Gifts
           .OrderByDescending(g => g.PurchaseGifts.Sum(pi => pi.Quantity))
           .ToListAsync();
        }
        public async Task<List<Gift>> SortByGiftName()
        {
            return await appDbContext.PurchaseGifts
                .Include(pi => pi.GiftId)
                .Select(pi => pi.Gift)
                .Distinct()
                .OrderBy(pi => pi.Name)
                .ToListAsync();
        }
        public async Task AddPurchaseItem(PurchaseGift purchaseItem)
        {
            appDbContext.PurchaseGifts.Add(purchaseItem);
            await appDbContext.SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await appDbContext.SaveChangesAsync();
        }
    }
}