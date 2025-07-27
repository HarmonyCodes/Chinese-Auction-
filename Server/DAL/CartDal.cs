using Microsoft.EntityFrameworkCore;
using Project.DAL;
using Project.models;
using Server.DAL.Interfaces;

namespace Server.DAL
{
    public class CartDal: ICartDal{
        public async Task<IEnumerable<GiftCart>> GetAllCartsAsync()
        {
            return await _context.GiftCarts
                .Include(gc => gc.GiftCartItems)
                .ThenInclude(i => i.Gift)
                .ToListAsync();
        }
    
        private readonly AppDbContext _context;
        public CartDal(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> GiftIdExistsAsync(int giftId)
        {
            return await _context.Gifts.AnyAsync(gift=>gift.Id == giftId);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<Gift> GetGiftByIdAsync(int giftId)
        {
            return await _context.Gifts.FirstOrDefaultAsync(g => g.Id == giftId);
        }
        public async Task<GiftCart> GetCartByUserIdAsync(int userId)
        {
            return await _context.GiftCarts.Include(gc => gc.GiftCartItems).ThenInclude(i => i.Gift).FirstOrDefaultAsync(c => c.UserId == userId);
        }
        public async Task DeleteFromCart(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id must be positive integer", nameof(id));
            var cartItem = await _context.GiftCartItems.FindAsync(id);
            if (cartItem == null)
            {
                throw new KeyNotFoundException($"Cart item with ID {id} not found.");
            }
            _context.GiftCartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
        public async Task CreateCartAsync(GiftCart cart)
        {
            _context.GiftCarts.Add(cart);
            await _context.SaveChangesAsync();
        }

    }
}
