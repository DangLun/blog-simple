using AutoMapper;
using Command.Application.Commands.Comment;
using Command.Domain.Entities;

namespace Command.Presentation.Mapping
{
    /// <summary>
    /// Defines mapping configurations for the presentation layer.
    /// </summary>
    public class PresentationMappingProfile : Profile
    {
        public PresentationMappingProfile()
        {
            CreateMap<CreateCommentCommand, Comment>();
        }
    }
}