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
using ActivityTracking.WebApi.Models;

namespace ActivityTracking.WebApi.Controllers
{
    public class ManagerController : Controller
    {
        #region Index
        public ActionResult Index()
        {
            List<string> managerDepartmentNames = GetUserInfo.UserInfo.GetManagerDepartments(HttpContext.User.Identity.Name);

            return View(managerDepartmentNames);
        }
        #endregion

        #region ChooseDepartment
        public ActionResult ChooseDepartment()
        {
            ChooseDepartmentViewModel chooseDepartmentViewModel = new ChooseDepartmentViewModel();
            chooseDepartmentViewModel.DepartmentList = new SelectList(UserInfo.GetManagerDepartments(HttpContext.User.Identity.Name));
            return View(chooseDepartmentViewModel);
        }
        #endregion

        #region Settings
        public ActionResult Settings()
        {
            ManagerSettingsViewModel managerSettingsViewModel = new ManagerSettingsViewModel {AllReasonModels = new List<ReasonModel>() };
            Repository<DivisionManager> divisionManagerRepository = new Repository<DivisionManager>();
            Repository<Reason> reasonRepository = new Repository<Reason>();
            DivisionManager divisionManager = divisionManagerRepository.GetList().First(m => m.Login == User.Identity.Name);
            var allReasons = reasonRepository.GetList();
            
            foreach (var reason in allReasons)
            {
                bool divManagerHasThisReason = false;
                foreach (var divisionManagerReason in divisionManager.Reasons)
                {
                    if (divisionManagerReason.Name == reason.Name)
                    {
                        divManagerHasThisReason = true;
                    }
                }
                if (divManagerHasThisReason == true)
                {
                    managerSettingsViewModel.AllReasonModels.Add(new ReasonModel { Reason = reason, isChoosen = true });
                }
                else
                {
                    managerSettingsViewModel.AllReasonModels.Add(new ReasonModel { Reason = reason, isChoosen = false });
                }
            }

            return View(managerSettingsViewModel);
        }
        #endregion

        #region Settings [HttpPost]
        [HttpPost]
        public ActionResult Settings(ManagerSettingsViewModel managerSettingsViewModel)
        {
            ApplicationContext context = new ApplicationContext();
            Repository<Reason> reasonRepository = new Repository<Reason>(context);
            Repository<DivisionManager> divisionManagerRepository = new Repository<DivisionManager>(context);
            DivisionManager divisionManager = divisionManagerRepository.GetList().First(m => m.Login == User.Identity.Name);
            divisionManager.Reasons.Clear();
            foreach (var reasonModel in managerSettingsViewModel.AllReasonModels)
            {
                if (reasonModel.isChoosen == true)
                {
                    var reason = reasonRepository.GetItem(reasonModel.Reason.Id);
                    divisionManager.Reasons.Add(reason);
                }
            }
            divisionManagerRepository.Update(divisionManager);
            return View(managerSettingsViewModel);
        }
        #endregion

        #region ShowDepartmentReportWithValidation
        [HttpPost]
        public ActionResult ShowDepartmentReportWithValidation(string DepartmentList, DateTime? Start, DateTime? End, bool BarChart, bool PieChart, bool ColumnChart)
        {
            if (Start == null || End == null)
            {
                TempData["message"] = "You should enter two dates";
                End = DateTime.Now.AddDays(-1).Date;
                Start = DateTime.Now.AddDays(-7).Date;
                
                return ShowDepartmentReport(DepartmentList, (DateTime)Start, (DateTime)End, BarChart, PieChart, ColumnChart);

            }
            else if (Start > End)
            {
                TempData["message"] = "End date should be bigger then start date";
                End = DateTime.Now.AddDays(-1).Date;
                Start = DateTime.Now.AddDays(-7).Date;
                return ShowDepartmentReport(DepartmentList, (DateTime)Start, (DateTime)End, BarChart, PieChart, ColumnChart);
            }
            else
            {
                return ShowDepartmentReport(DepartmentList, (DateTime)Start, (DateTime)End, BarChart, PieChart, ColumnChart);
            }
 
        }
        #endregion


