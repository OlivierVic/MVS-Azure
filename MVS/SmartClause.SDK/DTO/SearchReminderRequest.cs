using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartclause.SDK.DTO
{
    public class SearchReminderRequest
    {
        /// <summary>
        /// Linked with AND : If defined, the the date from which the reminders starts
        /// </summary>
        public DateTime? beginDate;
        /// <summary>
        /// Linked with AND : If defined, the the date to which the reminders date end
        /// </summary>
        public DateTime? endDate;
        /// <summary>
        /// Linked with AND : If defined, search reminders with state equal
        /// </summary>
        public int? state;
        /// <summary>
        /// Linked with AND : If defined, search reminders with id equal
        /// </summary>
        public int? id;
        /// <summary>
        /// Linked with AND : If defined, search reminders with name containing the string
        /// </summary>
        public string nameLike;
        /// <summary>
        /// Linked with AND : If defined, search reminders with name equals to the string
        /// </summary>
        public string nameIs;
        /// <summary>
        /// Linked with AND : If defined, search reminders with description containing to the string
        /// </summary>
        public string DescriptionContains;
        /// <summary>
        /// A receiver Email
        /// </summary>
        public string HasReceiver;
        /// <summary>
        /// FieldId Is of the Reminder
        /// </summary>
        public string fieldIs;
    }
}
