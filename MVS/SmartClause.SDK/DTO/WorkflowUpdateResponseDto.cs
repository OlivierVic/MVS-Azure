using System.Collections.Generic;

namespace SmartClause.SDK.DTO
{
    public class WorkflowUpdateResponseDto
    {
        public class Step
        {
            public string Name { get; set; }
        }

        public List<Step> AddedSteps { get; set; }
        public List<Step> RemovedSteps { get; set; }
    }
}
