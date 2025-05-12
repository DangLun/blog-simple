using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;
using Query.Application.DTOs.Post.Commons;
using Query.Application.DTOs.Post.OutputDTO;
using Query.Application.Query.Post;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Entities;


namespace Query.Application.UserCases.Post
{
    public class GetDetailPostQueryValidator : AbstractValidator<GetDetailPostQuery>
    {
        public GetDetailPostQueryValidator() {
            RuleFor(x => x.PostId).NotNull().GreaterThan(0);
        }
    }
    public class GetDetailPostQueryHandler : IRequestHandler<GetDetailPostQuery, Result<GetDetailPostResponseDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetDetailPostQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<GetDetailPostResponseDTO>> Handle(GetDetailPostQuery request, CancellationToken cancellationToken)
        {
            var postRepo = unitOfWork.Repository<Domain.Entities.Post, int>();
            var postTextRepo = unitOfWork.Repository<PostText, int>();
            var commentRepo = unitOfWork.Repository<Domain.Entities.Comment, int>();
            var savedPostRepo = unitOfWork.Repository<PostSaved, int>();
            var postReactionRepo = unitOfWork.Repository<PostReaction, int>();
            var followRepo = unitOfWork.Repository<Follow, int>();
            var tagRepo = unitOfWork.Repository<Domain.Entities.Tag, int>();
            var userRepo = unitOfWork.Repository<Domain.Entities.User, int>();
            var reactionRepo = unitOfWork.Repository<Domain.Entities.Reaction, int>();

            var follows = followRepo.FindAll();
            var tags = tagRepo.FindAll();
            var users = userRepo.FindAll();
            var reactions = reactionRepo.FindAll(false, x => !x.IsDeleted, x=>x.PostReactions); 

            var postRelates = postRepo.FindAll();

            var post = await postRepo.FirstOrDefaultAsync(false, x => x.Id == request.PostId && !x.IsDeleted,cancellationToken,
                x => x.User, 
                x => x.PostTags, 
                x=> x.Comments, 
                x=> x.SavedByUsers,
                x => x.PostReactions,
                x => x.PostText);

            if(post == null)
            {
                var message = MessageConstant.NotFound<Domain.Entities.Post>(x => x.Id, request.PostId);
                return Result.Failure(Error.NotFound(message));
            }

            var response = new PostDetailDTO
            {
                Id = post.Id,
                CreatedAt = post.CreatedAt,
                IsDeleted = post.IsDeleted,
                IsPublished = post.IsPublished,
                PostSummary = post.PostSummary,
                PostThumbnail = post.PostThumbnail,
                PostTextId = post.PostTextId,
                PostTitle = post.PostTitle,
                TotalComments = post.TotalComments,
                TotalReactions = post.TotalReactions,
                TotalReads = post.TotalReads,
                UpdatedAt = post.UpdatedAt,

                IsSaved = post.SavedByUsers == null ? false : post.SavedByUsers.Any(su => su.UserId == request.UserIdCall && su.IsActived) ?
                            true : false,
                PostText = post.PostText.Content,

                IsFollowed = follows.FirstOrDefault(f => f.FollowedId == post.User.Id && f.FollowerId == request.UserIdCall) != null,

                IsMe = post.User != null && post.User.Id == request.UserIdCall,

                TotalSaved = post.SavedByUsers == null ? 0 : post.SavedByUsers.Where(s => s.IsActived).ToList().Count,

                IsReactied = post.PostReactions == null ? false :
                    post.PostReactions.Any(pr => pr.UserId == request.UserIdCall && pr.PostId == post.Id && pr.IsActived),

                IsCommented = post.Comments == null ? false :
                    post.Comments.Any(c => c.UserId == request.UserIdCall),
                Comments = (from c in post.Comments join u in users on
                            c.UserId equals u.Id where c.ParentCommentId == 0
                            && c.PostId == post.Id select new { c, u }).OrderByDescending(o => o.c.CreatedAt).Select(y => new CommentDTO
                            {
                                CommentText = y.c.CommentText,
                                CreatedAt = y.c.CreatedAt,
                                Id = y.c.Id,
                                IsDeleted = y.c.IsDeleted,
                                IsMine = y.c.UserId == request.UserIdCall,
                                ParentCommentId = y.c.ParentCommentId,
                                PostId = y.c.PostId,
                                UpdatedAt = y.c.UpdatedAt,
                                UserId = y.c.UserId,
                                Depth = GetDepthComment(y.c.Id, post.Id),
                                UserComment = new UserDTO
                                {
                                    Avatar = y.u.Avatar,
                                    Email = y.u.Email,
                                    FullName = y.u.FullName,
                                    Id = y.u.Id,
                                    IsLoginWithGoogle = y.u.IsLoginWithGoogle
                                },
                                Replieds = GetReplieds(y.c.Id, request.UserIdCall, post.Id)
                            }).ToList(),
                PostRelates = postRelates.Where(pr => pr.UserId == post.User.Id && !pr.IsDeleted && pr.IsPublished && pr.Id != post.Id).Take(5).Select(p => new PostRelateDTO
                {
                    PostId = p.Id,
                    PostTitle = p.PostTitle,
                    Tags = tags.Where(t => t.PostTags.Any(pt => pt.PostId == p.Id) && !t.IsDeleted).Select(x => new TagDTO
                    {
                        Id = x.Id,
                        ClassName = x.ClassName,
                        CreatedAt = x.CreatedAt,
                        Description = x.Description,
                        IsDeleted = x.IsDeleted,
                        TagName = x.TagName,
                        UpdatedAt = x.UpdatedAt,
                    }).ToList()
                }).ToList(),
                Author = new AuthorDTO
                {
                    Id = post.User.Id,
                    Avatar = post.User.Avatar,
                    Email = post.User.Email,
                    FullName = post.User.FullName,
                    IsLoginWithGoogle = post.User.IsLoginWithGoogle,
                    JoinedDate = post.User.CreatedAt,
                    Bio = post.User.Bio,
                },
                Tags = tags.Where(t => t.PostTags.Any(p => p.PostId == post.Id) && !t.IsDeleted).Select(x => new TagDTO
                {
                    Id = x.Id,
                    ClassName = x.ClassName,
                    CreatedAt = x.CreatedAt,
                    Description = x.Description,
                    IsDeleted = x.IsDeleted,
                    TagName = x.TagName,
                    UpdatedAt = x.UpdatedAt,
                }).ToList(),
                Reactions = reactions.Select(r => new ReactionDTO
                {
                    Id = r.Id,
                    Count = r.PostReactions != null ? r.PostReactions.Count(pr => pr.PostId == post.Id && pr.IsActived) : 0,
                    IsSelected = r.PostReactions != null ? r.PostReactions.Any(pr => pr.PostId == post.Id && pr.UserId == request.UserIdCall && pr.IsActived) : false,
                    ReactionIcon = r.ReactionIcon,
                    ReactionName = r.ReactionName
                }).ToList()
            };

