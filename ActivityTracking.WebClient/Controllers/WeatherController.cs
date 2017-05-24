using ActivityTracking.DAL.EntityFramework;
using ActivityTracking.DomainModel;
using ActivityTracking.WebClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ActivityTracking.WebClient.Controllers
{
    public class WeatherController : ApiController
    {
        //The data should come from database but here I am hard coding it.    
        public static List<String> reports = new List<String>
        {
            "weewr","werewrewrew","sssssss"
        };

        [HttpGet]
        public List<String> Get()
        {
            return reports;
        }

        [HttpGet]
        public String Get(int id)
        {
            return reports.First();
        }

        [HttpPost]
        public bool Post(PostModel postModel)
        {
            ApplicationContext context = new ApplicationContext();
            Repository<ApplicationUser> userRepository = new Repository<ApplicationUser>(context);
            Repository<Absence> absenseRepository = new Repository<Absence>(context);
            if (userRepository.GetList().First(u => u.UserName == postModel.UserName) != null)
            {
                //ApplicationUser user = userRepository.GetList().First(u => u.UserName == "AlexandrTkachuk");

                ApplicationUser user = userRepository.GetList().First(u => u.UserName == postModel.UserName);
                absenseRepository.Create(new Absence { StartAbsence = postModel.Start, User = user, Date = DateTime.Today });
                return true;
            }
            else
            {
                return false;
            }
        }

        //[HttpDelete]
        //public bool Delete(int id)
        //{
        //    try
        //    {
        //        var itemToRemove = reports.Find((r) => r.id == id);
        //        reports.Remove(itemToRemove);
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
    }
}
