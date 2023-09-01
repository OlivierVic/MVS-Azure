namespace SmartClause.SDK.DTO
{
    /// <summary>
    /// An object representing a template to create from a file
    /// </summary>
    public class FromFileTemplate
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Field { get; set; }
        public string Language { get; set; }
        public string Nation { get; set; }
        public string Authors { get; set; }
        public string VaultId { get; set; }
        public bool IsDraft { get; set; }
    }
}