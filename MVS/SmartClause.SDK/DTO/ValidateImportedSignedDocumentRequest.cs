using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartClause.SDK.DTO
{
    public class ValidateImportedSignedDocumentRequest
    {
        [Required]
        public DateTime SignedDateTimeUtc { get; set; }
    }
}
