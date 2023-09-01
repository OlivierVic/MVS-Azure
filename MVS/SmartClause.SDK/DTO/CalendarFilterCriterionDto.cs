using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SmartClause.SDK.DTO
{
    public class CalendarFilterCriterionDto
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum TypeEnum
        {
            ItemType,
            Priority,
            Project,
            ContractType,
            State,
            Entity,
            ProviderOrClient,
        }

        public TypeEnum Type { get; set; }
        public CalendarFilterCriterionDataDto Data { get; set; }
    }
}