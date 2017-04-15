using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActivityTracking.DAL.EntityFramework;
using ActivityTracking.DomainModel;
using ActivityTracking.WebClient.Models;

namespace ActivityTracking.WebClient.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            Repository<ApplicationUser> userRepository = new Repository<ApplicationUser>();
            var users = userRepository.GetList();

            Repository<Group> groupRepository = new Repository<Group>();

            SelectList groupList = new SelectList(groupRepository.GetList(), "Id", "Name");

            AdminViewModel viewModel = new AdminViewModel {GroupList = groupList, UsersList = users };
            return View(viewModel);
        }
        public ActionResult ChangeGroup(string Id, string GroupList)
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
    }
}