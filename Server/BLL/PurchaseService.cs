using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.BLL.Interface;
using Project.DAL;
using Project.models;
using Server.DAL.Interfaces;

namespace Project.BLL
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseDal purchaseDal;
        private readonly IMapper _mapper;

        public PurchaseService(IPurchaseDal purchaseDal, IMapper mapper)
        {
            this.purchaseDal = purchaseDal;
            _mapper = mapper;
        }

        public async Task<Purchase> ConvertCartToPurchase(int Id)
        {
            var cart = await purchaseDal.GetCartByUserIdAsync(Id);

            if (cart == null || !cart.GiftCartItems.Any())
                throw new Exception("The cart is empty");

            // כל פריטי העגלה הופכים לסופיים (לא טיוטה)
            foreach (var item in cart.GiftCartItems)
            {
                item.IsDraft = false;
            }
            await purchaseDal.SaveChangesAsync();

            var purchase = _mapper.Map<Purchase>(cart);
            purchase.PurchaseGifts = cart.GiftCartItems.Select(i =>
            {
                return _mapper.Map<PurchaseGift>(i);
            }).ToList();

            var newPurchase = await purchaseDal.AddPurchase(purchase);

            foreach (var item in purchase.PurchaseGifts.ToList())
            {
                item.PurchaseId = newPurchase.Id;
                item.Id = 0;
                await purchaseDal.AddPurchaseItem(item);
                var gift = await purchaseDal.GetGiftById(item.GiftId);

                if (gift != null)
                {
                    if (gift.PurchaseGifts == null)
                    {
                        gift.PurchaseGifts = new List<PurchaseGift>();
                    }

                    gift.PurchaseGifts.Add(item);
                }

            }

            await purchaseDal.DeleteCart(cart);

            return purchase;
        }
        public async Task<List<Purchase>> GetAllPurchases()
        {
            var purchases = await purchaseDal.GetAllPurchases();

            return purchases;
        }
        public async Task<List<PurchaseGift>> GetPurchaseItemByGiftId(int id)
        {
            var purchaseItems = await purchaseDal.GetPurchaseItemByGiftId(id);

            return purchaseItems;
        }

        public async Task<List<Gift>> SortByPrice()
        {
            var gifts = await purchaseDal.SortByPrice();
            return gifts;
        }
        public async Task<List<Gift>> SortByGiftName()
        {
            var gifts = await purchaseDal.SortByGiftName();
            return gifts;
        }
        public async Task<List<Gift>> SortByPopular()
        {
            var gifts = await purchaseDal.SortByPopular();
            return gifts;
        }
    }

}
