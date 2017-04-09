using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracking.Models
{
    public class JustUserModel
    {
        public int Id { get; set; }
        public string Login { get; set; }

        public int StatusId { get; set; }
    }
}
