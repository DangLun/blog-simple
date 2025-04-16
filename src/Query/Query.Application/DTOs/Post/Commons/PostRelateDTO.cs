namespace Query.Application.DTOs.Post.Commons
{
    public class PostRelateDTO
    {
        public int PostId { get; set; } 
        public string PostTitle { get; set; }   
        public List<TagDTO>? Tags { get; set; }
    }
}
