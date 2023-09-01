using System;
using System.Collections.Generic;

namespace MVS.Common.Models
{
    public partial class VaultCategory
    {
        public VaultCategory()
        {
            VaultContacts = new HashSet<VaultContact>();
            VaultTiersContacts = new HashSet<VaultTiersContact>();
        }

        public int Id { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<VaultContact> VaultContacts { get; set; }
        public virtual ICollection<VaultTiersContact> VaultTiersContacts { get; set; }
    }
}
