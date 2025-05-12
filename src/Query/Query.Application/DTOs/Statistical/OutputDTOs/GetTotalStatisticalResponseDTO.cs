namespace Query.Application.DTOs.Statistical.OutputDTOs
{
    public class GetTotalStatisticalResponseDTO
    {
        public int TotalPost { get; set; } 
        public int TotalUser { get; set; }
        public int TotalUserLoginToday { get; set; }
    }
}
