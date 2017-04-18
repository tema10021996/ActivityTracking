using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivityTracking.WebClient.Models
{
    public class ChartViewModel
    {
        public string RowLabel { get; set; }
        public string Barlabel { get; set; }
        public string Comment { get; set; }
        public DateTime StartAbsence { get; set; }
        public DateTime EndAbsence { get; set; }
        public TimeSpan Duration { get; set; }
    }
}