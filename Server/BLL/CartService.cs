using Project.BLL.Interface;
using Project.DAL;
using Project.models;
using Microsoft.EntityFrameworkCore;
using Project.models.DTOs;
using Server.DAL.Interfaces;
using AutoMapper;

namespace Project.BLL
{
    public class CartService : ICartService
    {
        public async Task<IEnumerable<GiftCart>> GetAllCartsAsync()
        {
            // נניח של-ICartDal יש GetAllCartsAsync, אחרת נממש ישירות מה-DbContext
            if (_cartDal is null)
                throw new InvalidOperationException("Cart DAL is not initialized");
            return await _cartDal.GetAllCartsAsync();
        }
        private readonly ICartDal _cartDal;
        private readonly IMapper _mapper;

        public CartService(ICartDal cartDal, IMapper mapper)
        {
            _cartDal = cartDal;
            _mapper = mapper;
        }

        public async Task<GiftCartItem> AddToCartAsync(int userId, GiftCartItemDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "GiftCart cannot be null");
            }
            if (!await _cartDal.GiftIdExistsAsync(item.GiftId))
            {
                throw new ArgumentException("Gift Id not found");
            }
            // מניעת הוספה לעגלה אם המתנה כבר הוגרלה
            var gift = await _cartDal.GetGiftByIdAsync(item.GiftId);
            if (gift != null && gift.Raffled)
            {
                throw new InvalidOperationException("לא ניתן להוסיף לעגלה מתנה שכבר הוגרלה");
            }
            if (item.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be a positive integer");
            }

            var cart = await _cartDal.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new GiftCart
                {
                    UserId = userId,
                    GiftCartItems = new List<GiftCartItem>()
                };
                await _cartDal.CreateCartAsync(cart);
            }
            var existingItem = cart.GiftCartItems.FirstOrDefault(i => i.GiftId == item.GiftId && i.IsDraft);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
                await _cartDal.SaveChangesAsync();
                return existingItem;
            }
            else
            {
                var newItem = new GiftCartItem
                {
                    GiftId = item.GiftId,
                    Quantity = item.Quantity,
                    IsDraft = true // תמיד טיוטה בהוספה
                };
                cart.GiftCartItems.Add(newItem);
                await _cartDal.SaveChangesAsync();
                return newItem;
            }
        }

        public async Task DeleteFromCart(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be positive integer", nameof(id));
            }
            await _cartDal.DeleteFromCart(id);
        }

        public async Task<GiftCart> GetCartByUserIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be a positive integer", nameof(userId));
            }
            var cart = await _cartDal.GetCartByUserIdAsync(userId);
            // אם אין עגלה, החזר עגלה ריקה (או null) במקום לזרוק שגיאה
            return cart;
        }
    }

}
