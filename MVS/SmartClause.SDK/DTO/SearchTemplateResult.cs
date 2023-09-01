using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartclause.SDK.DTO
{
    public class SearchTemplateResult
    {
        public List<Dossier> TemplateFolders { get; set; }
        public List<Template> Templates { get; set; }
    }
}
