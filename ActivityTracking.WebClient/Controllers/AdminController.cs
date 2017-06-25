using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActivityTracking.ServicesForAcessToDB;
using ActivityTracking.DomainModel;
using ActivityTracking.WebClient.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using ActivityTracking.GetUserInfo;
using System.Collections;

namespace ActivityTracking.WebClient.Controllers
{
    public class AdminController : Controller
    {

        #region Index
        public ActionResult Index()
        {
            ManagerIndexViewModel viewModel = new ManagerIndexViewModel { DepartmentNames = new List<string>(), TeamNames = new List<string>() };
            List<string> managerDepartmentNames = GetUserInfo.UserInfo.GetManagerDepartments(HttpContext.User.Identity.Name);
            List<string> managerTeamstNames = GetUserInfo.UserInfo.GetManagerTeams(HttpContext.User.Identity.Name);
            viewModel.DepartmentNames = managerDepartmentNames;
            viewModel.TeamNames = managerTeamstNames;
            return View(viewModel);
        }
        #endregion

        #region ShowDepartmentUsers
        public ActionResult ShowDepartmentUsers(string departmentName)
        {
            ManagerShowUsersViewModel managerShowUsersViewModel = new ManagerShowUsersViewModel();
            var userInfoModels = GetUserInfo.UserInfo.GetTeamOrDepartmentOrUserIformation(null, departmentName, null, DateTime.Today, DateTime.Today);
            managerShowUsersViewModel.UserInfoModels = userInfoModels;
            managerShowUsersViewModel.DepartmentOrTeamName = departmentName;
            return View(managerShowUsersViewModel);
        }
        #endregion

        #region ShowTeamUsers
        public ActionResult ShowTeamUsers(string teamName)
        {
            ManagerShowUsersViewModel managerShowUsersViewModel = new ManagerShowUsersViewModel();
            var userInfoModels = GetUserInfo.UserInfo.GetTeamOrDepartmentOrUserIformation(teamName, null, null, DateTime.Today, DateTime.Today);
            managerShowUsersViewModel.UserInfoModels = userInfoModels;
            managerShowUsersViewModel.DepartmentOrTeamName = teamName;
            return View(managerShowUsersViewModel);
        }
        #endregion

