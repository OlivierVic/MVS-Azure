using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartclause.SDK.DTO
{
    public class UserStats
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Generated { get; set; }
        public int Filled { get; set; }
        public int Validated { get; set; }
        public int Signed { get; set; }
        public TimeSpan TimeSpent { get; set; }
        public Dictionary<string, int> DirectionGenerations { get; set; }
    }
}
