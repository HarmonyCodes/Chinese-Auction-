using Project.DAL;
using Project.models.DTOs;
using Server.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Server.DAL
{
    public class UserDal:IUserDal
    {
        private readonly AppDbContext _appDbContext;
        public UserDal(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;

        }
        public async Task<User> Login(LoginDto loginDto)
        {
            return await _appDbContext.Users.Where(user => user.UserName.Equals(loginDto.UserName)).FirstOrDefaultAsync();
        }
        public async Task<User> Register(User user)
        {
            _appDbContext.Users.Add(user);
            await _appDbContext.SaveChangesAsync();

            return user;
        }
    }
}
