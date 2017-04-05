using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracking.DAL.EntityFramework
{
    public class BaseRepository<TEntity, Key> : IRepository<TEntity, Key>
         where TEntity : class, new()
    {
        private readonly DbContext dbContext = default(DbContext);
        private bool isDisposed = false;

        public BaseRepository()
        {
            dbContext = Activator.CreateInstance(typeof(ApplicationContext)) as ApplicationContext;
        }

        public DbSet<TEntity> Set
        {
            get
            {
                return dbContext.Set<TEntity>();
            }
        }

        public void Delete(Key id)
        {
            var model = Get(id);
            if (model is TEntity)
            {
                this.Set.Remove(model);
                dbContext.SaveChanges();
            }
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate = null)
        {
            TEntity query = dbContext.Set<TEntity>().FirstOrDefault(predicate);
            return query;
        }

        public TEntity Get(Key id)
        {
            return this.dbContext.Set<TEntity>().Find(id);
        }

        public void Insert(TEntity entity)
        {
            this.Set.Add(entity);
            dbContext.SaveChanges();
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? pageNumber = default(int?), int? pageSize = default(int?))
        {
            IQueryable<TEntity> query = dbContext.Set<TEntity>();

            if (orderBy != null)
                query = orderBy(query);

            if (predicate != null)
                query = query.Where(predicate);

            if (pageNumber != null && pageSize != null)
                query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return query;
        }

        public IEnumerable<ValueType> SqlQuery<ValueType>(string sql, CommandType sqlType, object[] parameters)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            var entry = this.dbContext.Entry<TEntity>(entity);
            if (entry != null)
            {
                if (entry.State == EntityState.Detached)
                {
                    var attachedEntity = this.Set.Attach(entity);
                    entry = this.dbContext.Entry<TEntity>(attachedEntity);
                }
                entry.State = EntityState.Modified;
                dbContext.SaveChanges();
            }
            else
            {
                this.Insert(entity);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        ~BaseRepository()
        {
            Dispose(false);
        }

        private void Dispose(bool disposeManaged)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (disposeManaged && this.dbContext != null)
            {
                this.dbContext.Dispose();
            }

            this.isDisposed = true;
        }

        #endregion
    }
}

