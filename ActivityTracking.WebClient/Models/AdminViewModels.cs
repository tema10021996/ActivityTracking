﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ActivityTracking.DomainModel;
using System.Web.Mvc;

namespace ActivityTracking.WebClient.Models
{
    public class AdminSettingsViewModel
    {
        public List<Reason> AllReasons { get; set; }
    }

}