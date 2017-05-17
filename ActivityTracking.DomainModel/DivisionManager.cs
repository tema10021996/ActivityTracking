using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracking.DomainModel
{
    public class DivisionManager
    {
        public int Id { get; set; }
        public string Login { get; set; }

        public virtual ICollection<Reason> Reasons { get; set; }

        public DivisionManager()
        {
            Reasons = new List<Reason>();
        }
    }
}
