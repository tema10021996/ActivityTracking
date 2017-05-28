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
        public static List<string> reports = new List<string>
        {
            "aaaa", "sssssss"
        };

        [HttpGet]
        public List<string> Get()
        {
            return reports;
        }

        [HttpGet]
        public string Get(int id)
        {
            return reports.First();
        }

        [HttpPost]
        public bool Post(PostModel report)
        {
            //try
            //{
            //    Weather w = new Weather { id = 1, City = report.Start.ToShortTimeString(), Temperature = report.Date.ToString(), Humidity = report.UserName, Precipitation = "0%", Wind = "15mph" };
            //    reports.Add(w);
            //    return true;
            //}
            //catch
            //{
                return false;
            //}
        }

        [HttpDelete]
        public bool Delete(int id)
        {
            //try
            //{
            //    var itemToRemove = reports.Find((r) => r.id == id);
            //    reports.Remove(itemToRemove);
            //    return true;
            //}
            //catch
            //{
                return false;
            //}
        }
    }
}
