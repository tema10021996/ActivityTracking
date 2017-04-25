using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracking.DomainModel
{
    public class Absence
    {
        public int Id { get; set; }
        public DateTime StartAbsence { get; set; }
        public DateTime? EndAbsence { get; set; }
        public string Comment { get; set; }

        public int? ReasonId { get; set; }
        public virtual Reason Reason { get; set; }

        public int? UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public DateTime Date { get; set; }

    }
}
