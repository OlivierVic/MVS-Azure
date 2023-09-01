using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartclause.SDK
{
    public class ReminderReceiver
    {
        public int? Id;
        /// <summary>
        /// Reminder Id
        /// </summary>
        public int? reminderId;

        /// <summary>
        /// The receiver email
        /// </summary>
        public string email;

        /// <summary>
        /// The activation's receiver boolean
        /// </summary>
        public bool? activated;
    }
}
