using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivityTracking.WebApi.Models
{
    public class PostModel
    {
        public DateTime Start { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
    }
}