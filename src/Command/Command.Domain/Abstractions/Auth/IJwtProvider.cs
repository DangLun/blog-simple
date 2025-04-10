using Command.Domain.Entities;
using System.Security.Claims;

namespace Command.Domain.Abstractions.Auth
{
    public interface IJwtProvider
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        bool ValidateAccessToken(string accessToken);
        ClaimsPrincipal GetClaimsPrincipal(string accessToken);
    }
}
