namespace Contract.Abstractions.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);

        string GenerateTokenLink(string token, string routeTo);
    }
}
