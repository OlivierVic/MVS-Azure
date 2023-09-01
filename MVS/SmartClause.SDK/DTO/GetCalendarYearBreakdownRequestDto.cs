namespace SmartClause.SDK.DTO
{
    public class GetCalendarYearBreakdownRequestDto
    {
        public string EndUserId { get; set; }
        public string EndUserEmail { get; set; }
        public int Year { get; set; }
        public CalendarFilterDto Filter { get; set; }
    }
}