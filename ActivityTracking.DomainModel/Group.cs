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

        public ICollection<Reason> Reasons { get; set; }

        public ICollection<JustUser> JustUsers { get; set; }

        public Group()
        {
            JustUsers = new List<JustUser>();
            Reasons = new List<Reason>();
        }
    }
}
