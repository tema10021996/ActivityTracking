using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ActivityTracking.DAL.EntityFramework
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        // [TransactionFlow(TransactionFlowOption.Allowed)]
        void Delete(int id);

        // [TransactionFlow(TransactionFlowOption.Allowed)]
        TEntity GetItem(int id);

        IEnumerable<TEntity> GetList();



        //[TransactionFlow(TransactionFlowOption.Allowed)]
        void Create(TEntity entity);

        //[TransactionFlow(TransactionFlowOption.Allowed)]
        void Update(TEntity entity);

        
    }
}
