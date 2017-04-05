using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracking.DomainModel
{
    public class Absenсe
    {
        public int Id { get; set; }
        public DateTime StartAbsence { get; set; }
        public DateTime EndAbsence { get; set; }

        public int? ReasonId { get; set; }
        public Reason Reason { get; set; }

        public int? JustUserId { get; set; }
        public JustUser JustUser { get; set; }

        public int? ReportId { get; set; }
        public Report Report { get; set; }

        public DateTime Date { get; set; }

    }
}
