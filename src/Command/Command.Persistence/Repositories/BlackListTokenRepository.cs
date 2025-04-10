using Command.Domain.Abstractions.Repositories;
using Command.Domain.Entities;

namespace Command.Persistence.Repositories
{
    public class BlackListTokenRepository(ApplicationDbContext dbContext)
        : GenericRepository<BlackListToken, int>(dbContext), IBlackListTokenRepository
    {
        
    }
}
