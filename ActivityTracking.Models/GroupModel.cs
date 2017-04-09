using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracking.Models
{
    public class GroupModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan MayAbsentTime { get; set; }

        public ICollection<ReasonModel> Reasons { get; set; }

        public ICollection<JustUserModel> JustUsers { get; set; }

        public GroupModel()
        {
            JustUsers = new List<JustUserModel>();
            Reasons = new List<ReasonModel>();
        }
    }
}
