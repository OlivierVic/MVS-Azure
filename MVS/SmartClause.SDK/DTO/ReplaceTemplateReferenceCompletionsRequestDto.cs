using System.Collections.Generic;

namespace Smartclause.SDK.DTO
{
    public class ReplaceTemplateReferenceCompletionsRequestDto
    {
        public class Completion
        {
            public string TemplateWrite { get; set; }
            public string ReferenceId { get; set; }
            public string ReferenceElementPath { get; set; }
        }

        public List<Completion> Completions { get; set; }
    }
}