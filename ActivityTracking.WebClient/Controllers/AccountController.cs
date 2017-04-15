using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ActivityTracking.WebClient.Models;
using ActivityTracking.DomainModel;
using ActivityTracking.DAL.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;

namespace ActivityTracking.WebClient.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();}
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Repository<UserLogin> userLoginsRepository = new Repository<UserLogin>();
                var userLogin = userLoginsRepository.GetList().FirstOrDefault(u => u.Login == model.Login);
                if (userLogin != null)
                {


                    ApplicationUser user = new ApplicationUser { UserName = model.Login };
                    IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await UserManager.AddToRoleAsync(user.Id, "user");
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        foreach (string error in result.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                    }
                }
            }
            return View(model);
        }
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;          
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindAsync(model.Login, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Not correct Login or Password");
                }
                else
                {
                    ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = true }, claim);
                    if (String.IsNullOrEmpty(returnUrl))
                    {
 
                        if (user.Roles.FirstOrDefault(f => f.RoleId == "1") != null)
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        if (user.Roles.FirstOrDefault(f => f.RoleId == "2") != null)
                        {
                            return RedirectToAction("Index", "User");
                        }                      
                        if (user.Roles.FirstOrDefault(f => f.RoleId == "3") != null)
                        {
                            return RedirectToAction("Index", "Manager");
                        }
                    }
                    return Redirect(returnUrl);
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login");
        }

        public string AccountInfo()
        {
            return "Your information";
        }


    }
}