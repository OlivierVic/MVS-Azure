using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartclause.SDK.DTO
{
    public class SignContractRequest
    {
        public List<FileToSign> Files { get; set; }
        public string Id { get; set; }

        public string FileUrl { get; set; }
        public List<string> Emails { get; set; }
        public string HookUrl { get; set; }
        public string ReturnUrl { get; set; }
        public string EmailTitle { get; set; }

        public string EmailSender { get; set; }

        public string EmailBody { get; set; }
        public string Platform { get; set; }
        public int SignatureType { get; set; } = 1;
        public List<UserContact> Contacts { get; set; }
        public bool SendEnvelopeDirectly { get; set; }
        public class FileToSign
        {
            public string Id { get; set; }
            public string FileUrl { get; set; }
            public string FileName { get; set; }
            public bool AddAnchor { get; set; }
        };
    }

    public class SignMultipleContractsRequest
    {
        public List<string> Ids { get; set; }

        public List<string> FilesUrl { get; set; }
        public List<string> Emails { get; set; }
        public string HookUrl { get; set; }
        public string ReturnUrl { get; set; }
        public string EmailTitle { get; set; }

        public string EmailSender { get; set; }

        public string EmailBody { get; set; }
        public string Platform { get; set; }

        public List<UserContact> Contacts { get; set; }
        public bool SendEnvelopeDirectly { get; set; }
    }

    public class ContractsSignatureAnchor
    {
        public List<FileToSign> Files { get; set; }
        public List<UserContact> Contacts { get; set; }

        public string HookUrl { get; set; }

        public string ReturnUrl { get; set; }

        public string EmailTitle { get; set; }
        public string EmailBody { get; set; }
        public string EmailSender { get; set; }
        public string Platform { get; set; }
        public bool SendEnvelopeDirectly { get; set; }
        public int? SignatureType { get; set; }

        public class FileToSign
        {
            public string Id { get; set; }
            public string FileUrl { get; set; }
            public string FileName { get; set; }
            public bool AddAnchor { get; set; }
        };
    }

    public class ContractVoidDocuSignEnvelope
    {
        public string EnvelopeId { get; set; }
        public string VoidMessage { get; set; }
        public string Platform { get; set; }
    }
}
