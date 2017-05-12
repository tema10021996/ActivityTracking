using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ActivityTracking.WebClient.Models
{
    public class ManagerShowUsersViewModel
    {
        public List<string> UsersNames { get; set; }
        public string DepartmentName { get; set; }
    }
    public class ChooseDepartmentViewModel
    {
        public SelectList DepartmentList { get; set; }
    }

    public class ManagerShowDepartmentReportViewModel
    {
        public List<ReasonInfo> ReasonInfos { get; set; }
        public List<String> ReasonsNames { get; set; }

        [DataType(DataType.Date)]
        public DateTime Start { get; set; }

        [DataType(DataType.Date)]
        public DateTime End { get; set; }

        public string ChosenDepartmentName { get; set; }
    }

    public class ManagerShowGroupReportByUsersViewModel
    {
        public List<WorkerInfo> WorkersInfos { get; set; }
        public List<String> ReasonsNames { get; set; }

        [DataType(DataType.Date)]
        public DateTime Start { get; set; }

        [DataType(DataType.Date)]        
        public DateTime End { get; set; }

        public string ChosenDepartmentName { get; set; }
    }
    public class WorkerInfo
    {
        public string Name { get; set; }
        public List<ReasonInfo> ReasonInfos { get; set; }
        
    }
    public class ReasonInfo
    {
        public string ReasonName { get; set; }
        public double DurationInHours { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds{ get; set; }

    }

}