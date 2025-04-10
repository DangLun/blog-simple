namespace Command.Contract.DTOs
{
    public class UpdateSampleRequestDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
    }
}