        #region ShowDepartmentReport
        [HttpPost]
        public ActionResult ShowDepartmentReport(string DepartmentList, DateTime Start, DateTime End, bool BarChart, bool PieChart, bool ColumnChart)
        {
            ArrayList colors = new ArrayList();

            
            ManagerShowDepartmentReportViewModel managerShowDepartmentReportViewModel = new ManagerShowDepartmentReportViewModel()
            { Start = Start, End = End, ReasonsNames = new List<string>(), ReasonInfos = GenerateDataForDepartmentReportInPercentage(DepartmentList, Start, End), ChosenDepartmentName = DepartmentList, BarChart = BarChart, PieChart = PieChart, ColumnChart = ColumnChart };

            Repository<Absence> absenceRepository = new Repository<Absence>();
            Repository<Reason> reasonRepository = new Repository<Reason>();
            var reasons = reasonRepository.GetList();
            managerShowDepartmentReportViewModel.ReasonsNames.Add("Work");
            if (!colors.Contains("#0000FF"))
            {
                colors.Add("#0000FF");
            }
            foreach (var reason in reasons)
            {
                managerShowDepartmentReportViewModel.ReasonsNames.Add(reason.Name);
                if (!colors.Contains(reason.Color))
                {
                    colors.Add(reason.Color);
                }
            }

           
            string dataStr = Newtonsoft.Json.JsonConvert.SerializeObject(colors, Newtonsoft.Json.Formatting.None);
            ViewBag.Colors = new HtmlString(dataStr);
            return View("ShowDepartmentReport", managerShowDepartmentReportViewModel);
        }
        #endregion


        #region GenerateDataForDepartmentReportInPercentage
        private List<ReasonInfo> GenerateDataForDepartmentReportInPercentage(string DepartmentList, DateTime Start, DateTime End)
        {
            List<ReasonInfo> ReasonInfos = new List<ReasonInfo>();
            Repository<Absence> absenceRepository = new Repository<Absence>();
            Repository<Reason> reasonRepository = new Repository<Reason>();
            var reasons = reasonRepository.GetList();
            var departmentUserInfoModels = UserInfo.GetUserOrDepartmentIformation(DepartmentList, null, Start, End);
            TimeSpan workDurationForGivenDays = new TimeSpan(0, 0, 0);
            foreach (var userInfoModel in departmentUserInfoModels.OrderBy(u => u.userInformarion.Login))
            {

                var userAbsences = absenceRepository.GetList().Where(a => a.Date >= Start && a.Date <= End).Where(a => a.User.UserName == userInfoModel.userInformarion.Login);



                if (userInfoModel.WorkTimes != null)
                {
                    foreach (var userTimesForOneDay in userInfoModel.WorkTimes.GroupBy(t => t.TimeIn.Date))
                    {

                        var userTimesForOneDayToArray = userTimesForOneDay.ToArray();
                        var userAbsencesforOneDayToArray = userAbsences.Where(a => a.Date.Date == userTimesForOneDayToArray[0].TimeIn.Date).ToArray();


                        workDurationForGivenDays += CalculateWorkDuartionForOneUserForOneDay(userTimesForOneDayToArray, userAbsencesforOneDayToArray);
                    }
                }
            }
            string Workhours = workDurationForGivenDays.TotalHours.ToString();
            int Workindex = Workhours.IndexOf(',');
            int WorkresultHours = workDurationForGivenDays.Hours;
            if (Workindex == -1)
            {
                WorkresultHours = Convert.ToInt32(workDurationForGivenDays.TotalHours);
            }
            else
            {
                WorkresultHours = Convert.ToInt32(Workhours.Remove(Workindex));
            }
            ReasonInfos.Add(new ReasonInfo
            {
                ReasonName = "Work",
                DurationInHours = workDurationForGivenDays.TotalHours,
                Hours = WorkresultHours,
                Minutes = workDurationForGivenDays.Minutes,
                Seconds = workDurationForGivenDays.Seconds,
                Color = "#0000FF"

            });
            foreach (var reason in reasonRepository.GetList())
            {
                TimeSpan reasonDuration = new TimeSpan(0, 0, 0);
                foreach (var userInfoModel in departmentUserInfoModels)
                {
                    var userAbsences = absenceRepository.GetList().Where(a => a.Date >= Start && a.Date <= End).Where(a => a.User.UserName == userInfoModel.userInformarion.Login).Where(a => a.Reason.Name == reason.Name);
                    foreach (var absence in userAbsences)
                    {
                        reasonDuration += ((DateTime)absence.EndAbsence - absence.StartAbsence);
                    }
                }


                string hours = reasonDuration.TotalHours.ToString();
                int index = hours.IndexOf(',');
                int resultHours = reasonDuration.Hours;
                if (index == -1)
                {
                    resultHours = Convert.ToInt32(reasonDuration.TotalHours);
                }
                else
                {
                    resultHours = Convert.ToInt32(hours.Remove(index));
                }
                ReasonInfos.Add(new ReasonInfo
                {
                    ReasonName = reason.Name,
                    DurationInHours = reasonDuration.TotalHours,
                    Hours = resultHours,
                    Minutes = reasonDuration.Minutes,
                    Seconds = reasonDuration.Seconds,
                    Color = reason.Color
                });
                

            }

            return ReasonInfos;
        }
        #endregion
    

