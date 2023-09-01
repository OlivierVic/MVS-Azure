using System;
using System.Collections.Generic;

namespace MVS.Common.Models
{
    public partial class Answer
    {
        public int Id { get; set; }
        /// <summary>
        /// Id user répond à la question
        /// </summary>
        public string Question { get; set; }
        /// <summary>
        /// Id bénéficiaire/dossier pour lequel répond
        /// </summary>
        public string VaultId { get; set; }
        public string Data { get; set; }
        /// <summary>
        /// Champ libre pour compléments
        /// </summary>
        public string Comment { get; set; }
        public int? Job { get; set; }
        public bool? Selected { get; set; }

        public virtual JobParticular JobNavigation { get; set; }
    }
}
