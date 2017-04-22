using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracking.GetUserInfo
{
    public static class UserInfo
    {
        public static UserInfoModel GetUserInformation()
        {
            UserInfoModel userinfo = new UserInfoModel
            {
                Company = "ISD",
                CommercialExperiance = "5",
                Department = "IndDep",
                Division = "Division",
                FirstName = "Alexandr",
                HireDate = "1.05.2017",
                ISDExperiance = "0.5 year",
                LastName = "Tkachuk",
                Login = "Altk",
                Manage = "manage",
                OfficeLocation = "Dnepr",
                Position = "c# developer",
                Profession = "professor",
                Role = "manager",
                Sector = "sector",
                Title = "title",
                Times = new List<Time>
                {
                   new Time { Date = new DateTime(2017, 3, 5), TimeIn = new DateTime(2017, 3, 5, 8, 0, 0), TimeOut = new DateTime(2017, 3, 5, 16, 20, 0), Login = "Altk" }
                }
            };

            return userinfo;
        }
    }
}
