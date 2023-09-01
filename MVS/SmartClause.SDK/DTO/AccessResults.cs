using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartClause.SDK.DTO
{
    public partial class CheckAccess_Result
    {
        public string Type { get; set; }
        public string UserId { get; set; }
        public string ElementId { get; set; }
        public Nullable<int> AccessRequestStatus { get; set; }
        public Nullable<System.DateTime> AccessRequestDate { get; set; }
        public Nullable<bool> CanShared { get; set; }
        public Nullable<bool> Inherits { get; set; }
        public string OwnerName { get; set; }
    }

    public partial class GetAccessList_Result
    {
        public string Type { get; set; }
        public string UserType { get; set; }
        public string UserId { get; set; }
        public string ElementId { get; set; }
        public Nullable<int> AccessRequestStatus { get; set; }
        public Nullable<System.DateTime> AccessRequestDate { get; set; }
        public Nullable<bool> CanShared { get; set; }
        public Nullable<bool> Inherits { get; set; }
        public string Name { get; set; }
        public string Initials { get; set; }
        public string JobFamily { get; set; }
        public string Country { get; set; }
    }

    public class GetAllUsersWithAccess_Result
    {
        public List<GetAccessList_Result> AccessList { get; set; }
        public List<string> Countries { get; set; }
        public List<string> JobFamilies { get; set; }
    }
}
