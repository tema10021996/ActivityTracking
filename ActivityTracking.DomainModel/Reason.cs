using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracking.DomainModel
{
    public class Reason
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime AddingTime { get; set; }
        public string Color { get; set; }

        public virtual ICollection<DivisionManager> DivisionManagers { get; set; }
        public Reason()
        {
            DivisionManagers = new List<DivisionManager>();
        }
    }
}
