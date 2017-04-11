using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracking.DomainModel
{
    public class JustUser
    {
        public int Id { get; set; }
        public string Login { get; set; }

        public int? StatusId { get; set; }
        public virtual Status Status { get; set; }
 
        public virtual ICollection<Group> Groups { get; set; }

        public JustUser()
        {
            Groups = new List<Group>();
        }
    }
}
