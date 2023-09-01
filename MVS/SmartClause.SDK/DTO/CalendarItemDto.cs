using System;

namespace SmartClause.SDK.DTO
{
    public class CalendarItemDto
    {
        public string DeadlineId { get; set; }
        public string ReminderId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public bool IsDone { get; set; }
        public int Priority { get; set; }
        public string ContractId { get; set; }
        public string FileId { get; set; }
        public string ContractName { get; set; }
        public string ProjectName { get; set; }
        public string CreatorId { get; set; }
        public string CompleterId { get; set; }
        public DateTime? CompletedDateTime { get; set; }
    }
}