        #region CalculateWorkDuartionForOneUserForOneDay
        private TimeSpan CalculateWorkDuartionForOneUserForOneDay(WorkTime[] userTimesForOneDayToArray, Absence[] userAbsencesforOneDayToArray)
        {
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

            return WorkDurationForOneDay;
        }
        #endregion

        #region ShowMyReport
        public ActionResult ShowMyReport()
        {
            DateTime End = DateTime.Now.AddDays(-1).Date;
            DateTime Start = DateTime.Now.AddDays(-7).Date;
            return ShowUserReport( HttpContext.User.Identity.Name,Start, End);
        }
        #endregion

        #region ShowMyReport [HttpPost]
        [HttpPost]
        public ActionResult ShowMyReport(string userName, DateTime Start, DateTime End)
        {
            return ShowUserReport(HttpContext.User.Identity.Name, Start, End);
        }
        #endregion

        #region ShowDepartmentReportByUsersWithValidation

        public ActionResult ShowDepartmentReportByUsersWithValidation(string departmentName, DateTime? Start, DateTime? End)
        {
            if (Start == null || End == null)
            {
                TempData["message"] = "You should enter two dates";
                End = DateTime.Now.AddDays(-1).Date;
                Start = DateTime.Now.AddDays(-7).Date;

                return ShowDepartmentReportByUsers(departmentName, (DateTime)Start, (DateTime)End);

            }
            else if (Start > End)
            {
                TempData["message"] = "End date should be bigger then start date";
                End = DateTime.Now.AddDays(-1).Date;
                Start = DateTime.Now.AddDays(-7).Date;
                return ShowDepartmentReportByUsers(departmentName, (DateTime)Start, (DateTime)End);
            }
            else
            {
                return ShowDepartmentReportByUsers(departmentName, (DateTime)Start, (DateTime)End);
            }

        }
        #endregion

