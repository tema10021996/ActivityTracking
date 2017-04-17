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
        public string Tooltip { get; set; }
        public int StartHour { get; set; }
        public int StartMinute { get; set; }
        public int StartSecond { get; set; }
        public int EndHour { get; set; }
        public int EndMinute { get; set; }
        public int EndSecond { get; set; }
        public TimeSpan Duration { get; set; }
    }
}