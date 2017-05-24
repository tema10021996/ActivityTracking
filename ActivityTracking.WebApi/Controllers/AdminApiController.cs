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
using ActivityTracking.GetUserInfo;
using System.Collections;
using System.Net.Http;
using System.Web.Http;
using ActivityTracking.WebApi.Models;

namespace ActivityTracking.WebApi.Controllers
{
    public class AdminApiController : ApiController
    {
        private ApplicationUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
        }

        [HttpGet]
        public ShowDepartmentUsers
    }
}
