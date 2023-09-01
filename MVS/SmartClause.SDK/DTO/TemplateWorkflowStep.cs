using System;
using System.Collections.Generic;

namespace Smartclause.SDK.DTO
{
    public class TemplateWorkflowStep
    {
        public string Id { get; set; }
        public string WorkflowTemplateId { get; set; }
        public string ConditionRootId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> BuiltinOption { get; set; }
        public string ParentId { get; set; }
        public int StepOrder { get; set; }
        public Nullable<bool> Optional { get; set; }
        public List<WorkflowStepUser> WorkflowStepUsers { get; set; }
    }
}
