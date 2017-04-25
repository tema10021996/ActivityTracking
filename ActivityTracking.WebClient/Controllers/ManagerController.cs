using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActivityTracking.DAL.EntityFramework;
using ActivityTracking.DomainModel;
using ActivityTracking.WebClient.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections;
using ActivityTracking.GetUserInfo;

namespace ActivityTracking.WebClient.Controllers
{
    public class ManagerController : Controller
    {

        public ActionResult Index()
        {
            DateTime End = DateTime.Now.AddDays(-1).Date;
            DateTime Start = DateTime.Now.AddDays(-7).Date;
            Repository<Group> groupReposotiry = new Repository<Group>();
            var groups = groupReposotiry.GetList();
            Group group = null;
            foreach (var gr in groups)
            {
                foreach (var user in gr.Users)
                {
                    if (user.UserName == HttpContext.User.Identity.Name)
                    {
                        group = user.Group;
                        break;
                    }
                }
            }
            if (group == null)
            {
                return HttpNotFound();
            }

            return Index(Start, End);
        }


        [HttpPost]
        public ActionResult Index(DateTime Start, DateTime End)
        {
            ApplicationContext context = new ApplicationContext();
            Repository<Group> groupReposotiry = new Repository<Group>(context);
            Repository<Absence> absenceRepository = new Repository<Absence>(context);
            //Repository<Time> timeRepository = new Repository<Time>(context);
            ArrayList colors = new ArrayList();

            var groups = groupReposotiry.GetList();
            Group group = null;
            foreach (var gr in groups)
            {
                foreach (var user in gr.Users)
                {
                    if (user.UserName == HttpContext.User.Identity.Name)
                    {
                        group = user.Group;
                        break;
                    }
                }
            }
            if (group == null)
            {
                return HttpNotFound();
            }

            List<UserInfoModel> usersInfo = GetUserInfo.UserInfo.GetUserInformation(group.Name, null, Start, End);

            ManagerPostIndexViewModel viewModel = new ManagerPostIndexViewModel { Start = Start, End = End, ReasonsNames = new List<string>(),  WorkersInfos = new List<WorkerInfo>() {} };

            var groupReasons = group.Reasons;
            viewModel.ReasonsNames.Add("Work");
            if (!colors.Contains("#0000FF"))
            {
                colors.Add("#0000FF");
            }
            foreach (var reason in groupReasons)
            {               
                viewModel.ReasonsNames.Add(reason.Name);
            }

            foreach (var user in group.Users.OrderBy(u => u.UserName))
            {                
                
                WorkerInfo workerInfo = new WorkerInfo { Name = user.UserName, ReasonInfos = new List<ReasonInfo>() };
                var userAbsences = absenceRepository.GetList().Where(a => a.User.Id == user.Id).Where(ab => ab.Date.Date >= Start.Date && ab.Date <= End.Date);

                TimeSpan workDurationForGivenDays = new TimeSpan(0, 0, 0);

                //var userTimesByDays = UsersInfo.Where(t => t.Date.Date >= Start.Date && t.Date <= End.Date).Where(t=>t.User.Id == user.Id).GroupBy(t => t.Date);
                var userInfo = usersInfo.First();
                var userTimesByDays = userInfo.WorkTimes.GroupBy(t => t.TimeIn.Date);

                foreach (var userTimesForOneDay in userTimesByDays)
                {
                    var userTimesForOneDayToArray = userTimesForOneDay.ToArray();
                    var userAbsencesforOneDayToArray = userAbsences.Where(a => a.Date.Date == userTimesForOneDayToArray[0].TimeIn.Date).ToArray();

                    DateTime FirstIn = userTimesForOneDayToArray[0].TimeIn;
                    DateTime LastOut = userTimesForOneDayToArray[userTimesForOneDayToArray.Length - 1].TimeOut;


                        //--------------------------
                        List<Absence> absencesOutOfBuildingForOneDay = new List<Absence>();

                        if (userTimesForOneDayToArray.Length > 1)
                        {

                            for (int num = 0; num < userTimesForOneDayToArray.Length - 1; num++)
                            {
                                absencesOutOfBuildingForOneDay.Add(new Absence
                                {
                                    StartAbsence = new DateTime(userTimesForOneDayToArray[0].TimeIn.Year, userTimesForOneDayToArray[0].TimeIn.Month, userTimesForOneDayToArray[0].TimeIn.Day,
                                        userTimesForOneDayToArray[num].TimeOut.Hour, userTimesForOneDayToArray[num].TimeOut.Minute, userTimesForOneDayToArray[num].TimeOut.Second),
                                    EndAbsence = new DateTime(userTimesForOneDayToArray[0].TimeIn.Year, userTimesForOneDayToArray[0].TimeIn.Month, userTimesForOneDayToArray[0].TimeIn.Day,
                                        userTimesForOneDayToArray[num + 1].TimeIn.Hour, userTimesForOneDayToArray[num + 1].TimeIn.Minute, userTimesForOneDayToArray[num + 1].TimeIn.Second),
                                    Date = new DateTime(userTimesForOneDayToArray[0].TimeIn.Year, userTimesForOneDayToArray[0].TimeIn.Month, userTimesForOneDayToArray[0].TimeIn.Day)
                                });
                            }

                        }

                        List<Absence> AllAbsencesForOneDay = new List<Absence>();
                    foreach (var abs in userAbsencesforOneDayToArray)
                    {
                        AllAbsencesForOneDay.Add(abs);
                    }
                        foreach (var abs in absencesOutOfBuildingForOneDay)
                        {
                            AllAbsencesForOneDay.Add(abs);
                        }

                        var AllAbsencesForOneDaySorted = AllAbsencesForOneDay.OrderBy(a => a.StartAbsence);
                        //-----------------------

                        TimeSpan TimeInBuildingForOneDay = LastOut - FirstIn;
                    TimeSpan AllAbsencesDurationForOneDay = new TimeSpan(0, 0, 0);
                    foreach (Absence absence in AllAbsencesForOneDay)
                    {
                        AllAbsencesDurationForOneDay += ((DateTime)absence.EndAbsence - absence.StartAbsence);
                    }
                    TimeSpan WorkDurationForOneDay = TimeInBuildingForOneDay - AllAbsencesDurationForOneDay;

                    workDurationForGivenDays += WorkDurationForOneDay;
                }

               
                    workerInfo.ReasonInfos.Add(new ReasonInfo
                    {
                        ReasonName = "Work",
                        DurationInHours = workDurationForGivenDays.TotalHours,
                        Hours = workDurationForGivenDays.Hours,
                        Minutes = workDurationForGivenDays.Minutes,
                        Seconds = workDurationForGivenDays.Seconds
                    });
                
                
                foreach (var reason in group.Reasons)
                {
                    TimeSpan reasonTotalTime = new TimeSpan(0, 0, 0);
                    var reasonAbsences = userAbsences.Where(a => a.Reason.Id == reason.Id);
                    foreach (var absence in reasonAbsences)
                    {
                        reasonTotalTime += ((DateTime)absence.EndAbsence - absence.StartAbsence);
                    }

                    workerInfo.ReasonInfos.Add(new ReasonInfo
                    {
                        ReasonName = reason.Name,
                        DurationInHours = reasonTotalTime.TotalHours,
                        Hours = reasonTotalTime.Hours,
                        Minutes = reasonTotalTime.Minutes,
                        Seconds = reasonTotalTime.Seconds
                    });
                    if (!colors.Contains(reason.Color))
                    {
                        colors.Add(reason.Color);
                    }
                }

                viewModel.WorkersInfos.Add(workerInfo);
            

             }

            string dataStr = Newtonsoft.Json.JsonConvert.SerializeObject(colors, Newtonsoft.Json.Formatting.None);
            ViewBag.Colors = new HtmlString(dataStr);

            return View(viewModel);
        }

