using System;
using System.Collections.Generic;

namespace Smartclause.SDK.DTO
{
    public class WorkflowStepUser
    {
        public string Id { get; set; }
        public string WorkflowStepId { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Rights { get; set; }
        public Nullable<bool> Optional { get; set; }
        public string DisplayName { get; set; }
        public string FieldId { get; set; }
        public List<Condition> Conditions { get; set; } = new List<Condition>();
        public ConditionNode ConditionNode { get; set; }
    }
}
