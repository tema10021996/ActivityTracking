using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ActivityTracking.DomainModel;
using System.Web.Mvc;

namespace ActivityTracking.WebClient.Models
{
    public class AdminIndexViewModel
    {
        public List<UserViewModel> UsersViewModelsList { get; set; }
        public SelectList GroupList { get; set; }
        public SelectList RoleList { get; set; }
    }
    public class UserViewModel
    {
        public ApplicationUser User { get; set; }
        public string UserRole { get; set; }
    }

    public class AdminShowGroupInfoViewModel
    {
        public Group Group { get; set; }
        public ApplicationUser GroupManager { get; set; }
    }

}