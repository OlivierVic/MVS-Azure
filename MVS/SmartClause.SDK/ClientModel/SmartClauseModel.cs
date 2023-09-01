using System;
using System.Collections.Generic;

namespace Smartclause.SDK.ClientModel
{
    public class SmartClauseModel
    {
        public string Url { get; set; }

        public string AccessToken { get; set; }

        public int ProjectId { get; set; }

        public string TemplateTitle { get; set; }

        public string ContractId { get; set; }

        public string TemplateId { get; set; }

        public string ContractTitle { get; set; }

        public string Parties { get; set; }

        public List<ContractComment> Comments { get; set; }

        public List<DTO.Party> PartyList { get; set; }

        public int StartingTab { get; set; }

        public bool SentForSignature { get; set; }

        public bool IsSigned { get; set; }

        public DTO.Party CurrentUser { get; set; }
    }

    public partial class ContractComment
    {
        public ContractComment()
        {
            this.Children = new HashSet<ContractComment>();
        }

        public int Id { get; set; }
        public string IdContract { get; set; }
        public string IdUser { get; set; }
        public string UserName { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string Comment { get; set; }
        public string RefId { get; set; }
        public Nullable<int> IdContractComment { get; set; }
        public int Type { get; set; }
        public string PartyUID { get; set; }

        public virtual ICollection<ContractComment> Children { get; set; }
        public virtual ContractComment Parent { get; set; }
    }
}
