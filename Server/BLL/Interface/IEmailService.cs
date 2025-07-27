namespace Project.BLL.Interface
{
    public interface IEmailService
    {
        Task SendWinnerEmailAsync(string winnerEmail, string subject, string body);
    }

}
