using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActivityTracking.DomainModel;
using System.Web.UI.DataVisualization.Charting;
using ActivityTracking.GetReportInfo;

namespace ActivityTracking.WebClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<Report> reports = new List<Report>();
            ViewBag.Message = new ReportInfo().GetUserInfo();

                return View(reports);
        }

        public ActionResult Settings()
        {
            using (var groups = new ActivityTracking.DAL.EntityFramework.GroupDomainModel())
            {
                return View(groups.GetAll());
            }
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}