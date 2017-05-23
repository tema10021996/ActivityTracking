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
        public List<String> GetReasonsNames(string userName)
        {
            Repository<ApplicationUser> userRepository = new Repository<ApplicationUser>();
            if (userRepository.GetList().First(u => u.UserName == userName) != null)
            {
                string divisionManagerName = GetUserInfo.UserInfo.GetDivisionManagerOfUser(userName);
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
                return new List<string>() {"Meeting", "English"};
            }
            
        }

        [HttpPost]
        public bool CreateAbsence(PostModel postModel)
        {
            Repository<ApplicationUser> userRepository = new Repository<ApplicationUser>();
            Repository<Absence> absenseRepository = new Repository<Absence>();
            if (userRepository.GetList().First(u => u.UserName == postModel.UserName) != null)
            {
                    ApplicationUser user = userRepository.GetList().First(u => u.UserName == "AlexandrTkachuk");

                //ApplicationUser user = userRepository.GetList().First(u => u.UserName == postModel.UserName);
                absenseRepository.Create(new Absence { StartAbsence = postModel.Start, User = user, Date = DateTime.Today });
                return true;
            }
            else
            {
                return false;
            }
            
        }

        [HttpPut]
        public bool UpdateAbsence(DateTime endAbsence, string userName,string reasonName)
        {
            Repository<Reason> reasonsRepository = new Repository<Reason>();
            Reason reason = reasonsRepository.GetList().First(r => r.Name == reasonName);
            Repository<Absence> absenseRepository = new Repository<Absence>();
            Absence absence = absenseRepository.GetList().Last(a => a.User.UserName == userName);
            absence.Reason = reason;
            absence.EndAbsence = endAbsence;
            absenseRepository.Update(absence);
            return true;
        }
        
    }
}
