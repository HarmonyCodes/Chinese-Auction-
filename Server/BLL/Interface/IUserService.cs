using Project.models.DTOs;

namespace Server.BLL.Interface
{
    public interface IUserService
    {
        Task<string> Register(User user);

        Task<string> Login(LoginDto loginDto);
    }
}
