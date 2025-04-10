using Contract.Abstractions.Services;

namespace Command.Infrastructure.Configurations
{
    public class GoogleAuthSettings : IGoogleAuthSettings
    {
        public string UserInfoEndpoint { get; set; }
    }
}
