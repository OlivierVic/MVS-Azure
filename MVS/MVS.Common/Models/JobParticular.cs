using System;
using System.Collections.Generic;

namespace MVS.Common.Models
{
    public partial class JobParticular
    {
        public JobParticular()
        {
            Answers = new HashSet<Answer>();
            VaultAnswersAnticipationMeasures = new HashSet<VaultAnswersAnticipationMeasure>();
            VaultAnswersHeritages = new HashSet<VaultAnswersHeritage>();
            VaultAnswersPersonals = new HashSet<VaultAnswersPersonal>();
        }

        public int Id { get; set; }
        public string Job { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<VaultAnswersAnticipationMeasure> VaultAnswersAnticipationMeasures { get; set; }
        public virtual ICollection<VaultAnswersHeritage> VaultAnswersHeritages { get; set; }
        public virtual ICollection<VaultAnswersPersonal> VaultAnswersPersonals { get; set; }
    }
}