        #region FindUserReport
        public ActionResult FindUserReport(string userName, string returnUrl)
        {
            var ManagerDepartments = UserInfo.GetManagerDepartments(HttpContext.User.Identity.Name);
            foreach (var department in ManagerDepartments)
            {
                var userInfos = UserInfo.GetTeamOrDepartmentOrUserIformation(null, department, null, DateTime.Today, DateTime.Today);
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

        #region ShowUserReportWithValidation
        public ActionResult ShowUserReportWithValidation(string userName, DateTime? Start, DateTime? End, bool isDefaultRequest = false)
        {
            if (isDefaultRequest == true)
            {
                DBAcessService<WeekBeginningDay> weekBeginingDayService = new DBAcessService<WeekBeginningDay>();
                WeekBeginningDay weekBeginningDay = weekBeginingDayService.GetItem(1);
                for (DateTime i = DateTime.Today; i >= DateTime.Now.AddDays(-7).Date; i = i.AddDays(-1))
                {
                    if (i.DayOfWeek.ToString() == weekBeginningDay.DayName)
                    {
                        Start = i;
                        End = DateTime.Today;
                        return ShowUserReport(userName, (DateTime)Start, (DateTime)End);
                    }
                }

            }
            if (Start == null || End == null)
            {
                TempData["message"] = "You should enter two dates";
                DBAcessService<WeekBeginningDay> weekBeginingDayService = new DBAcessService<WeekBeginningDay>();
                WeekBeginningDay weekBeginningDay = weekBeginingDayService.GetItem(1);
                for (DateTime i = DateTime.Today; i >= DateTime.Now.AddDays(-7).Date; i = i.AddDays(-1))
                {
                    if (i.DayOfWeek.ToString() == weekBeginningDay.DayName)
                    {
                        Start = i;
                        End = DateTime.Today;
                        return ShowUserReport(userName, (DateTime)Start, (DateTime)End);
                    }
                }

                return ShowUserReport(userName, (DateTime)Start, (DateTime)End);

            }
            else if (Start > End)
            {
                TempData["message"] = "End date should be bigger then start date";
                DBAcessService<WeekBeginningDay> weekBeginingDayService = new DBAcessService<WeekBeginningDay>();
                WeekBeginningDay weekBeginningDay = weekBeginingDayService.GetItem(1);
                for (DateTime i = DateTime.Today; i >= DateTime.Now.AddDays(-7).Date; i = i.AddDays(-1))
                {
                    if (i.DayOfWeek.ToString() == weekBeginningDay.DayName)
                    {
                        Start = i;
                        End = DateTime.Today;
                        return ShowUserReport(userName, (DateTime)Start, (DateTime)End);
                    }
                }
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

            DBAcessService<ApplicationUser> applicationUserService = new DBAcessService<ApplicationUser>();
            var user = applicationUserService.GetList().First(u => u.UserName == userName);
            UserInfoModel info = GetUserInfo.UserInfo.GetTeamOrDepartmentOrUserIformation(null, null, user.UserName, Start, End).First();
            DBAcessService<Absence> absenceService = new DBAcessService<Absence>();
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

                    var absences = absenceService.GetList().Where(a => a.User.UserName == user.UserName).Where(a => a.Date == timeGroupToArray[0].TimeIn.Date).ToArray();
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

        #region ShowDepartmentReportWithValidation
        [HttpPost]
        public ActionResult ShowDepartmentReportWithValidation(string DepartmentList, DateTime? Start, DateTime? End, bool BarChart, bool PieChart, bool ColumnChart,bool isDefaultRequest = false)
        {
            if (isDefaultRequest == true)
            {
                DBAcessService<WeekBeginningDay> weekBeginingDyService = new DBAcessService<WeekBeginningDay>();
                WeekBeginningDay weekBeginningDay = weekBeginingDyService.GetItem(1);
                for (DateTime i = DateTime.Today; i >= DateTime.Now.AddDays(-7).Date; i = i.AddDays(-1))
                {
                    if (i.DayOfWeek.ToString() == weekBeginningDay.DayName)
                    {
                        Start = i;
                        End = DateTime.Today;
                        return ShowDepartmentReport(DepartmentList, (DateTime)Start, (DateTime)End, BarChart, PieChart, ColumnChart);
                    }
                }

            }
            if (Start == null || End == null)
            {
                TempData["message"] = "You should enter two dates";
                DBAcessService<WeekBeginningDay> weekBeginingDyService = new DBAcessService<WeekBeginningDay>();
                WeekBeginningDay weekBeginningDay = weekBeginingDyService.GetItem(1);
                for (DateTime i = DateTime.Today; i >= DateTime.Now.AddDays(-7).Date; i = i.AddDays(-1))
                {
                    if (i.DayOfWeek.ToString() == weekBeginningDay.DayName)
                    {
                        Start = i;
                        End = DateTime.Today;
                        return ShowDepartmentReport(DepartmentList, (DateTime)Start, (DateTime)End, BarChart, PieChart, ColumnChart);
                    }
                }

                return ShowDepartmentReport(DepartmentList, (DateTime)Start, (DateTime)End, BarChart, PieChart, ColumnChart);

            }
            else if (Start > End)
            {
                TempData["message"] = "End date should be bigger then start date";
                DBAcessService<WeekBeginningDay> weekBeginingDyService = new DBAcessService<WeekBeginningDay>();
                WeekBeginningDay weekBeginningDay = weekBeginingDyService.GetItem(1);
                for (DateTime i = DateTime.Today; i >= DateTime.Now.AddDays(-7).Date; i = i.AddDays(-1))
                {
                    if (i.DayOfWeek.ToString() == weekBeginningDay.DayName)
                    {
                        Start = i;
                        End = DateTime.Today;
                        return ShowDepartmentReport(DepartmentList, (DateTime)Start, (DateTime)End, BarChart, PieChart, ColumnChart);
                    }
                }
                return ShowDepartmentReport(DepartmentList, (DateTime)Start, (DateTime)End, BarChart, PieChart, ColumnChart);
            }
            else
            {
                return ShowDepartmentReport(DepartmentList, (DateTime)Start, (DateTime)End, BarChart, PieChart, ColumnChart);
            }                      
        }
        #endregion

        #region ShowTeamReportWithValidation
        [HttpPost]
        public ActionResult ShowTeamReportWithValidation(string returnUrl, string teamName, DateTime? Start, DateTime? End, bool BarChart, bool PieChart, bool ColumnChart, bool isDefaultRequest = false)
        {
            if (isDefaultRequest == true)
            {
                DBAcessService<WeekBeginningDay> weekBeginingDyService = new DBAcessService<WeekBeginningDay>();
                WeekBeginningDay weekBeginningDay = weekBeginingDyService.GetItem(1);
                for (DateTime i = DateTime.Today; i >= DateTime.Now.AddDays(-7).Date; i = i.AddDays(-1))
                {
                    if (i.DayOfWeek.ToString() == weekBeginningDay.DayName)
                    {
                        Start = i;
                        End = DateTime.Today;
                        return ShowTeamReport(returnUrl, teamName, (DateTime)Start, (DateTime)End, BarChart, PieChart, ColumnChart);
                    }
                }

            }
            if (Start == null || End == null)
            {
                TempData["message"] = "You should enter two dates";
                DBAcessService<WeekBeginningDay> weekBeginingDyService = new DBAcessService<WeekBeginningDay>();
                WeekBeginningDay weekBeginningDay = weekBeginingDyService.GetItem(1);
                for (DateTime i = DateTime.Today; i >= DateTime.Now.AddDays(-7).Date; i = i.AddDays(-1))
                {
                    if (i.DayOfWeek.ToString() == weekBeginningDay.DayName)
                    {
                        Start = i;
                        End = DateTime.Today;
                        return ShowTeamReport(returnUrl, teamName, (DateTime)Start, (DateTime)End, BarChart, PieChart, ColumnChart);
                    }
                }

                return ShowTeamReport(returnUrl, teamName, (DateTime)Start, (DateTime)End, BarChart, PieChart, ColumnChart);

            }
            else if (Start > End)
            {
                TempData["message"] = "End date should be bigger then start date";
                DBAcessService<WeekBeginningDay> weekBeginingDyService = new DBAcessService<WeekBeginningDay>();
                WeekBeginningDay weekBeginningDay = weekBeginingDyService.GetItem(1);
                for (DateTime i = DateTime.Today; i >= DateTime.Now.AddDays(-7).Date; i = i.AddDays(-1))
                {
                    if (i.DayOfWeek.ToString() == weekBeginningDay.DayName)
                    {
                        Start = i;
                        End = DateTime.Today;
                        return ShowTeamReport(returnUrl, teamName, (DateTime)Start, (DateTime)End, BarChart, PieChart, ColumnChart);
                    }
                }
                return ShowTeamReport(returnUrl, teamName, (DateTime)Start, (DateTime)End, BarChart, PieChart, ColumnChart);
            }
            else
            {
                return ShowTeamReport(returnUrl, teamName, (DateTime)Start, (DateTime)End, BarChart, PieChart, ColumnChart);
            }
        }
        #endregion

        #region ShowDepartmentReport
        [HttpPost]
        public ActionResult ShowDepartmentReport(string DepartmentList, DateTime Start, DateTime End, bool BarChart, bool PieChart, bool ColumnChart)
        {
            ArrayList colors = new ArrayList();


            ManagerShowDepartmentOrTeamReportViewModel managerShowDepartmentReportViewModel = new ManagerShowDepartmentOrTeamReportViewModel()
            { Start = Start, End = End, ReasonsNames = new List<string>(), ReasonInfos = GenerateDataForDepartmentReportInPercentage(DepartmentList, Start, End), ChosenDepartmentOrTeamName = DepartmentList, BarChart = BarChart, PieChart = PieChart, ColumnChart = ColumnChart };

            List<UserInfoModel> usersInfosFromThisDepartment = UserInfo.GetTeamOrDepartmentOrUserIformation(null, DepartmentList, null, DateTime.Today, DateTime.Today);
            if (usersInfosFromThisDepartment.Count == 0)
            {
                TempData["message"] = "This Department doesn't contain users";
                return RedirectToAction("Index");
            }
            string randomUserFromTgisDepartmentLogin = usersInfosFromThisDepartment.First().userInformarion.Login;
            string divManagerFromThisDepartmentLogin = UserInfo.GetDivisionManagerOfUser(randomUserFromTgisDepartmentLogin);
            DBAcessService<DivisionManager> divManagerService = new DBAcessService<DivisionManager>();        
            DivisionManager divisionManager = divManagerService.GetList().First(d => d.Login == divManagerFromThisDepartmentLogin);
            ICollection<Reason> reasons = divisionManager.Reasons;
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

        #region ShowTeamReport
        [HttpPost]
        public ActionResult ShowTeamReport(string returnUrl, string teamName, DateTime Start, DateTime End, bool BarChart, bool PieChart, bool ColumnChart)
        {
            ArrayList colors = new ArrayList();

            List<UserInfoModel> usersInTeamInfoModels = GetUserInfo.UserInfo.GetTeamOrDepartmentOrUserIformation(teamName, null, null, Start, End);
            if (usersInTeamInfoModels.Count == 0)
            {
                TempData["message"] = "This Department doesn't contain users";
                return Redirect(returnUrl);
            }

            ManagerShowDepartmentOrTeamReportViewModel viewModel = new ManagerShowDepartmentOrTeamReportViewModel()
            { Start = Start, End = End, ReasonsNames = new List<string>(), ReasonInfos = GenerateDataForTeamReportInPercentage(teamName, Start, End), ChosenDepartmentOrTeamName = teamName, BarChart = BarChart, PieChart = PieChart, ColumnChart = ColumnChart };

            DBAcessService<DivisionManager> divManagerService = new DBAcessService<DivisionManager>();
            string randomUserFromThisTeamtLogin = usersInTeamInfoModels.First().userInformarion.Login;
            string divManagerOfThisRandomUserLogin = UserInfo.GetDivisionManagerOfUser(randomUserFromThisTeamtLogin);
            DivisionManager divisionManager = divManagerService.GetList().First(d => d.Login == divManagerOfThisRandomUserLogin);
            ICollection<Reason> reasons = divisionManager.Reasons;
            viewModel.ReasonsNames.Add("Work");
            if (!colors.Contains("#0000FF"))
            {
                colors.Add("#0000FF");
            }
            foreach (var reason in reasons)
            {
                viewModel.ReasonsNames.Add(reason.Name);
                if (!colors.Contains(reason.Color))
                {
                    colors.Add(reason.Color);
                }
            }


            string dataStr = Newtonsoft.Json.JsonConvert.SerializeObject(colors, Newtonsoft.Json.Formatting.None);
            ViewBag.Colors = new HtmlString(dataStr);
            return View("ShowTeamReport", viewModel);
        }
        #endregion

        #region DeleteReason
        public ActionResult DeleteReason(int Id)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            Reason reason = unitOfWork.ReasonService.GetList().First(r => r.Id == Id);
            foreach (var absence in unitOfWork.AbsenceService.GetList())
            {
                if (absence.Reason.Id == reason.Id)
                {
                    unitOfWork.AbsenceService.Delete(absence.Id);
                }
            }
            unitOfWork.ReasonService.Delete(reason.Id);
            TempData["message"] = "Reason was deleted";
            return RedirectToAction("Settings");
        }
        #endregion

        #region AddReason
        public ActionResult AddReason(string reasonName, string reasonColor)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            foreach (var reason in unitOfWork.ReasonService.GetList())
            {
                if (reason.Name == reasonName)
                {
                    TempData["message"] = "Reason with such name already exists";
                    return RedirectToAction("Settings");
                }
            }
            if (reasonName == "")
            {
                TempData["message"] = "You should enter reason's name";
                return RedirectToAction("Settings");
            }
            else
            {
                Reason newReason = new Reason();
                newReason.AddingTime = DateTime.Now;
                newReason.Color = reasonColor;
                newReason.Name = reasonName;
                unitOfWork.ReasonService.Create(newReason);
                TempData["message"] = "Reason was added";
                return RedirectToAction("Settings");
            }

        }
        #endregion

        #region ChangeReason
        public ActionResult ChangeReason(int Id, string reasonNewName, string reasonNewColor)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            if (reasonNewName == "")
            {
                TempData["message"] = "You should enter reason's name";
                return RedirectToAction("Settings");
            }
            else
            {               
                Reason changingReason = unitOfWork.ReasonService.GetList().First(r => r.Id == Id);
                changingReason.Name = reasonNewName;
                changingReason.Color = reasonNewColor;
                unitOfWork.ReasonService.Update(changingReason);
                TempData["message"] = "Reason was changed";
                return RedirectToAction("Settings");
            }
        }
        #endregion

        #region ChangeWeekBeginningDay
        public ActionResult ChangeWeekBeginningDay(string DayNames)
        {
            UnitOfWork unitOfWork = new UnitOfWork();          
            WeekBeginningDay weekBeginningDay = unitOfWork.WeekBeginningDayService.GetItem(1);
            weekBeginningDay.DayName = DayNames;
            unitOfWork.WeekBeginningDayService.Update(weekBeginningDay);
            TempData["message"] = "Week beginning day was changed to: " + DayNames;
            return RedirectToAction("Settings");
        }

        #endregion

        #region GenerateDataForDepartmentReportInPercentage
        private List<ReasonInfo> GenerateDataForDepartmentReportInPercentage(string DepartmentList, DateTime Start, DateTime End)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            List<ReasonInfo> ReasonInfos = new List<ReasonInfo>();
            List<UserInfoModel> usersInfosFromThisDepartment = UserInfo.GetTeamOrDepartmentOrUserIformation(null, DepartmentList, null, Start, End);
            if (usersInfosFromThisDepartment.Count == 0)
            {
                TempData["message"] = "This Department doesn't contain users";
                return null;
            }
            string randomUserFromTgisDepartmentLogin = usersInfosFromThisDepartment.First().userInformarion.Login;
            string divManagerFromThisDepartmentLogin = UserInfo.GetDivisionManagerOfUser(randomUserFromTgisDepartmentLogin);          
            DivisionManager divisionManager = unitOfWork.DivisionManagerService.GetList().First(d => d.Login == divManagerFromThisDepartmentLogin);
            ICollection<Reason> reasons = divisionManager.Reasons;
            TimeSpan workDurationForGivenDays = new TimeSpan(0, 0, 0);
            foreach (var userInfoModel in usersInfosFromThisDepartment.OrderBy(u => u.userInformarion.Login))
            {

                var userAbsences = unitOfWork.AbsenceService.GetList().Where(a => a.Date >= Start && a.Date <= End).Where(a => a.User.UserName == userInfoModel.userInformarion.Login);



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
            foreach (var reason in reasons)
            {
                TimeSpan reasonDuration = new TimeSpan(0, 0, 0);
                foreach (var userInfoModel in usersInfosFromThisDepartment)
                {
                    var userAbsences = unitOfWork.AbsenceService.GetList().Where(a => a.Date >= Start && a.Date <= End).Where(a => a.User.UserName == userInfoModel.userInformarion.Login).Where(a => a.Reason.Name == reason.Name);
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

        #region GenerateDataForTeamReportInPercentage
        private List<ReasonInfo> GenerateDataForTeamReportInPercentage(string teamName, DateTime Start, DateTime End)
        {
            DBAcessService<DivisionManager> divManagerService = new DBAcessService<DivisionManager>();
            DBAcessService<Absence> absenceService = new DBAcessService<Absence>();
            List<ReasonInfo> ReasonInfos = new List<ReasonInfo>();
            List<UserInfoModel> usersInfosFromThisTeam = UserInfo.GetTeamOrDepartmentOrUserIformation(teamName, null, null, Start, End);
            if (usersInfosFromThisTeam.Count == 0)
            {
                TempData["message"] = "This Department doesn't contain users";
                return null;
            }
            string randomUserFromThisTeamtLogin = usersInfosFromThisTeam.First().userInformarion.Login;
            string divManagerOfThisRandomUserLogin = UserInfo.GetDivisionManagerOfUser(randomUserFromThisTeamtLogin);
            DivisionManager divisionManager = divManagerService.GetList().First(d => d.Login == divManagerOfThisRandomUserLogin);
            ICollection<Reason> reasons = divisionManager.Reasons;            
            TimeSpan workDurationForGivenDays = new TimeSpan(0, 0, 0);
            foreach (var userInfoModel in usersInfosFromThisTeam.OrderBy(u => u.userInformarion.Login))
            {
                var userAbsences = absenceService.GetList().Where(a => a.Date >= Start && a.Date <= End).Where(a => a.User.UserName == userInfoModel.userInformarion.Login);
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
            foreach (var reason in reasons)
            {
                TimeSpan reasonDuration = new TimeSpan(0, 0, 0);
                foreach (var userInfoModel in usersInfosFromThisTeam)
                {
                    var userAbsences = absenceService.GetList().Where(a => a.Date >= Start && a.Date <= End).Where(a => a.User.UserName == userInfoModel.userInformarion.Login).Where(a => a.Reason.Name == reason.Name);
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

        #region ShowDepartmentReportByUsersWithValidation

        public ActionResult ShowDepartmentReportByUsersWithValidation(string departmentName, DateTime? Start, DateTime? End, bool isDefaultRequest = false)
        {
            if (isDefaultRequest == true)
            {
                DBAcessService<WeekBeginningDay> weekBeginingDayService = new DBAcessService<WeekBeginningDay>();
                WeekBeginningDay weekBeginningDay = weekBeginingDayService.GetItem(1);
                for (DateTime i = DateTime.Today; i >= DateTime.Now.AddDays(-7).Date; i = i.AddDays(-1))
                {
                    if (i.DayOfWeek.ToString() == weekBeginningDay.DayName)
                    {
                        Start = i;
                        End = DateTime.Today;
                        return ShowDepartmentReportByUsers(departmentName, (DateTime)Start, (DateTime)End);
                    }
                }

            }
            if (Start == null || End == null)
            {
                TempData["message"] = "You should enter two dates";
                DBAcessService<WeekBeginningDay> weekBeginingDayService = new DBAcessService<WeekBeginningDay>();
                WeekBeginningDay weekBeginningDay = weekBeginingDayService.GetItem(1);
                for (DateTime i = DateTime.Today; i >= DateTime.Now.AddDays(-7).Date; i = i.AddDays(-1))
                {
                    if (i.DayOfWeek.ToString() == weekBeginningDay.DayName)
                    {
                        Start = i;
                        End = DateTime.Today;
                        return ShowDepartmentReportByUsers(departmentName, (DateTime)Start, (DateTime)End);
                    }
                }
                return ShowDepartmentReportByUsers(departmentName, (DateTime)Start, (DateTime)End);
            }
            else if (Start > End)
            {
                TempData["message"] = "End date should be bigger then start date";
                DBAcessService<WeekBeginningDay> weekBeginingDayService = new DBAcessService<WeekBeginningDay>();
                WeekBeginningDay weekBeginningDay = weekBeginingDayService.GetItem(1);
                for (DateTime i = DateTime.Today; i >= DateTime.Now.AddDays(-7).Date; i = i.AddDays(-1))
                {
                    if (i.DayOfWeek.ToString() == weekBeginningDay.DayName)
                    {
                        Start = i;
                        End = DateTime.Today;
                        
                    }
                }
                return ShowDepartmentReportByUsers(departmentName, (DateTime)Start, (DateTime)End);
            }
            else
            {
                return ShowDepartmentReportByUsers(departmentName, (DateTime)Start, (DateTime)End);
            }

        }
        #endregion

        #region ShowTeamReportByUsersWithValidation

        public ActionResult ShowTeamReportByUsersWithValidation(string teamName, DateTime? Start, DateTime? End, bool isDefaultRequest = false)
        {
            if (isDefaultRequest == true)
            {
                DBAcessService<WeekBeginningDay> weekBeginingDayService = new DBAcessService<WeekBeginningDay>();
                WeekBeginningDay weekBeginningDay = weekBeginingDayService.GetItem(1);
                for (DateTime i = DateTime.Today; i >= DateTime.Now.AddDays(-7).Date; i = i.AddDays(-1))
                {
                    if (i.DayOfWeek.ToString() == weekBeginningDay.DayName)
                    {
                        Start = i;
                        End = DateTime.Today;
                        return ShowTeamReportByUsers(teamName, (DateTime)Start, (DateTime)End);
                    }
                }

            }
            if (Start == null || End == null)
            {
                TempData["message"] = "You should enter two dates";
                DBAcessService<WeekBeginningDay> weekBeginingDayService = new DBAcessService<WeekBeginningDay>();
                WeekBeginningDay weekBeginningDay = weekBeginingDayService.GetItem(1);
                for (DateTime i = DateTime.Today; i >= DateTime.Now.AddDays(-7).Date; i = i.AddDays(-1))
                {
                    if (i.DayOfWeek.ToString() == weekBeginningDay.DayName)
                    {
                        Start = i;
                        End = DateTime.Today;
                        return ShowDepartmentReportByUsers(teamName, (DateTime)Start, (DateTime)End);
                    }
                }
                return ShowTeamReportByUsers(teamName, (DateTime)Start, (DateTime)End);
            }
            else if (Start > End)
            {
                TempData["message"] = "End date should be bigger then start date";
                DBAcessService<WeekBeginningDay> weekBeginingDayService = new DBAcessService<WeekBeginningDay>();
                WeekBeginningDay weekBeginningDay = weekBeginingDayService.GetItem(1);
                for (DateTime i = DateTime.Today; i >= DateTime.Now.AddDays(-7).Date; i = i.AddDays(-1))
                {
                    if (i.DayOfWeek.ToString() == weekBeginningDay.DayName)
                    {
                        Start = i;
                        End = DateTime.Today;

                    }
                }
                return ShowTeamReportByUsers(teamName, (DateTime)Start, (DateTime)End);
            }
            else
            {
                return ShowTeamReportByUsers(teamName, (DateTime)Start, (DateTime)End);
            }

        }
        #endregion

        #region ShowDepartmentReportByUsers
        [HttpPost]
        public ActionResult ShowDepartmentReportByUsers(string departmentName, DateTime Start, DateTime End)
        {
            DBAcessService<ApplicationUser> applicationUserService = new DBAcessService<ApplicationUser>();
            DBAcessService<DivisionManager> divManagerService = new DBAcessService<DivisionManager>();
            DBAcessService<Absence> absenceService = new DBAcessService<Absence>();
            ArrayList colors = new ArrayList();
            List<UserInfoModel> usersInDepartmentInfoModels = GetUserInfo.UserInfo.GetTeamOrDepartmentOrUserIformation(null, departmentName, null, Start, End);

            ManagerShowDepartmentOrTeamReportByUsersViewModel viewModel = new ManagerShowDepartmentOrTeamReportByUsersViewModel
            {
                Start = Start,
                End = End,
                ReasonsNames = new List<string>(),
                WorkersInfos = new List<WorkerInfo>() { },
                ReasonInfosForPercentageReport = GenerateDataForDepartmentReportInPercentage(departmentName, Start, End),
                ChosenDepartmentOrTeamName = departmentName
            };

            List<UserInfoModel> usersInfosFromThisDepartment = UserInfo.GetTeamOrDepartmentOrUserIformation(null, departmentName, null, DateTime.Today, DateTime.Today);
            if (usersInfosFromThisDepartment.Count == 0)
            {
                TempData["message"] = "This Department doesn't contain users";
                return RedirectToAction("Index");
            }
            string randomUserFromTgisDepartmentLogin = usersInfosFromThisDepartment.First().userInformarion.Login;
            string divManagerFromThisDepartmentLogin = UserInfo.GetDivisionManagerOfUser(randomUserFromTgisDepartmentLogin);
            DivisionManager divisionManager = divManagerService.GetList().First(d => d.Login == divManagerFromThisDepartmentLogin);
            ICollection<Reason> reasons = divisionManager.Reasons;
            viewModel.ReasonsNames.Add("Work");
            if (!colors.Contains("#0000FF"))
            {
                colors.Add("#0000FF");
            }
            foreach (var reason in reasons)
            {
                viewModel.ReasonsNames.Add(reason.Name);
            }



            foreach (var userInfoModel in usersInDepartmentInfoModels.OrderBy(u => u.userInformarion.Login))
            {

                var user = applicationUserService.GetList().First(u => u.UserName == userInfoModel.userInformarion.Login);

                WorkerInfo workerInfo = new WorkerInfo { Name = user.UserName, ReasonInfos = new List<ReasonInfo>() };
                var userAbsences = absenceService.GetList().Where(a => a.User.Id == user.Id).Where(ab => ab.Date.Date >= Start.Date && ab.Date <= End.Date);

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


                foreach (var reason in reasons)
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

            return View("ShowDepartmentReportByUsers", viewModel);
        }
        #endregion

        #region ShowTeamReportByUsers
        [HttpPost]
        public ActionResult ShowTeamReportByUsers(string teamName, DateTime Start, DateTime End)
        {
            DBAcessService<ApplicationUser> applicationUserService = new DBAcessService<ApplicationUser>();
            DBAcessService<DivisionManager> divManagerService = new DBAcessService<DivisionManager>();
            DBAcessService<Absence> absenceService = new DBAcessService<Absence>();
            ArrayList colors = new ArrayList();
            List<UserInfoModel> usersInTeamInfoModels = GetUserInfo.UserInfo.GetTeamOrDepartmentOrUserIformation(teamName, null, null, Start, End);
            if (usersInTeamInfoModels.Count == 0)
            {
                TempData["message"] = "This Department doesn't contain users";
                return RedirectToAction("Index");
            }

            ManagerShowDepartmentOrTeamReportByUsersViewModel viewModel = new ManagerShowDepartmentOrTeamReportByUsersViewModel
            {
                Start = Start,
                End = End,
                ReasonsNames = new List<string>(),
                WorkersInfos = new List<WorkerInfo>() { },
                ReasonInfosForPercentageReport = GenerateDataForTeamReportInPercentage(teamName, Start, End),
                ChosenDepartmentOrTeamName = teamName
            };
            string randomUserFromThisTeamtLogin = usersInTeamInfoModels.First().userInformarion.Login;
            string divManagerOfThisRandomUserLogin = UserInfo.GetDivisionManagerOfUser(randomUserFromThisTeamtLogin);
            DivisionManager divisionManager = divManagerService.GetList().First(d => d.Login == divManagerOfThisRandomUserLogin);
            ICollection<Reason> reasons = divisionManager.Reasons;
            viewModel.ReasonsNames.Add("Work");
            if (!colors.Contains("#0000FF"))
            {
                colors.Add("#0000FF");
            }
            foreach (var reason in reasons)
            {
                viewModel.ReasonsNames.Add(reason.Name);
            }



            foreach (var userInfoModel in usersInTeamInfoModels.OrderBy(u => u.userInformarion.Login))
            {

                var user = applicationUserService.GetList().First(u => u.UserName == userInfoModel.userInformarion.Login);

                WorkerInfo workerInfo = new WorkerInfo { Name = user.UserName, ReasonInfos = new List<ReasonInfo>() };
                var userAbsences = absenceService.GetList().Where(a => a.User.Id == user.Id).Where(ab => ab.Date.Date >= Start.Date && ab.Date <= End.Date);

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


                foreach (var reason in reasons)
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

            return View("ShowTeamReportByUsers", viewModel);
        }
        #endregion

        #region Settings
        public ActionResult Settings()
        {
            DBAcessService<WeekBeginningDay> weekBeginingDayService = new DBAcessService<WeekBeginningDay>();
            DBAcessService<Reason> reasonService = new DBAcessService<Reason>();
            var allReasons = reasonService.GetList();
            AdminSettingsViewModel adminSettingsViewModel = new AdminSettingsViewModel();
            adminSettingsViewModel.AllReasons = allReasons.ToList();
            adminSettingsViewModel.DayNames = new SelectList(new List<string>() {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"});
            adminSettingsViewModel.CurrentDayName = weekBeginingDayService.GetItem(1).DayName;
            return View(adminSettingsViewModel);
        }
        #endregion
   
    }
}