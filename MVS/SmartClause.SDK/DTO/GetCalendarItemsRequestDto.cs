namespace SmartClause.SDK.DTO
{
    public class GetCalendarItemsRequestDto
    {
        public string EndUserId { get; set; }
        public string EndUserEmail { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public CalendarFilterDto Filter { get; set; }
    }
}