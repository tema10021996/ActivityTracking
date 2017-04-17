using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActivityTracking.WebClient.Models;

namespace ActivityTracking.WebClient.Controllers
{
    public class UserController : Controller
    {
        
        public ActionResult Index()
        {
            //Chart Data
            ChartViewModel tv1 = new ChartViewModel { RowLabel = HttpContext.User.Identity.Name , Barlabel = "Out the work", StartHour = 0, StartMinute = 0, StartSecond = 0, EndHour = 8, EndMinute = 20, EndSecond = 0, Duration = new TimeSpan(8,20,0) };
            ChartViewModel tv2 = new ChartViewModel { RowLabel = HttpContext.User.Identity.Name , Barlabel = "Work", StartHour = 8, StartMinute = 20, StartSecond = 0, EndHour = 10, EndMinute = 42, EndSecond = 0, Duration = new TimeSpan(2, 22, 0) };
            ChartViewModel tv3 = new ChartViewModel { RowLabel = HttpContext.User.Identity.Name, Barlabel = "Meeting", StartHour = 10, StartMinute = 42, StartSecond = 0, EndHour = 11, EndMinute = 30, EndSecond = 0, Duration = new TimeSpan(0,48, 0) };
            ChartViewModel tv4 = new ChartViewModel { RowLabel = HttpContext.User.Identity.Name, Barlabel = "Work", StartHour = 11,StartMinute = 30, StartSecond = 0, EndHour = 13, EndMinute = 0, EndSecond = 0, Duration = new TimeSpan(1, 30 , 0), };
            ChartViewModel tv5 = new ChartViewModel { RowLabel = HttpContext.User.Identity.Name, Barlabel = "Dinner", StartHour = 13, StartMinute = 0, StartSecond = 0, EndHour = 13, EndMinute = 45, EndSecond = 0, Duration = new TimeSpan(0,45 , 0) };
            ChartViewModel tv6 = new ChartViewModel { RowLabel = HttpContext.User.Identity.Name, Barlabel = "Work", StartHour = 13, StartMinute = 45, StartSecond = 0, EndHour = 17, EndMinute = 30, EndSecond = 0, Duration = new TimeSpan(3,45 , 0) };
            ChartViewModel tv7 = new ChartViewModel { RowLabel = HttpContext.User.Identity.Name, Barlabel = "Out the work", StartHour = 17, StartMinute = 30, StartSecond = 0, EndHour = 24, EndMinute = 0, EndSecond = 0, Duration = new TimeSpan(6,30 , 0) };

            List<ChartViewModel> list = new List<ChartViewModel>();
            list.Add(tv1);
            list.Add(tv2);
            list.Add(tv3);
            list.Add(tv4);
            list.Add(tv5);
            list.Add(tv6);
            list.Add(tv7);
           
            return View(list);
        }
    }
}