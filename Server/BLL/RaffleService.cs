using Project.BLL.Interface;
using Project.Controllers;
using Project.DAL;
using Project.models;
using Server.DAL.Interfaces;
using System.Net.Mail;
using System.Reflection;

namespace Server.BLL
{
    public class RaffleService : IRaffleService
    {
        private readonly IRaffleDal _RaffleDal;
        private readonly IEmailService _emailService;
        private readonly ILogger<RaffleService> _logger;
        public RaffleService(IRaffleDal RaffleDal, IEmailService emailService, ILogger<RaffleService> logger)
        {
            _RaffleDal = RaffleDal;
            _emailService = emailService;
            _logger = logger;
        }


        public async Task<User> DrawWinnerAsync(int giftId)
        {
            var userIds = await _RaffleDal.GetUserIdsByGiftIdAsync(giftId);
            await _RaffleDal.GiftRaffled(giftId);
            if (userIds == null || userIds.Count == 0)
                throw new InvalidOperationException("No participants found for the raffle.");

            var random = new Random();
            int winnerId = userIds[random.Next(userIds.Count)];

            var winner = await _RaffleDal.GetUserByIdAsync(winnerId);

            if (winner == null)
                throw new KeyNotFoundException($"User with ID {winnerId} not found.");

            var addWinner = new GiftWinner
            {
                GiftId = giftId,
                UserId = winner.Id,
                WinDate = DateTime.Now
            };
            await _RaffleDal.AddWinner(addWinner);


            _logger.LogInformation($"Winner drawn: {winner.UserName} (ID: {winner.Id}) for gift ID {giftId} on {DateTime.UtcNow} with {winner.Email.Trim().Trim('\'', '"')} email.");
            if (string.IsNullOrWhiteSpace(winner.Email))
                throw new InvalidOperationException("Winner email is missing.");

            var subject = $"Congratulations! You've won a gift raffle!";
            var body = $"Dear {winner.UserName},\n\n" +
                       $"Congratulations! You have won the gift raffle for the gift '{giftId}'.\n" +
                       $"Thank you for participating!\n\n" +
                       $"Best regards,\n" +
                       $"The Raffle Team";

            await _emailService.SendWinnerEmailAsync(winner.Email.Trim().Trim('\'', '"'), subject, body);

            return winner;
        }
        public async Task<byte[]> CreateFinalWinnersReportAsync()
        {
            var winnersData = await _RaffleDal.GetWinnersReportDataAsync();

            using (var stream = new MemoryStream())
            {
                ReportGenerator.CreateWinnersReport(winnersData, stream);
                return stream.ToArray();
            }
        }
        public async Task<byte[]> CreateFinalRevenueReport()
        {
            var totalRevenue = await _RaffleDal.GetRevenue();

            using (var stream = new MemoryStream())
            {
                ReportGenerator.CreateRevenueReport(totalRevenue, stream);
                return stream.ToArray();
            }
        }

        public async Task<List<GiftWinner>> GetWinners()
        {
            return await _RaffleDal.GetWinners();
        }

        public async Task<bool> AreAllGiftsRaffledAsync()
        {
            return await _RaffleDal.AreAllGiftsRaffledAsync();
        }
    }
}