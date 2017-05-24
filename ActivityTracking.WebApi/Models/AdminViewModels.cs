using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ActivityTracking.DomainModel;
using System.Web.Mvc;

namespace ActivityTracking.WebApi.Models
{
    public class AdminViewModels
    {
        public class AdminSettingsViewModel
        {
            public List<Reason> AllReasons { get; set; }
        }
    }
}