        public ActionResult ShowUsers()
        {
            Repository<ApplicationUser> userRepository = new Repository<ApplicationUser>();
            var user = userRepository.GetList().First(u => u.UserName == HttpContext.User.Identity.Name);
            Group group = user.Group;
            return View(group);
        }
        public ActionResult ChangeGroupMayAbsentTime(int Id, string Minutes)
        {
            Repository<Group> groupRepository = new Repository<Group>();
            var group = groupRepository.GetList().First(g=>g.Id == Id);
            group.MayAbsentTime = new TimeSpan(0, Convert.ToInt32(Minutes), 0);
            groupRepository.Update(group);
            return RedirectToAction("ShowUsers");
        }

        public ActionResult ShowUserReport(string Id)
        {
            DateTime End = DateTime.Now.AddDays(-1).Date;
            DateTime Start = DateTime.Now.AddDays(-8).Date;
            return ShowUserReport(Id, Start, End);
        }

        [HttpPost]
        public ActionResult ShowUserReport(string Id, DateTime Start, DateTime End)
        {
           

            ApplicationContext context = new ApplicationContext();
            Repository<ApplicationUser> userRepository = new Repository<ApplicationUser>(context);
             var user = userRepository.GetList().First(u => u.Id == Id);

            UserInfoModel info = GetUserInfo.UserInfo.GetUserInformation(null, user.UserName, Start, End).First();
            
            var times = info.WorkTimes.GroupBy(t=>t.TimeIn.Date);
            Repository<Absence> absenceRepository = new Repository<Absence>(context);

            ShowUserReportViewModel model = new ShowUserReportViewModel {Start = Start, End = End, list = new List<ChartViewModel>(), UserInfo = info };
            ArrayList colors = new ArrayList();

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

                var  AllAbsencesArray = AllAbsences.OrderBy(a=>a.EndAbsence).ToArray();


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
                if (AllAbsencesArray.Length > 0)
                {
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
                }
                else
                {
                    var tempEndWork = new DateTime(timeGroupToArray[0].TimeIn.Year, timeGroupToArray[0].TimeIn.Month, timeGroupToArray[0].TimeIn.Day,
                        timeGroupToArray[0].TimeOut.Hour, timeGroupToArray[0].TimeOut.Minute, timeGroupToArray[0].TimeOut.Second);
                    ChartViewModel beginWorkTillFirstAbsence = new ChartViewModel
                    {
                        RowLabel = timeGroupToArray[0].TimeIn.Date.ToShortDateString(),
                        Barlabel = "Work",
                        StartAbsence = tempStartWorkTillFirstAbsence,
                        EndAbsence = tempEndWork,
                        Duration = tempEndWork - tempStartWorkTillFirstAbsence,
                        Comment = null
                    };
                    if (!colors.Contains("#0000FF"))
                    {
                        colors.Add("#0000FF");
                    }
                    model.list.Add(beginWorkTillFirstAbsence);
                }
               

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

                            RowLabel =  AllAbsencesArray[j].Date.ToShortDateString(),
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
                //DateTime LastAbsenceEndTime = (DateTime)absences[absences.Length - 1].EndAbsence;

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
            ViewBag.User = user.UserName;
            return View(model);
        }

    }
}