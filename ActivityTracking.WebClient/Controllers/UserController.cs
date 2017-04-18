using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActivityTracking.WebClient.Models;
using ActivityTracking.DAL.EntityFramework;
using ActivityTracking.DomainModel;

namespace ActivityTracking.WebClient.Controllers
{
    public class UserController : Controller
    {
        
        public ActionResult Index()
        {
            ApplicationContext context = new ApplicationContext();
            Repository<Time> timeRepository = new Repository<Time>(context);
            var times = timeRepository.GetList().Where(t => t.User.UserName == HttpContext.User.Identity.Name).ToArray();
            Repository<Absenсe> absenceRepository = new Repository<Absenсe>(context);
            
            List<ChartViewModel> list = new List<ChartViewModel>();

            for (int i = 0; i < times.Length; i++)
            {
                var absences = absenceRepository.GetList().Where(a => a.User.UserName == HttpContext.User.Identity.Name).Where(a=>a.Date == times[i].Date).ToArray();

                var tempStartAbsence = new DateTime(times[i].Date.Year, times[i].Date.Month, times[i].Date.Day, 0, 0, 0);
                var tempEndAbsence = new DateTime(times[i].Date.Year, times[i].Date.Month, times[i].Date.Day, absences[0].StartAbsence.Hour, absences[0].StartAbsence.Minute, absences[0].StartAbsence.Second);
                ChartViewModel beginTv = new ChartViewModel
                { RowLabel = HttpContext.User.Identity.Name + " " + times[i].Date.ToShortDateString(),
                    Barlabel = "Out of Work",
                    StartAbsence = tempStartAbsence,
                    EndAbsence = tempEndAbsence,
                    Duration = tempEndAbsence - tempStartAbsence,
                    Comment = null
                };
                list.Add(beginTv);

                for (int j = 0; j < absences.Length; j++)
                {
                    DateTime endAbsence = (DateTime)absences[j].EndAbsence;
                    ChartViewModel tv = new ChartViewModel
                    {
                        RowLabel = HttpContext.User.Identity.Name + " " + absences[j].Date.ToShortDateString(),
                        Barlabel = absences[j].Reason.Name,
                        StartAbsence = absences[j].StartAbsence,
                        EndAbsence = endAbsence,
                        Duration = endAbsence - absences[j].StartAbsence,
                        Comment = absences[j].Comment
                    };
                    list.Add(tv);
                    if (j != absences.Length - 1)
                    {
                        ChartViewModel gapTv = new ChartViewModel
                        {

                            RowLabel = HttpContext.User.Identity.Name + " " + absences[j].Date.ToShortDateString(),
                            Barlabel = "Work",
                            StartAbsence = endAbsence,
                            EndAbsence = absences[j + 1].StartAbsence,
                            Duration = absences[j + 1].StartAbsence - endAbsence,
                            Comment = null
                        };
                        list.Add(gapTv);
                    }

                }
                DateTime endAbsenceforEndTv = (DateTime)absences[absences.Length - 1].EndAbsence;

                var tempStart = new DateTime(times[i].Date.Year, times[i].Date.Month, times[i].Date.Day, endAbsenceforEndTv.Hour, endAbsenceforEndTv.Minute, endAbsenceforEndTv.Second);
                var tempEnd = new DateTime(times[i].Date.Year, times[i].Date.Month, times[i].Date.Day, 23, 59, 59);
                ChartViewModel endTv = new ChartViewModel
                {
                    RowLabel = HttpContext.User.Identity.Name + " " + times[i].Date.ToShortDateString(),
                    Barlabel = "Out of Work",
                    StartAbsence = tempStart,
                    EndAbsence = tempEnd,
                    Duration = tempEnd - tempStart,
                    Comment = null
                };
                list.Add(endTv);
                
            }


            return View(list);
        }
    }
}