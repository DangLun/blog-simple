using Command.Domain.Entities;

namespace Command.Domain.Abstractions.Repositories
{
    public interface ICommentRepository : IGenericRepository<Comment, int>
    {
    }
}
