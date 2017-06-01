using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActivityTracking.DomainModel;
using ActivityTracking.DAL.EntityFramework;

namespace ActivityTracking.GetUserInfo
{
    public static class UserInfo
    {
        #region GetUserInformation
        private static List<UserInformation> GetUserInformation(string login, string departmentName)
        {
            List<UserInformation> AllUserInfoModels = new List<UserInformation>() {
                new UserInformation
                {
                        Company = "ISD",
                        CommercialExperiance = "5",
                        Department = "Department1",
                        Division = "Division",
                        FirstName = "Alexandr",
                        HireDate = "1.05.2017",
                        ISDExperiance = "0.5 year",
                        LastName = "Tkachuk",
                        Login = "Alexandr",
                        Manager = "AlexandraMorozova",
                        OfficeLocation = "Dnepr",
                        Position = "c# developer",
                        Profession = "professor",
                        Role = "user",
                        Sector = "sector",
                        Title = "title"
                },
                new UserInformation
                {
                        Company = "ISD",
                        CommercialExperiance = "5",
                        Department = "Department1",
                        Division = "Division",
                        FirstName = "Nikita",
                        HireDate = "1.05.2017",
                        ISDExperiance = "0.5 year",
                        LastName = "Maltsev",
                        Login = "NikitaMaltsev",
                        Manager = "AlexandraMorozova",
                        OfficeLocation = "Dnepr",
                        Position = "c# developer",
                        Profession = "professor",
                        Role = "user",
                        Sector = "sector",
                        Title = "title"
                },
                new UserInformation
                {
                        Company = "ISD",
                        CommercialExperiance = "5",
                        Department = "Department1",
                        Division = "Division",
                        FirstName = "Artem",
                        HireDate = "1.05.2017",
                        ISDExperiance = "0.5 year",
                        LastName = "Chuhalo",
                        Login = "ArtemChuhalo",
                        Manager = "AlexandraMorozova",
                        OfficeLocation = "Dnepr",
                        Position = "c# developer",
                        Profession = "professor",
                        Role = "user",
                        Sector = "sector",
                        Title = "title"
                },
                new UserInformation
                {
                        Company = "ISD",
                        CommercialExperiance = "5",
                        Department = "Department1",
                        Division = "Division",
                        FirstName = "Ivan",
                        HireDate = "1.05.2017",
                        ISDExperiance = "0.5 year",
                        LastName = "Ivanov",
                        Login = "IvanIvanov",
                        Manager = "AlexandraMorozova",
                        OfficeLocation = "Dnepr",
                        Position = "c# developer",
                        Profession = "professor",
                        Role = "user",
                        Sector = "sector",
                        Title = "title"
                },
                new UserInformation
                {
                        Company = "ISD",
                        CommercialExperiance = "5",
                        Department = "Department1",
                        Division = "Division",
                        FirstName = "Max",
                        HireDate = "1.05.2017",
                        ISDExperiance = "0.5 year",
                        LastName = "Maximov",
                        Login = "MaxMaximov",
                        Manager = "AlexandraMorozova",
                        OfficeLocation = "Dnepr",
                        Position = "c# developer",
                        Profession = "professor",
                        Role = "user",
                        Sector = "sector",
                        Title = "title"
                },
                 new UserInformation
                {
                        Company = "ISD",
                        CommercialExperiance = "5",
                        Department = "Department1",
                        Division = "Division",
                        FirstName = "Alex",
                        HireDate = "1.05.2014",
                        ISDExperiance = "3 year",
                        LastName = "Morozova",
                        Login = "omor",
                        Manager = "AlexandraMorozova",
                        OfficeLocation = "Dnepr",
                        Position = "c# developer",
                        Profession = "professor",
                        Role = "user",
                        Sector = "sector",
                        Title = "title"
                },
                new UserInformation
                {
                        Company = "ISD",
                        CommercialExperiance = "5",
                        Department = "Department28",
                        Division = "Division",
                        FirstName = "Alexandra",
                        HireDate = "1.05.2017",
                        ISDExperiance = "0.5 year",
                        LastName = "Morozova",
                        Login = "AlexandraMorozova",
                        Manager = "ValeriyPavlovich",
                        OfficeLocation = "Dnepr",
                        Position = "c# developer",
                        Profession = "professor",
                        Role = "manager",
                        Sector = "sector",
                        Title = "title"
                }
            };
            List<UserInformation> result = new List<UserInformation>();
            if (departmentName == null)
            {
                result.Add(AllUserInfoModels.First(u => u.Login == login));
                return result;
            }
            else
            {
                foreach (var userInfoModel in AllUserInfoModels)
                {
                    if (userInfoModel.Department == departmentName)
                    {
                        result.Add(userInfoModel);
                    }
                }
                return result;
            }
        }
        #endregion

        #region  GetUserOrDepartmentIformation
        public static List<UserInfoModel> GetUserOrDepartmentIformation(string departmentName, string login, DateTime start, DateTime end)
        {
            List<UserInfoModel> returnInformation = new List<UserInfoModel>();

            if (departmentName == null)
            {
                UserInfoModel userinfo = new UserInfoModel
                {
                    userInformarion = GetUserInformation(login, null).First(),
                    WorkTimes = GetUserWorkTimes(login, start, end)
                };
                returnInformation.Add(userinfo);
            }
            else
            {
                var usersInformationsFromDepartment = GetUserInformation(null, departmentName);
                foreach (var tempUserInfo in usersInformationsFromDepartment)
                {
                    UserInfoModel userinfo = new UserInfoModel
                    {
                        userInformarion = tempUserInfo,
                        WorkTimes = GetUserWorkTimes(tempUserInfo.Login, start, end)
                    };
                    returnInformation.Add(userinfo);

                }

            }

            return returnInformation;            
        }
        #endregion

        #region GetUserWorkTimes
        private static List<WorkTime> GetUserWorkTimes(string login, DateTime Start, DateTime End)
        {
            List<WorkTime> AllWorkTimeslist = new List<WorkTime>
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

            List<WorkTime> resultWorkTimeList = new List<WorkTime>();
            resultWorkTimeList = AllWorkTimeslist.Where(u => u.TimeIn.Date >= Start && u.TimeIn.Date <= End).ToList();
            if (resultWorkTimeList.Count > 0)
            {
                return resultWorkTimeList;
            }
            else
            {
                return null; 
            }

        }
        #endregion

        #region GetManagerDepartments
        public static List<string> GetManagerDepartments(string ManagerLogin)
        {
            List<String> departmentsList = new List<string>() { "Department1", "Department2", "Department3", "Department4" };
            return departmentsList;
        }
        #endregion

        #region GetAllDivisionManagers
        public static List<string> GetAllDivisionManagers()
        {
            List<String> divManagersList = new List<string>() { "AlexandraMorozova", "DivManager2", "DivManager3", "DivManager4", "DivManager5", "DivManager6", "DivManager7" };
            return divManagersList;
        }
        #endregion

        #region GetDivisionManagerOfUser
        public static string GetDivisionManagerOfUser(string userLogin)
        {
            string divManager = "AlexandraMorozova";
            return divManager;
        }
        #endregion

        #region GetAllUsersLogins
        public static List<String> GetAllUsersLogins()
        {
            return new List<string>() { "AlexandrT", "NikitaMaltsev", "ArtemChuhalo", "IvanIvanov", "MaxMaximov", "omor", "AlexandraMorozova" };
        }
        #endregion

    }
}

            

