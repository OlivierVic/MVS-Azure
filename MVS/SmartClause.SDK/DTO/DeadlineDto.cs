using System;

namespace SmartClause.SDK.DTO
{
    public class DeadlineResponse
    {
        public string Id { get; set; }
        public string ContractId { get; set; }
        public string FileId { get; set; }
        public string ContractName { get; set; }
        public string ProjectName { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public bool IsDone { get; set; }
        public string CompletedUserId { get; set; }
        public DateTime? CompletedDate { get; set; }
    }

    public class DeadlinesPerMonthBreakdownResponse
    {
        public int Month { get; set; }
        public int Count { get; set; }
    }

    public class UpdateDeadlineIsDoneRequest
    {
        public string DeadlineId { get; set; }
        public string CompletedUserId { get; set; }
        public bool? IsDone { get; set; }
    }
}