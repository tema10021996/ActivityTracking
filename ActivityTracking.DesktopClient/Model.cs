using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracking.DesktopClient
{
    public class PostModel
    {
        public DateTime StartAbsence { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
    }

    public class PutModel
    {
        public DateTime EndAbsence { get; set; }
        public string UserName { get; set; }
        public string ReasonName { get; set; }
        public string Comment { get; set; }
    }
}
