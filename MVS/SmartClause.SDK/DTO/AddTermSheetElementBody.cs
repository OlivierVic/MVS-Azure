namespace SmartClause.SDK.DTO
{
    public class AddOrUpdateCommonTermSheetElementBody
    {
        public int? ElementId { get; set; }
        public int? DataId { get; set; }
        public string ContractId { get; set; }
        public string FileId { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public int Order { get; set; }
        public bool IsProtected { get; set; }
        public string CreatorEndUserId { get; set; }
        public string SuggestionValue { get; set; }
    }
}