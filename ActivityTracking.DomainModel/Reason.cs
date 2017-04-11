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

        public int? UserWhoAddedId { get; set; }
        public virtual JustUser UserWhoAdded { get; set; }

        public virtual ICollection<Group> Groups { get; set; }

        public Reason()
        {
            Groups = new List<Group>();
        }
    }
}
