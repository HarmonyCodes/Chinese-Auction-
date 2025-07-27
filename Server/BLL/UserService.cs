using Microsoft.AspNetCore.Identity;
using Project.models.DTOs;
using Server.BLL.Interface;
using Server.DAL.Interfaces;

namespace Server.BLL
{
    public class UserService : IUserService
    {
        private readonly PasswordHasher<User> _passwordHasher = new();
        private readonly IUserDal _userDal;
        private readonly IJWTService _jwtService;
        public UserService(IUserDal userDal, IJWTService jwtService)
        {
            _userDal = userDal;
            _jwtService = jwtService;
        }

        public async Task<string> Register(User user)
        {
            if (user.UserName == null || user.Email == null || user.Password == null)
                throw new ArgumentNullException(nameof(user), "UserName Email and Password  cannot be null.");

            // Email validation
            if (!System.Text.RegularExpressions.Regex.IsMatch(user.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Invalid email format.");

            // Password validation: at least 8 chars, one uppercase, one lowercase, one digit, one special char
            if (!System.Text.RegularExpressions.Regex.IsMatch(user.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$"))
                throw new ArgumentException("Password must be at least 8 characters and include uppercase, lowercase, digit, and special character.");

            // Phone validation: Israeli format example (adjust as needed)
            if (user.Phone != null && !string.IsNullOrEmpty(user.Phone) && !System.Text.RegularExpressions.Regex.IsMatch(user.Phone, @"^05\d{8}$"))
                throw new ArgumentException("Phone number must be a valid Israeli mobile number (e.g., 05XXXXXXXX).");


            user.PasswordHash = _passwordHasher.HashPassword(user, user.Password);

            var savedUser = await _userDal.Register(user);
            var token = _jwtService.GenerateToken(savedUser);

            return token;
        }


        public async Task<string> Login(LoginDto loginDto)
        {
            var findUser = await _userDal.Login(loginDto);
            if (findUser == null)
            {
                throw new ArgumentNullException(nameof(loginDto), "User not found");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(loginDto.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$"))
                throw new ArgumentException("Password must be at least 8 characters and include uppercase, lowercase, digit, and special character.");


            var result = _passwordHasher.VerifyHashedPassword(findUser, findUser.PasswordHash, loginDto.Password);

            if (result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                var token = _jwtService.GenerateToken(findUser);

                return token;
            }
            else
            {
                throw new Exception("Incorrect password");
            }
        }

    }
}
