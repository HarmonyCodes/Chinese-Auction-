using Project.models;
using Server.BLL.Interface;
using Server.DAL.Interfaces;

namespace Server.BLL
{
    public class DonorService:IDonorService
    {
        private readonly IDonorDal _donorDal;

        public DonorService(IDonorDal donorDal)
        {
            _donorDal = donorDal;
        }
        public async Task<List<Donor>> GetAllDonors()
        {
            var donors = await _donorDal.GetAllDonors();
            return donors;
        }

        public async Task<Donor> AddDonor(Donor donor)
        {
            if (donor == null)
            {
                throw new ArgumentNullException(nameof(donor), "Donor cannot be null");
            }
            if (string.IsNullOrWhiteSpace(donor.FullName) || string.IsNullOrWhiteSpace(donor.Phone))
            {
                throw new ArgumentException("Donor's first and last and Donor's phone name cannot be empty");
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(donor.Phone, @"^05\d{8}$"))
                throw new ArgumentException("Phone number must be a valid Israeli mobile number (e.g., 05XXXXXXXX).");
            if (!string.IsNullOrWhiteSpace(donor.Email) & !System.Text.RegularExpressions.Regex.IsMatch(donor.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Invalid email format.");
            return await _donorDal.AddDonor(donor);
        }

        public async Task<Donor> UpdateDonor(Donor donor)
        {
            if (donor == null)
            {
                throw new ArgumentNullException(nameof(donor), "Donor cannot be null");
            }
            if (string.IsNullOrWhiteSpace(donor.FullName) || string.IsNullOrWhiteSpace(donor.Phone))
            {
                throw new ArgumentException("Donor's first and last and Donor's phone name cannot be empty");
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(donor.Phone, @"^05\d{8}$"))
                throw new ArgumentException("Phone number must be a valid Israeli mobile number (e.g., 05XXXXXXXX).");
            if (!string.IsNullOrWhiteSpace(donor.Email) & !System.Text.RegularExpressions.Regex.IsMatch(donor.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Invalid email format.");
            return await _donorDal.UpdateDonor(donor);
        }

        public async Task DeleteDonor(int id)
        {

            await _donorDal.DeleteDonor(id);
        }
        public async Task<List<Donor>> GetDonorsByGift(string giftName)
        {
            var donors = await _donorDal.GetDonorsByGift(giftName);
            if (!donors.Any())
                throw new KeyNotFoundException($"No donors found for gift '{giftName}'");
            return donors;
        }
    }
}
