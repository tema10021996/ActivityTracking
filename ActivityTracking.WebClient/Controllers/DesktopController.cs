using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ActivityTracking.DomainModel;
using ActivityTracking.DAL.EntityFramework;
using ActivityTracking.WebClient.Models;

namespace ActivityTracking.WebClient.Controllers
{
    public class DesktopController: ApiController
    {

        [HttpGet]
        public List<String> GetReasonsNames(string id)
         {
            Repository<ApplicationUser> userRepository = new Repository<ApplicationUser>();
            if (userRepository.GetList().First(u => u.UserName == id) != null)
            {
                string divisionManagerName = GetUserInfo.UserInfo.GetDivisionManagerOfUser(id);
                Repository<DivisionManager> divManagerRepository = new Repository<DivisionManager>();
                DivisionManager divisionManger = divManagerRepository.GetList().First(m => m.Login == divisionManagerName);
                List<string> reasonsNames = new List<string>();
                foreach (Reason reason in divisionManger.Reasons)
                {
                    reasonsNames.Add(reason.Name);
                }
                return reasonsNames;
            }
            else
            {
                return new List<string>() { "Meeting", "English" };
            }

        }

        [HttpGet]
        public int GetMayAbsentMinutes(string id)
        {
            Repository<ApplicationUser> userRepository = new Repository<ApplicationUser>();
            if (userRepository.GetList().First(u => u.UserName == id) != null)
            {
                string divisionManagerName = GetUserInfo.UserInfo.GetDivisionManagerOfUser(id);
                Repository<DivisionManager> divManagerRepository = new Repository<DivisionManager>();
                DivisionManager divisionManger = divManagerRepository.GetList().First(m => m.Login == divisionManagerName);
                return divisionManger.MayAbsentMinutes;
            }
            else
            {
                return 20;
            }

        }


        [HttpPost]
        public bool CreateAbsence(PostModel postModel)
        {
            ApplicationContext context = new ApplicationContext();
            Repository<ApplicationUser> userRepository = new Repository<ApplicationUser>(context);
            Repository<Absence> absenseRepository = new Repository<Absence>(context);
            if (userRepository.GetList().First(u => u.UserName == postModel.UserName) != null)
            {
                ApplicationUser user = userRepository.GetList().First(u => u.UserName == postModel.UserName);
                absenseRepository.Create(new Absence { StartAbsence = postModel.StartAbsence, User = user, Date = DateTime.Today });
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPut]
        public bool UpdateAbsence(PutModel putModel)
        {
            ApplicationContext context = new ApplicationContext();
            Repository<Reason> reasonsRepository = new Repository<Reason>(context);
            Reason reason = null;
            reason = reasonsRepository.GetList().First(r => r.Name == putModel.ReasonName);
            Repository<Absence> absenseRepository = new Repository<Absence>(context);
            Absence absence = absenseRepository.GetList().Last(a => a.User.UserName == putModel.UserName);
            if (absence.EndAbsence == null)
            {
                absence.Reason = reason;
                absence.EndAbsence = putModel.EndAbsence;
                if (putModel.Comment != null)
                {
                    absence.Comment = putModel.Comment;
                }
                absenseRepository.Update(absence);
                return true;
            }
            else
            {
                return false;
            }
            
        }
        
    }
}
