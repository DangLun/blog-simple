using Query.Domain.Entities;
using System.Security.Claims;

namespace Query.Domain.Abstractions.Auth
{
    public interface IJwtProvider
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        bool ValidateAccessToken(string accessToken);
        ClaimsPrincipal GetClaimsPrincipal(string accessToken);
    }
}
