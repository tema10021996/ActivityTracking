using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActivityTracking.DomainModel;

namespace ActivityTracking.DAL.EntityFramework
{
    public class GroupDomainModel : BaseDomainModel
    {
        public IEnumerable<Group> GetAll()
        {
            using (var repository = new Repository<Group>())
            {
                var list = repository.GetList();
                //var list = repository.GetList().Select(x => new ActivityTracking.DomainModel.Group
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
