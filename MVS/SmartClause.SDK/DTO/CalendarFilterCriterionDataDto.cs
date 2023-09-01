using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SmartClause.SDK.DTO
{
    public class CalendarFilterCriterionDataDto
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum ItemTypeEnum
        {
            Deadline,
            Reminder,
        }

        public ItemTypeEnum? ItemType { get; set; }
        public int? Priority { get; set; }
        public string Project { get; set; }
        public string TemplateId { get; set; }
        public bool? State { get; set; }
        public string Entity { get; set; }
        public string ProviderOrClient { get; set; }
    }
}