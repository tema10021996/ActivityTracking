using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ActivityTracking.DAL.EntityFramework;
using ActivityTracking.DomainModel;
using ActivityTracking.WebClient.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using ActivityTracking.GetUserInfo;
using System.Collections;
using System.Net.Http;
using System.Web.Http;
using ActivityTracking.WebApi.Models;

namespace ActivityTracking.WebApi.Controllers
{
    public class AdminApiController : ApiController
    {
        [HttpGet]
        public List<string> ViewIndex ()
        {
            List<string> list = new List<string>() { "12334234345", "12eqweer32we" };
            //AdminController admin = new AdminController();
            return list;
            //return admin.Index();
        }

        [HttpPost]
        public bool AddReason(List<string> list)
        {
            string[] ar = list.ToArray();
            AdminController admin = new AdminController();
            admin.AddReason(ar[0], ar[1]);

            return true;
        }

    }
}
