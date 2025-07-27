using Microsoft.EntityFrameworkCore;
using Project.DAL;
using Server.DAL.Interfaces;
using System;
using System.Reflection;
using Project.models;


namespace Server.DAL
{
    public class RaffleDal:IRaffleDal
    {
        private readonly AppDbContext _context;

        public RaffleDal(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<int>> GetUserIdsByGiftIdAsync(int giftId)
        {
            return await _context.PurchaseGifts
                .Where(pi => pi.GiftId == giftId)
                .Select(pi => pi.Purchase.UserId)
                .Distinct()
                .ToListAsync();
        }

        public async Task GiftRaffled(int giftId)
        {
            var gift = await _context.Gifts.FindAsync(giftId);
            if (gift == null)
                throw new KeyNotFoundException("Gift not found");

            gift.Raffled = true;

            await _context.SaveChangesAsync();

        }

        public async Task<bool> AreAllGiftsRaffledAsync()
        {
            return await _context.Gifts.AllAsync(g => g.Raffled);
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }
        public async Task AddWinner(GiftWinner winner)
        {
            _context.GiftWinners.Add(winner);
            await _context.SaveChangesAsync();


        }
        public async Task<Dictionary<string, string>> GetWinnersReportDataAsync()
        {
            return await _context.GiftWinners
                .Include(w => w.User)
                .Include(w => w.Gift)
                .GroupBy(w => w.Gift.Name)
                .Select(g => new { GiftName = g.Key, WinnerName = g.First().User.UserName })
                .ToDictionaryAsync(x => x.GiftName, x => x.WinnerName);
        }

        public async Task<decimal> GetRevenue()
        {
            return (decimal)await _context.Gifts
            .SelectMany(g => g.PurchaseGifts
            .Select(pi => g.Price * pi.Quantity))
            .SumAsync();
        }
        public async Task<List<GiftWinner>> GetWinners()
        {
            return await _context.GiftWinners
                .Include(w => w.User)
                .Include(w => w.Gift)
                .ToListAsync();
        }

    }
}
