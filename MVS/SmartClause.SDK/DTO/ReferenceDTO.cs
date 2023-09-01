using System.Collections.Generic;

namespace SmartClause.SDK.DTO
{
    public class ReferenceDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DefaultExtraData { get; set; }
        public string KeyFieldName { get; set; }
        public string ValueFieldName { get; set; }
        public List<ReferenceElementDTO> ReferenceElements { get; set; }
        public string TenantId { get; set; }
    }
}
