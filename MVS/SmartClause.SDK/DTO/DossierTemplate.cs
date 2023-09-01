using Smartclause.SDK.DTO;

namespace SmartClause.SDK.DTO
{
    public class DossierTemplate
    {
        public string Id { get; set; }
        public string DossierId { get; set; }
        public string TemplateId { get; set; }
    
        public virtual Dossier Dossier { get; set; }
        public virtual Template Template { get; set; }
    }
}