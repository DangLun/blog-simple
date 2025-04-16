namespace Query.Application.DTOs.Post.Commons
{
    public class PostDetailDTO
    {
        public int Id { get; set; }
        public string PostTitle { get; set; }
        public string? PostThumbnail { get; set; }
        public string? PostSummary { get; set; }
        public int PostTextId { get; set; }
        public string PostText { get; set; }
        public int TotalReactions { get; set; }
        public int TotalComments { get; set; }
        public int TotalReads { get; set; }
        public int TotalSaved { get; set; }
        public bool IsPublished { get; set; }
        public bool IsDeleted { get; set; }
        public int TotalLike { get; set; }
        public int TotalHeart { get; set; }
        public int TotalHappy { get; set; }
        public int TotalHaha { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsSaved { get; set; }
        public bool? IsReactied { get; set; }
        public bool? IsCommented { get; set; }
        public bool? IsMe { get; set; }
        public bool? IsFollowed { get; set; }
        public AuthorDTO? Author { get; set; }
        public List<PostRelateDTO>? PostRelates { get; set; }
        public List<TagDTO>? Tags { get; set; }
        public List<CommentDTO>? Comments { get; set; }
        public List<ReactionDTO>? Reactions { get; set; }
    }
}
