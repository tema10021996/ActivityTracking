using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracking.DAL.EntityFramework
{
    public class GroupDomainModel : BaseDomainModel
    {
        public IEnumerable<ActivityTracking.DomainModel.Group> GetAll()
        {
            using (var repository = new Repository<ActivityTracking.DomainModel.Group>())
            {
                var list = repository.GetList();
                //var list = repository.Query().Select(x => new ActivityTracking.DomainModel.Group
                //{
                //    Id = x.Id,
                //    Name = x.Name,                    
                //    MayAbsentTime = x.MayAbsentTime                    
                //}).ToList();
                return list;
            }
        }
    }
}
