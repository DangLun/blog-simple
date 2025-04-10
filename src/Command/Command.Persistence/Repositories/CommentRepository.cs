using Command.Domain.Abstractions.Repositories;
using Command.Domain.Entities;

namespace Command.Persistence.Repositories
{
    public class CommentRepository(ApplicationDbContext dbContext) 
        : GenericRepository<Comment, int>(dbContext), ICommentRepository 
    {
        
    }
}
