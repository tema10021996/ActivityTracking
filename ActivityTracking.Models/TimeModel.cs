using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracking.Models
{
    public class TimeModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }        
    }
}
