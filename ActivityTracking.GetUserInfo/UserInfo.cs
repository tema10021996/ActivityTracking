using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracking.GetUserInfo
{
    public static class UserInfo
    {
        public static List<UserInfoModel> GetUserInformation(string groupName, string login, DateTime Start, DateTime End)
        {
            List<UserInfoModel> returnUsersInformaion = new List<UserInfoModel>();

            List<WorkTime> list = new List<WorkTime>
                {
                    new WorkTime {TimeIn = new DateTime(2017, 4, 21, 8, 40, 0), TimeOut = new DateTime(2017, 4, 21, 10, 20, 0)} ,
                    new WorkTime {TimeIn = new DateTime(2017, 4, 21, 10, 40, 0), TimeOut = new DateTime(2017, 4, 21, 17, 20, 0)},
                    
                    new WorkTime {TimeIn = new DateTime(2017, 4, 22, 8, 0, 0), TimeOut = new DateTime(2017, 4, 22, 14, 0, 0)},
                    new WorkTime {TimeIn = new DateTime(2017, 4, 22, 15, 0, 0), TimeOut = new DateTime(2017, 4, 22, 16, 50, 0) },

                    new WorkTime {TimeIn = new DateTime(2017, 4, 23, 8, 55, 0), TimeOut = new DateTime(2017, 4, 23, 10, 0, 0)},
                    new WorkTime {TimeIn = new DateTime(2017, 4, 23, 10, 55, 0), TimeOut = new DateTime(2017, 4, 23, 12, 50, 0)},
                    new WorkTime {TimeIn = new DateTime(2017, 4, 23, 13, 0, 0), TimeOut = new DateTime(2017, 4, 23, 16, 50, 0)},

                    new WorkTime {TimeIn = new DateTime(2017, 4, 24, 8, 15, 0), TimeOut = new DateTime(2017, 4, 24, 12, 20, 0)},
                    new WorkTime { TimeIn = new DateTime(2017, 4, 24, 12, 50, 0), TimeOut = new DateTime(2017, 4, 24, 16, 20, 0)},

                    new WorkTime {TimeIn = new DateTime(2017, 4, 25, 8, 40, 0), TimeOut = new DateTime(2017, 4, 25, 10, 20, 0)},
                    new WorkTime {TimeIn = new DateTime(2017, 4, 25, 10, 40, 0), TimeOut = new DateTime(2017, 4, 25, 17, 20, 0)},

                    new WorkTime {TimeIn = new DateTime(2017, 4, 26, 8, 0, 0), TimeOut = new DateTime(2017, 4, 26, 14, 0, 0)},
                    new WorkTime {TimeIn = new DateTime(2017, 4, 26, 15, 0, 0), TimeOut = new DateTime(2017, 4, 26, 16, 50, 0)},

                    new WorkTime {TimeIn = new DateTime(2017, 4, 27, 8, 50, 0), TimeOut = new DateTime(2017, 4, 27, 17, 40, 0)},

                    new WorkTime {TimeIn = new DateTime(2017, 4, 28, 8, 55, 0), TimeOut = new DateTime(2017, 4, 28, 10, 0, 0)},
                    new WorkTime {TimeIn = new DateTime(2017, 4, 28, 10, 55, 0), TimeOut = new DateTime(2017, 4, 28, 12, 50, 0)},
                    new WorkTime {TimeIn = new DateTime(2017, 4, 28, 13, 0, 0), TimeOut = new DateTime(2017, 4, 28, 16, 50, 0)},
                };

            if (groupName == null)
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
                    Login = "AlexandrTkachuk",
                    Manager = "AlexandraMorozova",
                    OfficeLocation = "Dnepr",
                    Position = "c# developer",
                    Profession = "professor",
                    Role = "manager",
                    Sector = "sector",
                    Title = "title",
                    WorkTimes = list.Where(u=>u.TimeIn.Date >= Start && u.TimeIn.Date <= End).ToList()
                };
                returnUsersInformaion.Add(userinfo);
            }

            else
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
                    Login = "AlexandrTkachuk",
                    Manager = "AlexandraMorozova",
                    OfficeLocation = "Dnepr",
                    Position = "c# developer",
                    Profession = "professor",
                    Role = "manager",
                    Sector = "sector",
                    Title = "title",
                    WorkTimes = list.Where(u => u.TimeIn.Date >= Start && u.TimeIn.Date <= End).ToList()
                };
                returnUsersInformaion.Add(userinfo);

            }
            
            return returnUsersInformaion;
            
        }
    }
}

            

