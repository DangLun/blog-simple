using Command.Domain.Entities;

namespace Command.Domain.Abstractions.Repositories
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken, int>
    {
        Task RevokeRefreshToken(RefreshToken refreshToken);
    }
}
