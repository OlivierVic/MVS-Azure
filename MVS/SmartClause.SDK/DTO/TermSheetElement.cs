using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartClause.SDK.DTO
{
    public class TermSheetElementData
    {
        public int Id { get; set; }
        public string Source { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Value { get; set; }
        public string ContractField { get; set; }
        public string Type { get; set; }
        public string SuggestionValue { get; set; }
    }

    public class TermSheetElement
    {
        public int Id { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public bool CanCreateAlert { get; set; }
        public int Order { get; set; }
        public List<TermSheetElementData> DataList { get; set; }
    }
}