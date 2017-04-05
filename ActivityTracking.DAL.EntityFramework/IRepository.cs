using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracking.DAL.EntityFramework
{
    public interface IRepository<TEntity, Key> : IDisposable
        where TEntity : class, new()
    {
        // [TransactionFlow(TransactionFlowOption.Allowed)]
        void Delete(Key id);

        // [TransactionFlow(TransactionFlowOption.Allowed)]
        TEntity Get(Key id);

        //[TransactionFlow(TransactionFlowOption.Allowed)]
        void Insert(TEntity entity);

        //[TransactionFlow(TransactionFlowOption.Allowed)]
        void Update(TEntity entity);

        // [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<ValueType> SqlQuery<ValueType>(string sql, CommandType sqlType, object[] parameters);

        //[TransactionFlow(TransactionFlowOption.Allowed)]
        IQueryable<TEntity> Query(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? pageNumber = null,
            int? pageSize = null);

        //[TransactionFlow(TransactionFlowOption.Allowed)]
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate = null);
    }
}
