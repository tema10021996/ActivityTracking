using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ActivityTracking.DomainModel;
using System.Web.Mvc;

namespace ActivityTracking.WebClient.Models
{
    public class AdminViewModel
    {
        public IEnumerable<ApplicationUser> UsersList { get; set; }
        public SelectList GroupList { get; set; }
    }
}