using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracking.Models
{
    public class AbsenceModel
    {
        public int Id { get; set; }
        public DateTime StartAbsence { get; set; }
        public DateTime EndAbsence { get; set; }

        public int ReasonId { get; set; }

        public int JustUserId { get; set; }

        public int ReportId { get; set; }

        public DateTime Date { get; set; }
    }
}
