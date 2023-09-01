namespace SmartClause.SDK.DTO
{
    /// <summary>
    /// The request to get the contracts a contributor contributed to.
    /// </summary>
    public class SearchContributorContractsRequest
    {
        /// <summary>
        /// The search query
        /// </summary>
        public string SearchQuery { get; set; }
        /// <summary>
        /// The email of the contributor
        /// </summary>
        public string ContributorEmail { get; set; }
    }
}