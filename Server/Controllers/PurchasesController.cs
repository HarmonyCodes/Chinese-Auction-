using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Interface;
using Project.models;
using Project.models.DTOs;
using Server.BLL.Interface;
using System.Security.Claims;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, IMapper mapper, ILogger<UserController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserDto userDto)
        {

            try
            {
                _logger.LogInformation($"Add user: UserName: {userDto.UserName}, Email {userDto.Address}");
                var user = _mapper.Map<User>(userDto);
                var token = await _userService.Register(user);
                return Ok(new { token });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "AddUser failed with ArgumentException");
                return Conflict();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddUser failed");
                throw ex;
            }

        }
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {

            try
            {
                _logger.LogInformation($"Login user: user Name: {loginDto.UserName} user Password {loginDto.Password}");
                var token = await _userService.Login(loginDto);
                return Ok(new { token });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Login user failed with ArgumentException");
                return Conflict();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login user failed");
                throw ex;
            }

        }

    }

}
