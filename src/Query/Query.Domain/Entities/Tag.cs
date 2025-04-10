using Query.Domain.Abstractions.Aggregates;

namespace Query.Domain.Entities
{
    public class Tag : AggregateRoot<int>
    {
        public string TagName {  get; set; }
        public string? ClassName {  get; set; }
        public bool IsDeleted { get; set; } 
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<PostTag>? PostTags { get; set; }
    }
}
