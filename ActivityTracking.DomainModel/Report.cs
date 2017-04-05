using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracking.DomainModel
{
    public class Report
    {
        public int Id { get; set; }

        public int? JustUserId { get; set; }
        public JustUser JustUser { get; set; }

        public int? TimeId { get; set; }
        public Time Time { get; set; }

        public ICollection<Absenсe> Absenсes { get; set; }

        public Report()
        {
            Absenсes = new List<Absenсe>();
        }
    }
}
