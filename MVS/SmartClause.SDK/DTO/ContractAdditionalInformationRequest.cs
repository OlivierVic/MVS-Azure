namespace SmartClause.SDK.DTO
{
    public class ContractAdditionalInformationRequest
    {
        /// <summary>
        /// The contract id
        /// </summary>
        public string ContractId { get; set; }
        /// <summary>
        /// The new language of the contract.
        /// Can be null to not change the previous value.
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// The new country of the contract.
        /// Set to null to not change the previous value.
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// The new project of the contract.
        /// Set to null to not change the previous value.
        /// </summary>
        public string Project { get; set; }
        /// <summary>
        /// The new contractor of the contract.
        /// Set to null to not change the previous value.
        /// </summary>
        public string Contractor { get; set; }
    }
}