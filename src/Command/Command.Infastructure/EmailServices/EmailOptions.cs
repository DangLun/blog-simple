namespace Command.Infrastructure.EmailServices
{
    public class EmailOptions
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string SenderEmail { get; set; }
        public string SenderPassword { get; set; }
        public bool EnableSsl { get; set; }
        public string VerificationUrl { get; set; }
    }
}
