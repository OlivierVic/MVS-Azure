using System;

namespace SmartClause.SDK.DTO
{
    public class ForceSignContractRequest
    {
        public bool Sign { get; set; } = true;
        public DateTime? SignedDateTimeUtc { get; set; }
    }
}
