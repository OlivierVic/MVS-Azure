namespace SmartClause.SDK.DTO
{
    public class CreateTemplateResult
    {
        public string Type { get; set; }
        public string Content { get; set; }
        public string Json { get; set; }
        public int Line { get; set; }
        public string QuestionName { get; set; }
        public string JsonError { get; set; }
        public string ErrorFrom { get; set; }
    }
}
