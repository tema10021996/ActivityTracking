using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ActivityTracking.DomainModel;

namespace ActivityTracking.WebClient.Controllers
{
    public class DescktopController: ApiController
    {
        public static List<Absence> reports = new List<Absence>();

        [HttpGet]
        public List<Absence> Get()
        {
            return reports;
        }

        [HttpGet]
        public Absence Get(int id)
        {
            return reports.Find((r) => r.UserId == id);
        }

        [HttpPost]
        public bool Post(Absence report)
        {
            try
            {
                reports.Add(report);
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpDelete]
        public bool Delete(int id)
        {
            try
            {
                var itemToRemove = reports.Find((r) => r.UserId == id);
                reports.Remove(itemToRemove);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
