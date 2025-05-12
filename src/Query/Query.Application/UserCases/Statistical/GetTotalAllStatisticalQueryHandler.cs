using Contract.Shared;
using MediatR;
using Query.Application.DTOs.Statistical.OutputDTOs;
using Query.Application.Query.Statistical;
using Query.Domain.Abstractions.Repositories;

namespace Query.Application.UserCases.Statistical
{
    public class GetTotalAllStatisticalQueryHandler : IRequestHandler<GetTotalStatisticalQuery, Result<GetTotalStatisticalResponseDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetTotalAllStatisticalQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<GetTotalStatisticalResponseDTO>> Handle(GetTotalStatisticalQuery request, CancellationToken cancellationToken)
        {
            var postRepo = unitOfWork.Repository<Domain.Entities.Post, int>();
            var userRepo = unitOfWork.Repository<Domain.Entities.User, int>();

            var posts = postRepo.FindAll(false, x => !x.IsDeleted);
            var users = userRepo.FindAll();
            var totalUserLoginToday = users.Where(x => x.LastLoginAt == DateTime.Now).Count();

            var response = new GetTotalStatisticalResponseDTO
            {
                TotalPost = posts.Count(),
                TotalUser = users.Count(),
                TotalUserLoginToday = totalUserLoginToday
            };

            return Result.Success(response);
        }
    }
}
