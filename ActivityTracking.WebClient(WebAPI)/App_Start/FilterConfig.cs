﻿using System.Web;
using System.Web.Mvc;

namespace ActivityTracking.WebClient_WebAPI_
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
