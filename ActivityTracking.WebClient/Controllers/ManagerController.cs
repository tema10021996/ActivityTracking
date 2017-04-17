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

namespace ActivityTracking.WebClient.Controllers
{
    public class ManagerController : Controller
    {
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult ShowUsers()
        {
            Repository<ApplicationUser> userRepository = new Repository<ApplicationUser>();
            var user = userRepository.GetList().First(u => u.UserName == HttpContext.User.Identity.Name);
            Group group = user.Group;
            return View(group);
        }
        public ActionResult ChangeGroupMayAbsentTime(int Id, string Hours, string Minutes, string Seconds)
        {
            Repository<Group> groupRepository = new Repository<Group>();
            var group = groupRepository.GetList().First(g=>g.Id == Id);
            group.MayAbsentTime = new TimeSpan(Convert.ToInt32(Hours), Convert.ToInt32(Minutes), Convert.ToInt32(Seconds));
            groupRepository.Update(group);
            return RedirectToAction("ShowUsers");
        }

    }
}