using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActivityTracking.GetUserInfo;

namespace ActivityTracking.GetUserInfo
{
    public class UserInfoModel
    {
        public string Company { get; set; }
        public string OfficeLocation { get; set; }
        public string Login { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Sector { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string Profession { get; set; }
        public string Title { get; set; }
        public string Position { get; set; }
        public string Role { get; set; }
        public string Manage { get; set; }
        public string HireDate { get; set; }
        public string ISDExperiance { get; set; }
        public string CommercialExperiance { get; set; }

        public List<Time> Times { get; set; }
    }        
}
