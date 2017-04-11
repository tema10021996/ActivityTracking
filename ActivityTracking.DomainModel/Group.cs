using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracking.DomainModel
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan MayAbsentTime { get; set; }

        public virtual ICollection<Reason> Reasons { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

        public Group()
        {
            Users = new List<ApplicationUser>();
            Reasons = new List<Reason>();
        }
    }
}
