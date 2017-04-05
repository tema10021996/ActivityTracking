using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ActivityTracking.DAL.EntityFramework
{
    public class GroupModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan MayAbsentTime { get; set; }
    }
}
