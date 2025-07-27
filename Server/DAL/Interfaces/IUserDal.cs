using Project.models.DTOs;

namespace Server.DAL.Interfaces
{
    public interface IUserDal
    {
        Task<User> Register(User user);
        Task<User> Login(LoginDto loginDto);
    }
}
