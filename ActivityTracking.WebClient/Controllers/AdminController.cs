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

        public ActionResult ChangeGroupMayAbsentTime(string Hours, string Minutes, string Seconds)
        {
            Repository<Group> groupRepository = new Repository<Group>();
            var groups = groupRepository.GetList();
            foreach (var group in groups)
            {
                group.MayAbsentTime = new TimeSpan(Convert.ToInt32(Hours), Convert.ToInt32(Minutes), Convert.ToInt32(Seconds));
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
    }
}