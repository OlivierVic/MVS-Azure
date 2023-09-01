using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace SmartClause.SDK.DTO
{
    public class SearchCriterionDataRequest
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum StatusEnum
        {
            None = 0,
            InNegotiation = 1,
            InSignature,
            Signed,
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum ComparatorEnum
        {
            GreaterOrEqual = 1,
            LessOrEqual,
            GreaterThan,
            LessThan,
            EqualTo
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum UnitEnum
        {
            Months = 1,
            Years
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum TermSheetTypeEnum
        {
            Text = 1,
            Multi = 2,
            DateTime = 3,
            List = 4,
            Number = 9
        }

        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string Language { get; set; }
        public string[] ContractTypes { get; set; }
        public string ReferenceElementId { get; set; }
        public string Country { get; set; }
        public List<string> ContributorsEmails { get; set; }
        public string Project { get; set; }
        public List<string> ReferenceElementIds { get; set; }
        public string[] Status { get; set; }
        public ComparatorEnum? Comparator { get; set; }
        public int? Number { get; set; }
        public UnitEnum? Unit { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public TermSheetTypeEnum Type { get; set; }

    }
}