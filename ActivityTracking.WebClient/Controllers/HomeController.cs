using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActivityTracking.DomainModel;
using System.Web.UI.DataVisualization.Charting;
using ActivityTracking.DAL.EntityFramework;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace ActivityTracking.WebClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<Report> reports = new List<Report>();

            return View(reports);
        }

        public ActionResult Settings()
        {

               var groups = new Repository<Group>();
            
                SelectList groupList = new SelectList(groups.GetList(), "Id", "Name");
                ViewBag.groupslist = groupList;
               
                return View();
            
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}