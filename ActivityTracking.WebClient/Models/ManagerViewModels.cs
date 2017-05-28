using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActivityTracking.DomainModel;

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

        public bool BarChart { get; set; }

        public bool PieChart { get; set; }
        public bool ColumnChart { get; set; }
    }

    public class ManagerShowDepartmentReportByUsersViewModel
    {
        public List<WorkerInfo> WorkersInfos { get; set; }
        public List<String> ReasonsNames { get; set; }
        public List<ReasonInfo> ReasonInfosForPercentageReport { get; set; }

        [DataType(DataType.Date)]
        public DateTime Start { get; set; }

        [DataType(DataType.Date)]        
        public DateTime End { get; set; }

        public string ChosenDepartmentName { get; set; }
    }

    public class ManagerSettingsViewModel
    {
        public List<ReasonModel> AllReasonModels { get; set; }
        public int MayAbsentMinutes { get; set; }
    }

    public class ReasonModel
    {
        public Reason Reason { get; set; }
        public bool isChoosen { get; set; }
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
        public string Color { get; set; }

    }

}