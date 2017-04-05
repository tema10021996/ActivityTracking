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
        public JustUser UserWhoAdded { get; set; }

        public ICollection<Group> Groups { get; set; }

        public Reason()
        {
            Groups = new List<Group>();
        }
    }
}
