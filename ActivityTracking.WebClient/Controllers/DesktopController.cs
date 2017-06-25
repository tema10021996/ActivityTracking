using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ActivityTracking.DomainModel;
using ActivityTracking.ServicesForAcessToDB;
using ActivityTracking.WebClient.Models;

namespace ActivityTracking.WebClient.Controllers
{
    public class DesktopController: ApiController
    {
        [HttpGet]
        public List<String> GetReasonsNames(string id)
         {
            DBAcessService<ApplicationUser> applicationUserService = new DBAcessService<ApplicationUser>();
            if (applicationUserService.GetList().First(u => u.UserName == id) != null)
            {
                string divisionManagerName = GetUserInfo.UserInfo.GetDivisionManagerOfUser(id);
                DBAcessService<DivisionManager> divManagerService = new DBAcessService<DivisionManager>();
                DivisionManager divisionManger = divManagerService.GetList().First(m => m.Login == divisionManagerName);
                List<string> reasonsNames = new List<string>();
                foreach (Reason reason in divisionManger.Reasons)
                {
                    reasonsNames.Add(reason.Name);
                }
                return reasonsNames;
            }
            else
            {
                return new List<string>() {"Default" };
            }
        }

        [HttpGet]
        public int GetMayAbsentMinutes(string id)
        {
            DBAcessService<ApplicationUser> applicationUserService = new DBAcessService<ApplicationUser>();
            if (applicationUserService.GetList().First(u => u.UserName == id) != null)
            {
                string divisionManagerName = GetUserInfo.UserInfo.GetDivisionManagerOfUser(id);
                DBAcessService<DivisionManager> divManagerService = new DBAcessService<DivisionManager>();
                DivisionManager divisionManger = divManagerService.GetList().First(m => m.Login == divisionManagerName);
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
            UnitOfWork unitOfWork = new UnitOfWork();
            if (unitOfWork.ApplicationUserService.GetList().First(u => u.UserName == postModel.UserName) != null)
            {
                ApplicationUser user = unitOfWork.ApplicationUserService.GetList().First(u => u.UserName == postModel.UserName);
                unitOfWork.AbsenceService.Create(new Absence { StartAbsence = postModel.StartAbsence, User = user, Date = DateTime.Today });
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
            UnitOfWork unitOfWork = new UnitOfWork();           
            Reason reason = null;
            reason = unitOfWork.ReasonService.GetList().First(r => r.Name == putModel.ReasonName);;
            Absence absence = unitOfWork.AbsenceService.GetList().Last(a => a.User.UserName == putModel.UserName);
            if (absence.EndAbsence == null)
            {
                absence.Reason = reason;
                absence.EndAbsence = putModel.EndAbsence;
                if (putModel.Comment != null)
                {
                    absence.Comment = putModel.Comment;
                }
                unitOfWork.AbsenceService.Update(absence);
                return true;
            }
            else
            {
                return false;
            }
            
        }
        
    }
}
