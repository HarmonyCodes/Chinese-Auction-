using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DAL;
using Project.models;
using Project.models.DTOs;
using Server.BLL.Interface;

namespace Project.Controllers
{
    namespace Project.WebApi.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]

        public class DonorController : ControllerBase

        {
            private readonly IDonorService _donorService;
            private readonly ILogger<DonorController> _logger;
            private readonly IMapper _mapper;
            public DonorController(IDonorService donorService, ILogger<DonorController> logger, IMapper mapper)
            {
                _donorService = donorService;
                _logger = logger;
                _mapper = mapper;
            }


            [Authorize(Roles = "Manager")]
            [HttpGet]
            public async Task<ActionResult<List<Donor>>> GetDonors()
            {
                try
                {
                    _logger.LogInformation("Trying to get donors");
                    var donors = await _donorService.GetAllDonors();
                    return Ok(donors);
                }
                catch (KeyNotFoundException ex)
                {
                    _logger.LogInformation(ex, "Not found donors");
                    return Ok();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to get donors");
                    throw;
                }
            }
            [Authorize(Roles = "Manager")]
            [HttpPost]
            public async Task<ActionResult<Donor>> AddDonor([FromBody] DonorDto donor)
            {
                try
                {
                    _logger.LogInformation($"Add donor: Name: {donor.FullName}");
                    var addDonor = _mapper.Map<Donor>(donor);
                    var addedDonor = await _donorService.AddDonor(addDonor);
                    return Ok(addedDonor);
                }
                catch (ArgumentException ex)
                {
                    _logger.LogError(ex, "AddDonor failed with ArgumentException");
                    return Conflict();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "AddDonor failed");
                    throw;
                }
            }
            [Authorize(Roles = "Manager")]
            [HttpPut("{id}")]
            public async Task<ActionResult<Donor>> UpdateDonor(int id, [FromBody] DonorDto donor)
            {
                id = id < 0 ? throw new ArgumentException("Id cannot be negative") : id;
                try
                {
                    _logger.LogInformation($"Update donor: Id: {id}, Name: {donor.FullName}");
                    var updateDonor = _mapper.Map<Donor>(donor);
                    updateDonor.Id = id;
                    var updatedDonor = await _donorService.UpdateDonor(updateDonor);
                    return Ok(updatedDonor);
                }
                catch (KeyNotFoundException ex)
                {
                    _logger.LogError(ex, "UpdateDonor failed with KeyNotFoundException");
                    return NotFound();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "UpdateDonor failed");
                    throw;
                }
            }

            [Authorize(Roles = "Manager")]
            [HttpDelete("{id}")]
            public async Task<ActionResult> DeleteDonor(int id)
            {
                id = id < 0 ? throw new ArgumentException("Id cannot be negative") : id;

                try
                {
                    _logger.LogInformation($"Delete donor with Id: {id}");
                    // Assuming you have a method in your service to delete a donor
                    await _donorService.DeleteDonor(id);
                    return Ok($"Delete donor {id} succesfuly");
                }
                catch (KeyNotFoundException ex)
                {
                    _logger.LogError(ex, "DeleteDonor failed with KeyNotFoundException");
                    return NotFound();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "DeleteDonor failed");
                    throw;
                }
            }
            [Authorize(Roles = "Manager")]
            [HttpGet("{GiftName}")]
            public async Task<ActionResult<List<Donor>>> GetDonorsByGift(string GiftName)
            {
                try
                {
                    _logger.LogInformation("Trying to get donors by gifts");
                    var donors = await _donorService.GetDonorsByGift(GiftName);
                    return Ok(donors);
                }
                catch (KeyNotFoundException ex)
                {
                    _logger.LogInformation(ex, "Not found donors");
                    return Ok();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to get donors");
                    throw;
                }
            }
        }
    }
}
