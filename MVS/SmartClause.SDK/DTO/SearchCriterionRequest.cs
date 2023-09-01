using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SmartClause.SDK.DTO
{
    public class SearchCriterionRequest
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum TypeEnum
        {
            DateSignature = 1,
            DateContractStart,
            DateContractEnd,
            Language,
            ContractType,
            Country,
            Owner,
            Contributors,
            Project,
            Status,
            Duration,
            FinancialAmount,
            ReferenceElements,
            TermSheets
        }

        public TypeEnum Type { get; set; }
        public SearchCriterionDataRequest Data { get; set; }
    }
}