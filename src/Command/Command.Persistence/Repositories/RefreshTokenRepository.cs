using Command.Domain.Abstractions.Repositories;
using Command.Domain.Entities;

namespace Command.Persistence.Repositories
{
    public class RefreshTokenRepository(ApplicationDbContext dbContext)
        : GenericRepository<RefreshToken, int>(dbContext), IRefreshTokenRepository
    {
        public async Task RevokeRefreshToken(RefreshToken refreshToken)
        {
            refreshToken.IsRevoked = true;
            await SaveChangesAsync();
        }
    }
}
