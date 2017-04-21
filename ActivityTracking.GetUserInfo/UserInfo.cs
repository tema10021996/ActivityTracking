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
                Title = "title"
            };

            return userinfo;
        }
    }
}
