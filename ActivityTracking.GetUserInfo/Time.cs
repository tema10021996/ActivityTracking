using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracking.GetUserInfo
{
    public class Time
    {
        public string Login { get; set; }
        public DateTime Date { get; set; }

        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }
    }
}
