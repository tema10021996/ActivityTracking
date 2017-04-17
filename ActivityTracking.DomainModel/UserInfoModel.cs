using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracking.DomainModel
{
    public class UserInfoModel
    {
        public int? UserId { get; set; }
        public string Company { get; set; }
        public string OfficeLocation { get; set; }
        public string Login { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Sector { get; set; }
        public string Division { get; set; }
        public string Department { get; set; }
        public string Profession { get; set; }
        public string Title { get; set; }
        public string Position { get; set; }
        public string Role { get; set; }
        public string Manager { get; set; }
        public string HireDate { get; set; }
        public string CommercialExp { get; set; }
        public string ISD_Exp { get; set; }

        public override string ToString()
        {
            return UserId+" "+Company;
        }
    }
}
