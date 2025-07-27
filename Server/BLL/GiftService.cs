using Project.BLL.Interface;
using Project.models;
using Server.DAL.Interfaces;

namespace Project.BLL
{
    public class GiftService : IGiftService
    {
        private readonly IGiftDal _giftDal;
        public GiftService(IGiftDal giftDal)
        {
            _giftDal = giftDal;
        }
        public async Task<List<Gift>> GetAllGifts()
        {
            return await _giftDal.GetAllGifts();
        }

        public async Task<Gift> AddGift(Gift gift)
        {
            if (gift == null)
            {
                throw new ArgumentNullException(nameof(gift), "Gift cannot be null");
            }
            if (string.IsNullOrWhiteSpace(gift.Name) || gift.Price <= 0)
            {
                throw new ArgumentException("Gift's name cannot be empty and price must be greater than zero");
            }
            if (!await _giftDal.DonorExistsAsync(gift.DonorId))
            {
                throw new KeyNotFoundException("Donor not found");
            }
            if (!await _giftDal.CategoryExistsAsync(gift.CategoryId))
            {
                throw new KeyNotFoundException("Category not found");
            }

            return await _giftDal.AddGift(gift);
        }

        public async Task<Gift> UpdateGift(Gift gift)
        {
            if (gift == null)
            {
                throw new ArgumentNullException(nameof(gift), "Gift cannot be null");
            }
            if (string.IsNullOrWhiteSpace(gift.Name) || gift.Price <= 0)
            {
                throw new ArgumentException("Gift's name cannot be empty and price must be greater than zero");
            }
            if (!await _giftDal.DonorExistsAsync(gift.DonorId))
            {
                throw new KeyNotFoundException("Donor not found");
            }
            if (!await _giftDal.CategoryExistsAsync(gift.CategoryId))
            {
                throw new KeyNotFoundException("Category not found");
            }
            return await _giftDal.UpdateGift(gift);
        }

        public async Task DeleteGift(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("Id cannot be negative");
            }
            await _giftDal.DeleteGift(id);
        }
        public async Task<List<Gift>> GetGiftsByName(string name)
        {

            return await _giftDal.GetGiftsByName(name);

        }
        public async Task<List<Gift>> GetGiftsByDonorName(string name)
        {
            return await _giftDal.GetGiftsByDonorName(name);
        }

        public async Task<List<Gift>> GetGiftsByBuyers(int sum)
        {
            return await _giftDal.GetGiftsByBuyers(sum);
        }

        public async Task<List<Gift>> GetSortByCategory()
        {
            return await _giftDal.GetSortByCategory();
        }

        public async Task<List<Gift>> GetSortByPrice()
        {
            return await _giftDal.GetSortByPrice();
        }
    }

}