            return Result.Success(new GetDetailPostResponseDTO
            {
                Post = response
            });
        }
        
        private int GetDepthComment(int commentId, int postId)
        {
            var commentRepo = unitOfWork.Repository<Domain.Entities.Comment, int>();

            int depth = 1;

            var comments = commentRepo.FindAll(false, x => x.PostId == postId).OrderBy(x => x.Id).ToList();

            var comment = comments.FirstOrDefault(x => x.Id == commentId);

            if (comment == null || comment.ParentCommentId == 0) return 1;
            

            while(comment.ParentCommentId != 0)
            {
                depth++;
                int l = 0, r = comments.Count() - 1;
                Domain.Entities.Comment? parentComment = null;
                while(l <= r)
                {
                    int mid = l + r >> 1;
                    if (comments[mid].Id == comment.ParentCommentId)
                    {
                        parentComment = comments[mid];
                        break;
                    }
                    else if (comments[mid].Id > comment.ParentCommentId) r = mid - 1;
                    else l = mid + 1;
                }
                if (parentComment == null) return depth;
                comment = parentComment;
            }
            return depth;
        }

        private List<CommentDTO> GetReplieds(int commentId, int? userIdCall, int postId)
        {
            var response = new List<CommentDTO>();
            var commentRepo = unitOfWork.Repository<Domain.Entities.Comment, int>();
            
            var commentChilds = commentRepo.FindAll(false, x => x.ParentCommentId == commentId 
            && x.PostId == postId, x=>x.User).ToList();

            foreach (var comment in commentChilds)
            {
                response.Add(new CommentDTO
                {
                    CommentText = comment.CommentText,
                    Id = comment.Id,
                    IsDeleted = comment.IsDeleted,
                    Depth = GetDepthComment(comment.Id, postId),
                    CreatedAt = comment.CreatedAt,
                    IsMine = comment.UserId == userIdCall,
                    ParentCommentId = comment.ParentCommentId,
                    PostId = comment.PostId,
                    UpdatedAt = comment.UpdatedAt,
                    UserComment = new UserDTO
                    {
                        Avatar = comment.User.Avatar,
                        Email = comment.User.Email,
                        FullName = comment.User.FullName,
                        Id = comment.User.Id,
                        IsLoginWithGoogle = comment.User.IsLoginWithGoogle,
                    },
                    UserId = comment.UserId,
                    Replieds = GetReplieds(comment.Id, userIdCall, postId)
                });
            }

            return response;
        }
    }
}
