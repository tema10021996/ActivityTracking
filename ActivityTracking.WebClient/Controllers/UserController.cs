using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActivityTracking.WebClient.Models;
using ActivityTracking.DAL.EntityFramework;
using ActivityTracking.DomainModel;
using ActivityTracking.GetUserInfo;
using System.Collections;

namespace ActivityTracking.WebClient.Controllers
{
    public class UserController : Controller
    {

        public ActionResult Index()
        {
            DateTime End = DateTime.Now.AddDays(-1).Date;
            DateTime Start = DateTime.Now.AddDays(-7).Date;
            return Index(Start, End);
        }

        [HttpPost]
        public ActionResult Index(DateTime Start, DateTime End)
        {


            ApplicationContext context = new ApplicationContext();
            Repository<ApplicationUser> userRepository = new Repository<ApplicationUser>(context);
            var user = userRepository.GetList().First(u => u.UserName == HttpContext.User.Identity.Name);

            UserInfoModel info = GetUserInfo.UserInfo.GetUserInformation(null, user.UserName, Start, End).First();
            var times = info.WorkTimes.GroupBy(t => t.TimeIn.Date);

            Repository<Absence> absenceRepository = new Repository<Absence>(context);

            ShowUserReportViewModel model = new ShowUserReportViewModel { Start = Start, End = End, list = new List<ChartViewModel>(), UserInfo = info, DaysCount = (End - Start).Days };
            ArrayList colors = new ArrayList();

            for (DateTime start = Start; start <= End; start = start.AddDays(1))
            {
                var tempTimes = info.WorkTimes.Where(t => t.TimeIn.Date == start).ToArray();
                if (tempTimes.Length == 0)
                {
                    ChartViewModel notWorkThisDay = new ChartViewModel
                    {
                        RowLabel = start.Date.ToShortDateString(),
                        Barlabel = "Out of Work",
                        StartAbsence = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0),
                        EndAbsence = new DateTime(start.Year, start.Month, start.Day, 23, 59, 59),
                        Duration = new DateTime(start.Year, start.Month, start.Day, 23, 59, 59) - new DateTime(start.Year, start.Month, start.Day, 0, 0, 0),
                        Comment = null
                    };
                    if (!colors.Contains("#FFFFFF"))
                    {
                        colors.Add("#FFFFFF");
                    }
                    model.list.Add(notWorkThisDay);

                }

            }


