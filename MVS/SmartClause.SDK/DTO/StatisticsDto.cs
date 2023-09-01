using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace SmartClause.SDK.DTO
{
    public class StatisticsRequest
    {
        public DateRangeRequest DateRange { get; set; }
        public List<SearchCriterionRequest> Criteria { get; set; }
    }

    public class DateRangeRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class AvailableTemplateCountResult
    {
        public int AvailableTemplateCount { get; set; }
    }

    public class GeneratedCountResult
    {
        public int GeneratedCount { get; set; }
    }

    public class PercentageSignedResult
    {
        public double PercentageSigned { get; set; }
    }

    public class AverageNegotiationTimeResult
    {
        public double AverageDays { get; set; }
    }

    public class BreakdownPairResponse<T>
    {
        public T Name { get; set; }
        public double Percentage { get; set; }
    }

    public class BreakdownPairResponse : BreakdownPairResponse<string>
    { }

    public class AverageResult
    {
        public double Average { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ContractStatusEnum
    {
        None = 0,
        InNegotiation = 1,
        InSignature,
        Signed
    }

    public class UniqueVisitorsResponse
    {
        public int UniqueVisitors { get; set; }
    }

    public class UniqueVisitorsPerDayAverageResponse
    {
        public double UniqueVisitorsPerDayAverage { get; set; }
    }

    public class Statistics
    {
        public StatisticsPersonalized Personalized { get; set; } = new();
        public StatisticsContractual Contractual { get; set; } = new();
    }

    public class StatisticsContractual
    {
        public int TemplatesCount { get; set; }
        public int GeneratedCount { get; set; }
        public double SignedPercentage { get; set; }
        public double AverageNegotiationDays { get; set; }
        public List<BreakdownPairResponse> ContractTypes { get; set; }
    }

    public class StatisticsPersonalized
    {
        public int GeneratedCount { get; set; }
        public double AverageFinancialAmount { get; set; }
        public double AverageNegotiationDays { get; set; }
        public List<BreakdownPairResponse> Countries { get; set; }
        public List<BreakdownPairResponse> Status { get; set; }
    }
}