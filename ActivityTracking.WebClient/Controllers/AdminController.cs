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
using Microsoft.AspNet.Identity.EntityFramework;
using ActivityTracking.GetUserInfo;
using System.Collections;

namespace ActivityTracking.WebClient.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
        }

        // GET: Admin
        public ActionResult Index()
        {
            Repository < Microsoft.AspNet.Identity.EntityFramework.IdentityRole > rolesRepository= new Repository<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>();
            Repository<ApplicationUser> userRepository = new Repository<ApplicationUser>();
            var users = userRepository.GetList().OrderBy(u=>u.UserName);

            Repository<Group> groupRepository = new Repository<Group>();
            SelectList groupList = new SelectList(groupRepository.GetList(), "Id", "Name");

            Repository<IdentityRole> roleRepository = new Repository<IdentityRole>();
            SelectList roleList = new SelectList(roleRepository.GetList().Where(r=>r.Name !="admin"), "Name", "Name");

            AdminIndexViewModel viewModel = new AdminIndexViewModel { GroupList = groupList, RoleList = roleList, UsersViewModelsList = new List<UserViewModel>()};
            foreach (var user in users)
            {

                viewModel.UsersViewModelsList.Add(new UserViewModel { User = user, UserRole = rolesRepository.GetList().First(r => r.Id == user.Roles.First().RoleId).Name });
            }
        
            return View(viewModel);
        }

        public ActionResult ShowGroups()
        {
            Repository<Group> groupRepository = new Repository<Group>();
            var groups = groupRepository.GetList();
            return View(groups);
        }

        public ActionResult ChangeGroupMayAbsentTime(string Minutes)
        {
            Repository<Group> groupRepository = new Repository<Group>();
            var groups = groupRepository.GetList();
            foreach (var group in groups)
            {
                group.MayAbsentTime = new TimeSpan(0, Convert.ToInt32(Minutes), 0);
                groupRepository.Update(group);
            }
            return RedirectToAction("ShowGroups");
        }

        public ActionResult ShowGroupInfo(int Id)
        {
            ApplicationContext context = new ApplicationContext();
            Repository<Group> groupRepository = new Repository<Group>(context);
            var group = groupRepository.GetItem(Id);
            Repository<IdentityRole> roleRepository = new Repository<IdentityRole>(context);
            ApplicationUser groupManager;
            if (group.Users.Any(m => m.Roles.First().RoleId == roleRepository.GetList().First(r => r.Name == "manager").Id))
            {
                groupManager = group.Users.First(m => m.Roles.First().RoleId == roleRepository.GetList().First(r => r.Name == "manager").Id);
                AdminShowGroupInfoViewModel showGroupInfoViewModel = new AdminShowGroupInfoViewModel { Group = group, GroupManager = groupManager };
                return View(showGroupInfoViewModel);
            }
            else
            {
                AdminShowGroupInfoViewModel showGroupInfoViewModel = new AdminShowGroupInfoViewModel { Group = group, GroupManager = null };
                return View(showGroupInfoViewModel);
            }          
        }

        [HttpPost]
        public ActionResult ChangeUserGroup(string Id, string GroupList)
        {
            ApplicationContext context = new ApplicationContext();
            Repository<ApplicationUser> userRepository = new Repository<ApplicationUser>(context);
            var user = userRepository.GetList().First(u => u.Id == Id);

            Repository<Group> groupRepository = new Repository<Group>(context);
            Group group = groupRepository.GetList().First(g => g.Id == Convert.ToInt32(GroupList));
            user.Group = group;
            userRepository.Update(user);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> ChangeUserRole(string Id, string RoleList)
        {           
            ApplicationUser user = await UserManager.FindByIdAsync(Id);
            Repository<IdentityRole> roleRepository = new Repository<IdentityRole>();
            UserManager.RemoveFromRole(user.Id, roleRepository.GetList().First(u=>u.Id == user.Roles.First().RoleId).Name);        
            
            await UserManager.AddToRoleAsync(user.Id, (roleRepository.GetList().First(u => u.Name == RoleList).Name));

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> DeleteUser(string Id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(Id);
            IdentityResult result = await UserManager.DeleteAsync(user);           
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteGroup(int Id)
        {
            ApplicationContext context = new ApplicationContext();
            Repository<Group> groupRepository = new Repository<Group>(context);
            var group = groupRepository.GetList().First(g=>g.Id == Convert.ToInt32(Id));
            Repository<ApplicationUser> userRepository = new Repository<ApplicationUser>(context);
            var users = userRepository.GetList().Where(u => u.GroupId == group.Id);
            foreach (var user in users)
            {
                user.Group = null;
                userRepository.Update(user);
            }
            groupRepository.Delete(group.Id);
            return RedirectToAction("ShowGroups");
        }

        [HttpPost]
        public ActionResult CreateNewGroup(string GroupName)
        {
            Group group = new Group { Name = GroupName, MayAbsentTime = new TimeSpan(0, 15, 0) };
            Repository<Group> groupRepository = new Repository<Group>();
            groupRepository.Create(group);
            return RedirectToAction("ShowGroups");
        }

        public ActionResult FindUserReport(string ChosenLogin)
        {
            Repository<ApplicationUser> userRepository = new Repository<ApplicationUser>();
            var users = userRepository.GetList().Where(u => u.UserName == ChosenLogin).ToArray() ;
            if (users.Length == 1)
            {
                var user = users[0];
                return RedirectToAction("ShowUserReport","Admin", new { user.Id });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        public ActionResult ShowUserReport(string Id)
        {
            DateTime End = DateTime.Now.AddDays(-1).Date;
            DateTime Start = DateTime.Now.AddDays(-7).Date;
            return ShowUserReport(Id, Start, End);
        }

        [HttpPost]
        public ActionResult ShowUserReport(string Id, DateTime Start, DateTime End)
        {


            ApplicationContext context = new ApplicationContext();
            Repository<ApplicationUser> userRepository = new Repository<ApplicationUser>(context);
            var user = userRepository.GetList().First(u => u.Id == Id);
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
            ViewBag.User = user.UserName;
            return View(model);
        }

    }
}