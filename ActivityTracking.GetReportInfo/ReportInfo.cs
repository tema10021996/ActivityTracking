using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActivityTracking.DomainModel;

namespace ActivityTracking.GetReportInfo
{
    public class ReportInfo
    {
        //public UserInfoModel userInfo = new UserInfoModel();

        //public ReportInfo()
        //{
        //    this.userInfo = new UserInfoModel
        //    {
        //        UserId = 1,
        //        Login = "Qwe",
        //        CommercialExp = "Qwe1",
        //        Company = "Qwe2",
        //        Department = "Qwe3",
        //        Division = "Qwe4",
        //        FirstName = "Qwe5",
        //        HireDate = "Qwe6",
        //        ISD_Exp = "Qwe7",
        //        LastName = "Qwe8",
        //        Manager = "Qwe9",
        //        OfficeLocation = "Qwe10",
        //        Position = "Qwe11",
        //        Profession = "Qwe12",
        //        Role = "Qwe13",
        //        Sector = "Qwe14",
        //        Title = "Qwe15"
        //    };
        //}

        public string GetUserInfo()
        {
            return "UserId = 1, Login = Qwe, CommercialExp = Qwe1, Company = Qwe2, Department = Qwe3, Division = Qwe4, FirstName = Qwe5";
        }
    }
}
