using System;
using System.Collections.Generic;

namespace Smartclause.SDK.DTO
{
    public class RemindersPerMonthBreakdownResponse
    {
        public int Month { get; set; }
        public int Count { get; set; }
    }

    public class ReminderForMonthResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DeadlineId { get; set; }
        public int Priority { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public bool IsDone { get; set; }
        public string ContractId { get; set; }
        public string FileId { get; set; }
        public string ContractName { get; set; }
        public string ProjectName { get; set; }
        public string CompleterId { get; set; }
        public DateTime? CompletedDateTime { get; set; }
    }

    public enum DateUnitEnum
    {
        Days,
        Months,
        Years
    }

    public enum DateOffsetDirectionEnum
    {
        Before,
        After
    }

    public class ReminderDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DeadlineId { get; set; }
        public int Priority { get; set; }
        public string Notes { get; set; }
        public int DateOffset { get; set; }
        public DateUnitEnum DateUnit { get; set; }
        public DateOffsetDirectionEnum DateOffsetDirection { get; set; }
        public string CreatorId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CompleterId { get; set; }
        public DateTime? CompletedDateTime { get; set; }
    }

    public class UpdateReminderIsDoneRequest
    {
        public string ReminderId { get; set; }
        public string CompleterId { get; set; }
        public string EndUserId { get; set; }
    }

    public class AddReminderRequest
    {
        public string Name { get; set; }
        public int DateOffset { get; set; }
        public DateUnitEnum DateUnit { get; set; }
        public DateOffsetDirectionEnum DateOffsetDirection { get; set; }
        public string Notes { get; set; }
        public int Priority { get; set; }
        public string CreatorId { get; set; }
        public string DeadlineId { get; set; }
        public List<string> SharedUserIds { get; set; }
    }

    public class UpdateReminderRequest
    {
        public string Name { get; set; }
        public int DateOffset { get; set; }
        public DateUnitEnum DateUnit { get; set; }
        public DateOffsetDirectionEnum DateOffsetDirection { get; set; }
        public string Notes { get; set; }
        public int Priority { get; set; }
        public string CreatorId { get; set; }
        public string DeadlineId { get; set; }
        public List<string> SharedUserIds { get; set; }
        public string EndUserId { get; set; }
    }

    public class DeleteReminderRequest
    {
        public string EndUserId { get; set; }
    }
}