            foreach (var timeGroup in times)
            {
                var timeGroupToArray = timeGroup.ToArray();

                DateTime FirstIn = timeGroupToArray[0].TimeIn;
                DateTime LastOut = timeGroupToArray[timeGroupToArray.Length - 1].TimeOut;

                var absences = absenceRepository.GetList().Where(a => a.User.UserName == user.UserName).Where(a => a.Date == timeGroupToArray[0].TimeIn.Date).ToArray();
                List<Absence> absencesOutOfBuilding = new List<Absence>();

                if (timeGroupToArray.Length > 1)
                {
                    for (int num = 0; num < timeGroupToArray.Length - 1; num++)
                    {
                        absencesOutOfBuilding.Add(new Absence
                        {
                            StartAbsence = new DateTime(timeGroupToArray[0].TimeIn.Year, timeGroupToArray[0].TimeIn.Month, timeGroupToArray[0].TimeIn.Day,
                                timeGroupToArray[num].TimeOut.Hour, timeGroupToArray[num].TimeOut.Minute, timeGroupToArray[num].TimeOut.Second),
                            EndAbsence = new DateTime(timeGroupToArray[0].TimeIn.Year, timeGroupToArray[0].TimeIn.Month, timeGroupToArray[0].TimeIn.Day,
                                timeGroupToArray[num + 1].TimeIn.Hour, timeGroupToArray[num + 1].TimeIn.Minute, timeGroupToArray[num + 1].TimeIn.Second),
                            Date = new DateTime(timeGroupToArray[0].TimeIn.Year, timeGroupToArray[0].TimeIn.Month, timeGroupToArray[0].TimeIn.Day),
                            Reason = new Reason { Name = "Out of Work", Color = "#FFFFFF" }
                        });
                    }
                }

                List<Absence> AllAbsences = new List<Absence>();
                foreach (var abs in absences)
                {
                    AllAbsences.Add(abs);
                }
                foreach (var abs in absencesOutOfBuilding)
                {
                    AllAbsences.Add(abs);
                }

                var AllAbsencesArray = AllAbsences.OrderBy(a => a.EndAbsence).ToArray();


                var tempStartBeforeWork = new DateTime(timeGroupToArray[0].TimeIn.Year, timeGroupToArray[0].TimeIn.Month, timeGroupToArray[0].TimeIn.Day, 0, 0, 0);
                var tempEndBeforeWork = new DateTime(timeGroupToArray[0].TimeIn.Year, timeGroupToArray[0].TimeIn.Month, timeGroupToArray[0].TimeIn.Day, FirstIn.Hour, FirstIn.Minute, FirstIn.Second);
                ChartViewModel beforeWork = new ChartViewModel
                {
                    RowLabel = timeGroupToArray[0].TimeIn.Date.ToShortDateString(),
                    Barlabel = "Out of Work",
                    StartAbsence = tempStartBeforeWork,
                    EndAbsence = tempEndBeforeWork,
                    Duration = tempEndBeforeWork - tempStartBeforeWork,
                    Comment = null
                };
                if (!colors.Contains("#FFFFFF"))
                {
                    colors.Add("#FFFFFF");
                }
                model.list.Add(beforeWork);

                var tempStartWorkTillFirstAbsence = new DateTime(timeGroupToArray[0].TimeIn.Year, timeGroupToArray[0].TimeIn.Month, timeGroupToArray[0].TimeIn.Day, FirstIn.Hour, FirstIn.Minute, FirstIn.Second);
                var tempEndWorkTillFirstAbsence = new DateTime(timeGroupToArray[0].TimeIn.Year, timeGroupToArray[0].TimeIn.Month, timeGroupToArray[0].TimeIn.Day,
                    AllAbsencesArray[0].StartAbsence.Hour, AllAbsencesArray[0].StartAbsence.Minute, AllAbsencesArray[0].StartAbsence.Second);
                ChartViewModel beginWorkTillFirstAbsence = new ChartViewModel
                {
                    RowLabel = timeGroupToArray[0].TimeIn.Date.ToShortDateString(),
                    Barlabel = "Work",
                    StartAbsence = tempStartWorkTillFirstAbsence,
                    EndAbsence = tempEndWorkTillFirstAbsence,
                    Duration = tempEndWorkTillFirstAbsence - tempStartWorkTillFirstAbsence,
                    Comment = null
                };
                if (!colors.Contains("#0000FF"))
                {
                    colors.Add("#0000FF");
                }
                model.list.Add(beginWorkTillFirstAbsence);

                for (int j = 0; j < AllAbsencesArray.Length; j++)
                {
                    DateTime endAbsence = (DateTime)AllAbsencesArray[j].EndAbsence;
                    ChartViewModel tv = new ChartViewModel
                    {
                        RowLabel = AllAbsencesArray[j].Date.ToShortDateString(),
                        Barlabel = AllAbsencesArray[j].Reason.Name,
                        StartAbsence = AllAbsencesArray[j].StartAbsence,
                        EndAbsence = endAbsence,
                        Duration = endAbsence - AllAbsencesArray[j].StartAbsence,
                        Comment = AllAbsencesArray[j].Comment
                    };
                    if (!colors.Contains(AllAbsencesArray[j].Reason.Color))
                    {
                        colors.Add(AllAbsencesArray[j].Reason.Color);
                    }
                    model.list.Add(tv);
                    if (j != AllAbsencesArray.Length - 1)
                    {
                        ChartViewModel gapTv = new ChartViewModel
                        {

                            RowLabel = AllAbsencesArray[j].Date.ToShortDateString(),
                            Barlabel = "Work",
                            StartAbsence = endAbsence,
                            EndAbsence = AllAbsencesArray[j + 1].StartAbsence,
                            Duration = AllAbsencesArray[j + 1].StartAbsence - endAbsence,
                            Comment = null
                        };
                        if (!colors.Contains("#0000FF"))
                        {
                            colors.Add("#0000FF");
                        }
                        model.list.Add(gapTv);
                    }
                    else
                    {
                        ChartViewModel gapTv = new ChartViewModel
                        {

                            RowLabel = AllAbsencesArray[j].Date.ToShortDateString(),
                            Barlabel = "Work",
                            StartAbsence = endAbsence,
                            EndAbsence = LastOut,
                            Duration = LastOut - endAbsence,
                            Comment = null
                        };
                        if (!colors.Contains("#0000FF"))
                        {
                            colors.Add("#0000FF");
                        }
                        model.list.Add(gapTv);
                    }

                }


                var tempStart = new DateTime(timeGroupToArray[0].TimeIn.Year, timeGroupToArray[0].TimeIn.Month, timeGroupToArray[0].TimeIn.Day, LastOut.Hour, LastOut.Minute, LastOut.Second);
                var tempEnd = new DateTime(timeGroupToArray[0].TimeIn.Year, timeGroupToArray[0].TimeIn.Month, timeGroupToArray[0].TimeIn.Day, 23, 59, 59);
                ChartViewModel afterWork = new ChartViewModel
                {
                    RowLabel = timeGroupToArray[0].TimeIn.Date.ToShortDateString(),
                    Barlabel = "Out of Work",
                    StartAbsence = tempStart,
                    EndAbsence = tempEnd,
                    Duration = tempEnd - tempStart,
                    Comment = null
                };
                if (!colors.Contains("#FFFFFF"))
                {
                    colors.Add("#FFFFFF");
                }
                model.list.Add(afterWork);

            }
            string dataStr = Newtonsoft.Json.JsonConvert.SerializeObject(colors, Newtonsoft.Json.Formatting.None);
            ViewBag.Colors = new HtmlString(dataStr);
            return View(model);
        }
    }
}