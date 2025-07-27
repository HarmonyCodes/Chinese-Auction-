using Server.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Interface;
using Project.models;
using System.Reflection;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftRaffleController : ControllerBase
    {
        private readonly IRaffleService _giftRaffleService;
        private readonly ILogger<GiftRaffleController> _logger;
        public GiftRaffleController(IRaffleService giftRaffleService, ILogger<GiftRaffleController> logger)
        {
            _giftRaffleService = giftRaffleService;
            _logger = logger;
        }
        [Authorize(Roles = "Manager")]
        [HttpPost("raffle/{giftId}")]
        public async Task<ActionResult<User>> RaffleGift(int giftId)
        {
            try
            {
                _logger.LogInformation($"Raffling gift with Id: {giftId}");
                var winner = await _giftRaffleService.DrawWinnerAsync(giftId);
                return Ok(winner);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "RaffleGift failed with KeyNotFoundException");
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "RaffleGift failed with InvalidOperationException");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to raffle gift");
                throw;
            }
        }
        [Authorize(Roles = "Manager")]
        [HttpGet("download-winners-report")]
        public async Task<IActionResult> DownloadWinnersReport()
        {
            try
            {
                _logger.LogInformation("Try to get winners report");
                var fileBytes = await _giftRaffleService.CreateFinalWinnersReportAsync();
                return File(fileBytes, "application/pdf", "WinnersReport.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to download winners report");
                throw;
            }
        }
        [Authorize(Roles = "Manager")]
        [HttpGet("download-totalRevenue")]
        public async Task<IActionResult> DownloadRevenueReport()
        {
            try
            {
                _logger.LogInformation("Try to get revenue report");
                var fileBytes = await _giftRaffleService.CreateFinalRevenueReport();
                return File(fileBytes, "application/pdf", "RevenueReport.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to download revenue report");
                throw;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<GiftWinner>>> GetWinners()
        {
            try
            {
                _logger.LogInformation("Try to get winners");
                var winners = await _giftRaffleService.GetWinners();
                return Ok(winners);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to download revenue report");
                throw;
            }
        }
        [Authorize]
        [HttpGet("status")]
        public async Task<ActionResult<bool>> AreAllGiftsRaffled()
        {
            try
            {
                _logger.LogInformation("Checking if all gifts are raffled");
                var allRaffled = await _giftRaffleService.AreAllGiftsRaffledAsync();
                return Ok(allRaffled);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check if all gifts are raffled");
                throw;
            }
        }

    }

}
