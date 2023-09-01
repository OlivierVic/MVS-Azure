using System.Collections.Generic;

namespace SmartClause.SDK.DTO
{
    public class TermSheetElementResponse
    {
        public int Id { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public bool CanCreateAlert { get; set; }
        public int Order { get; set; }
        public List<TermSheetElementDataResponse> DataList { get; set; }
        public bool IsProtected { get; set; }
    }
}