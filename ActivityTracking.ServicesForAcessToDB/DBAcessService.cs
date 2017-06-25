using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActivityTracking.DAL.EntityFramework;
using System.Data.Entity;

namespace ActivityTracking.ServicesForAcessToDB
{
    public class DBAcessService<TEntity> where TEntity:class
    {
        ApplicationContext context;
        Repository<TEntity> repository;
        public DBAcessService(ApplicationContext Context)
        {
            context = Context;
            repository = new Repository<TEntity>(context);
        }

        public DBAcessService()
        {
            context = new ApplicationContext();
            repository = new Repository<TEntity>(context);
        }

        public TEntity GetItem(int id)
        {
            return repository.GetItem(id);
        }
        public IEnumerable<TEntity> GetList()
        {

            return repository.GetList();
        }

        public void Create(TEntity item)
        {
            repository.Create(item);
        }

        public void Update(TEntity item)
        {
            repository.Update(item);
        }

        public void Delete(int id)
        {
            repository.Delete(id);
        }
    }
}
