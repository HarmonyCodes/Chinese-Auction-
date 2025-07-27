using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Interface;
using Project.models;
using Project.models.DTOs;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : ControllerBase
    {
        private readonly IGiftService _giftService;
        private readonly ILogger<GiftController> _logger;
        private readonly IMapper _mapper;

        public GiftController(IGiftService giftService, ILogger<GiftController> logger, IMapper mapper)
        {
            _giftService = giftService;
            _logger = logger;
            _mapper = mapper;
        }

        [Authorize]

        [HttpGet]
        public async Task<ActionResult<List<Gift>>> GetGifts()
        {
            try
            {
                _logger.LogInformation("Trying to get gifts");
                var gifts = await _giftService.GetAllGifts();
                return Ok(gifts);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Not found gifts");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get gifts");
                throw;
            }
        }
        [Authorize]

        [HttpGet("sortByCategory")]
        public async Task<ActionResult<List<Gift>>> GetGiftsSortedByCategory()
        {
            try
            {
                _logger.LogInformation("Trying to get gifts sorted by category");
                var gifts = await _giftService.GetSortByCategory();
                return Ok(gifts);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogInformation(ex, "Not found gifts");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get gifts sorted by category");
                throw;
            }
        }
        [Authorize]

        [HttpGet("sortByPrice")]
        public async Task<ActionResult<List<Gift>>> GetGiftsSortByPrice()
        {
            try
            {
                _logger.LogInformation("Trying to get gifts");
                var gifts = await _giftService.GetSortByPrice();
                return Ok(gifts);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogInformation(ex, "Not found gifts");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get gifts");
                throw;
            }
        }
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<ActionResult<Gift>> AddGift([FromBody] GiftDto gift)
        {
            try
            {
                _logger.LogInformation($"Add gift: Name: {gift.Name}");
                var giftEntity = _mapper.Map<Gift>(gift);
                var addedGift = await _giftService.AddGift(giftEntity);
                return Ok(addedGift);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "AddGift failed with ArgumentException");
                return Conflict();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "AddGift failed with KeyNotFoundException");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddGift failed");
                throw;
            }


        }
        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]

        public async Task<ActionResult<Gift>> UpdateGift([FromBody] GiftDto gift, int id)
        {
            try
            {
                _logger.LogInformation($"Update gift: Id: {id}, Name: {gift.Name}");
                var giftEntity = _mapper.Map<Gift>(gift);
                giftEntity.Id = id;
                var updatedGift = await _giftService.UpdateGift(giftEntity);
                return Ok(updatedGift);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "UpdateGift failed with ArgumentException");
                return Conflict();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "UpdateGift failed with KeyNotFoundException");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateGift failed");
                throw;
            }
        }
        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGift(int id)
        {
            try
            {
                _logger.LogInformation($"Delete gift: Id: {id}");
                await _giftService.DeleteGift(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "DeleteGift failed with KeyNotFoundException");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteGift failed");
                throw;
            }
        }
        [Authorize(Roles = "Manager")]
        [HttpGet("{name}")]
        public async Task<ActionResult<List<Gift>>> GetGiftsByName(string name)
        {
            try
            {
                _logger.LogInformation("Trying to get gifts");
                var gifts = await _giftService.GetGiftsByName(name);
                return Ok(gifts);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogInformation(ex, "Not found gifts");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get gifts");
                throw;
            }
        }
        [Authorize(Roles = "Manager")]
        [HttpGet("donorName/{name}")]
        public async Task<ActionResult<List<Gift>>> GetGiftsByDonorName(string name)
        {
            try
            {
                _logger.LogInformation("Trying to get gifts");
                var gifts = await _giftService.GetGiftsByDonorName(name);
                return Ok(gifts);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogInformation(ex, "Not found gifts");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get gifts");
                throw;
            }
        }
        [Authorize(Roles = "Manager")]
        [HttpGet("buyers/{sum}")]
        public async Task<ActionResult<List<Gift>>> GetGiftsByBuyers(int sum)
        {
            try
            {
                _logger.LogInformation("Trying to get gifts");
                var gifts = await _giftService.GetGiftsByBuyers(sum);
                return Ok(gifts);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogInformation(ex, "Not found gifts");
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get gifts");
                throw;
            }
        }

    }

}