        #region ShowDepartmentReportByUsers
        [HttpPost]
        public ActionResult ShowDepartmentReportByUsers(string departmentName, DateTime Start, DateTime End)
        {
            Repository<Absence> absenceRepository = new Repository<Absence>();
            Repository<ApplicationUser> applicationUserRepository = new Repository<ApplicationUser>();
            ArrayList colors = new ArrayList();

            List<UserInfoModel> usersInDepartmentInfoModels = GetUserInfo.UserInfo.GetUserOrDepartmentIformation(departmentName, null, Start, End);

            ManagerShowDepartmentReportByUsersViewModel viewModel = new ManagerShowDepartmentReportByUsersViewModel { Start = Start, End = End, ReasonsNames = new List<string>(),
                WorkersInfos = new List<WorkerInfo>() { }, ReasonInfosForPercentageReport = GenerateDataForDepartmentReportInPercentage(departmentName, Start, End),
                ChosenDepartmentName = departmentName};

            Repository<Reason> reasonRepository = new Repository<Reason>();
            var reasons = reasonRepository.GetList();
            viewModel.ReasonsNames.Add("Work");
            if (!colors.Contains("#0000FF"))
            {
                colors.Add("#0000FF");
            }
            foreach (var reason in reasons)
            {
                viewModel.ReasonsNames.Add(reason.Name);
            }



            foreach (var userInfoModel in usersInDepartmentInfoModels.OrderBy(u=>u.userInformarion.Login))
            {

                var user = applicationUserRepository.GetList().First(u => u.UserName == userInfoModel.userInformarion.Login);

                WorkerInfo workerInfo = new WorkerInfo { Name = user.UserName, ReasonInfos = new List<ReasonInfo>() };
                var userAbsences = absenceRepository.GetList().Where(a => a.User.Id == user.Id).Where(ab => ab.Date.Date >= Start.Date && ab.Date <= End.Date);

                TimeSpan workDurationForGivenDays = new TimeSpan(0, 0, 0);
                
                if (userInfoModel.WorkTimes != null)
                {
                    foreach (var userTimesForOneDay in userInfoModel.WorkTimes.GroupBy(t => t.TimeIn.Date))
                    {

                        var userTimesForOneDayToArray = userTimesForOneDay.ToArray();
                        var userAbsencesforOneDayToArray = userAbsences.Where(a => a.Date.Date == userTimesForOneDayToArray[0].TimeIn.Date).ToArray();
 

                        workDurationForGivenDays += CalculateWorkDuartionForOneUserForOneDay(userTimesForOneDayToArray, userAbsencesforOneDayToArray);
                    }
                }
                string Workhours = workDurationForGivenDays.TotalHours.ToString();
                int Workindex = Workhours.IndexOf(',');
                int WorkresultHours = workDurationForGivenDays.Hours;              
                if (Workindex == -1)
                {
                    WorkresultHours = Convert.ToInt32(workDurationForGivenDays.TotalHours);
                }
                else
                {
                    WorkresultHours = Convert.ToInt32(Workhours.Remove(Workindex));
                }
                workerInfo.ReasonInfos.Add(new ReasonInfo
                {
                    ReasonName = "Work",
                    DurationInHours = workDurationForGivenDays.TotalHours,
                    Hours = WorkresultHours,
                    Minutes = workDurationForGivenDays.Minutes,
                    Seconds = workDurationForGivenDays.Seconds
                });


                foreach (var reason in reasonRepository.GetList())
                {
                    TimeSpan reasonTotalTime = new TimeSpan(0, 0, 0);
                    var reasonAbsences = userAbsences.Where(a => a.Reason.Id == reason.Id);
                    foreach (var absence in reasonAbsences)
                    {
                        reasonTotalTime += ((DateTime)absence.EndAbsence - absence.StartAbsence);
                    }
                    string hours = reasonTotalTime.TotalHours.ToString();
                    int index = hours.IndexOf(',');
                    int resultHours = reasonTotalTime.Hours;
                    if (index == -1)
                    {
                        resultHours = Convert.ToInt32(reasonTotalTime.TotalHours);
                    }
                    else
                    {
                        resultHours = Convert.ToInt32(hours.Remove(index));
                    }
                    workerInfo.ReasonInfos.Add(new ReasonInfo
                    {
                        ReasonName = reason.Name,
                        DurationInHours = reasonTotalTime.TotalHours,
                        Hours = resultHours,
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

            return View("ShowDepartmentReportByUsers",viewModel);
        }
        #endregion

        #region ShowDepartmentUsers
        public ActionResult ShowDepartmentUsers(string departmentName)
        {
            ManagerShowUsersViewModel managerShowUsersViewModel = new ManagerShowUsersViewModel();

            var userInfoModels = GetUserInfo.UserInfo.GetUserOrDepartmentIformation(departmentName, null, DateTime.Today, DateTime.Today);
            List<string> userNames = new List<string>();
            foreach (var userInfoModel in userInfoModels)
            {
                userNames.Add(userInfoModel.userInformarion.Login);
            }
            managerShowUsersViewModel.UsersNames = userNames;
            managerShowUsersViewModel.DepartmentName = departmentName;
            return View(managerShowUsersViewModel);
        }
        #endregion

        //#region ChangeGroupMayAbsentTime
        //public ActionResult ChangeGroupMayAbsentTime(int Id, string Minutes)
        //{
        //    Repository<Group> groupRepository = new Repository<Group>();
        //    var group = groupRepository.GetList().First(g=>g.Id == Id);
        //    group.MayAbsentTime = new TimeSpan(0, Convert.ToInt32(Minutes), 0);
        //    groupRepository.Update(group);
        //    return RedirectToAction("ShowUsers");
        //}
        //#endregion

        #region ShowUserReportWithValidation
        public ActionResult ShowUserReportWithValidation(string userName, DateTime? Start, DateTime? End)
        {
            if (Start == null || End == null)
            {
                TempData["message"] = "You should enter two dates";
                End = DateTime.Now.AddDays(-1).Date;
                Start = DateTime.Now.AddDays(-7).Date;

                return ShowUserReport(userName, (DateTime)Start, (DateTime)End);

            }
            else if (Start > End)
            {
                TempData["message"] = "End date should be bigger then start date";
                End = DateTime.Now.AddDays(-1).Date;
                Start = DateTime.Now.AddDays(-7).Date;
                return ShowUserReport(userName, (DateTime)Start, (DateTime)End);
            }
            else
            {
                return ShowUserReport(userName, (DateTime)Start, (DateTime)End);
            }
        }
        #endregion

        #region ShowUserReport [HttpPost]
        [HttpPost]
        public ActionResult ShowUserReport(string userName, DateTime Start, DateTime End)
        {

            ApplicationContext context = new ApplicationContext();
            Repository<ApplicationUser> userRepository = new Repository<ApplicationUser>(context);
            var user = userRepository.GetList().First(u => u.UserName == userName);

            UserInfoModel info = GetUserInfo.UserInfo.GetUserOrDepartmentIformation(null, user.UserName, Start, End).First();


            Repository<Absence> absenceRepository = new Repository<Absence>(context);

            ShowUserReportViewModel model = new ShowUserReportViewModel { Start = Start, End = End, listOfWorkAndAbesnceTimeForChart = new List<ChartViewModel>(), UserInfo = info, DaysCount = (End - Start).Days };
            ArrayList colors = new ArrayList();

            if (info.WorkTimes == null)
            {
                for (DateTime start = Start; start <= End; start = start.AddDays(1))
                {
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
                        model.listOfWorkAndAbesnceTimeForChart.Add(notWorkThisDay);

                    }

                }

            }
            else
            {
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
                        model.listOfWorkAndAbesnceTimeForChart.Add(notWorkThisDay);

                    }

                }
            }
            if (info.WorkTimes != null)
            {

                var times = info.WorkTimes.GroupBy(t => t.TimeIn.Date);
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
                    model.listOfWorkAndAbesnceTimeForChart.Add(beforeWork);

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
                        model.listOfWorkAndAbesnceTimeForChart.Add(beginWorkTillFirstAbsence);
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
                        model.listOfWorkAndAbesnceTimeForChart.Add(beginWorkTillFirstAbsence);
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
                        model.listOfWorkAndAbesnceTimeForChart.Add(tv);
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
                            model.listOfWorkAndAbesnceTimeForChart.Add(gapTv);
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
                            model.listOfWorkAndAbesnceTimeForChart.Add(gapTv);
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
                    model.listOfWorkAndAbesnceTimeForChart.Add(afterWork);

                }
            }

            ShowUserReportViewModel resultModel = new ShowUserReportViewModel { Start = Start, End = End, listOfWorkAndAbesnceTimeForChart = model.listOfWorkAndAbesnceTimeForChart.OrderBy(l => l.StartAbsence.Date).ToList(), UserInfo = info, DaysCount = (End - Start).Days };
            
            string dataStr = Newtonsoft.Json.JsonConvert.SerializeObject(colors, Newtonsoft.Json.Formatting.None);
            ViewBag.Colors = new HtmlString(dataStr);
            ViewBag.User = user.UserName;
            return View("ShowUserReport", resultModel);
        }
        #endregion

        #region FindUserReport
        public ActionResult FindUserReport(string userName, string returnUrl)
        {
            var ManagerDepartments = UserInfo.GetManagerDepartments(HttpContext.User.Identity.Name);
            foreach (var department in ManagerDepartments)
            {
                var userInfos = UserInfo.GetUserOrDepartmentIformation(department, null, DateTime.Today, DateTime.Today);
                foreach (var user in userInfos)
                {
                    if (user.userInformarion.Login == userName)
                    {
                        DateTime End = DateTime.Now.AddDays(-1).Date;
                        DateTime Start = DateTime.Now.AddDays(-7).Date;
                        return ShowUserReport(userName, Start, End);
                    }
                }
            }
            return Redirect(returnUrl);         
        }
        #endregion
    }
}