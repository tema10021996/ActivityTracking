using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracking.Models
{
    public class ReasonModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime AddingTime { get; set; }

        public int UserWhoAddedId { get; set; }
    }
}
