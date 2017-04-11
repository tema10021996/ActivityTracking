using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracking.DomainModel
{
    public class Time
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }

        public int? JustUserId { get; set; }
        public virtual JustUser JustUser { get; set; }
    }
}
