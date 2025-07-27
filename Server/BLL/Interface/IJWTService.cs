namespace Server.BLL.Interface
{
    public interface IJWTService
    {
        string GenerateToken(User user);
    }
}
