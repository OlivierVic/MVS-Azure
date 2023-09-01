using System.Collections.Generic;

namespace Smartclause.SDK.DTO
{
    public class TemplateWorkflow
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string IdTemplate { get; set; }
        public List<TemplateWorkflowStep> WorkflowTemplateSteps { get; set; }


    }
}
