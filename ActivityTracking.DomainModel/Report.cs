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

        public int? UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Time> Times { get; set; }

        public virtual ICollection<Absenсe> Absenсes { get; set; }

        public Report()
        {
            Absenсes = new List<Absenсe>();
        }
    }
}
