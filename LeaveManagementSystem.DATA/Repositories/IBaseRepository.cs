using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementSystem.DATA.Repositories
{
    public interface IBaseRepository<TEntity>
         where TEntity : class
    {
        TEntity GetById(object id);
        Task<TEntity> GetByIdAsync(object id);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
        IQueryable<TEntity> GetZ(Expression<Func<TEntity, bool>> filter);
        IQueryable<TEntity> GetZ(Expression<Func<TEntity, bool>> filter, string includeProperties = "");
        IEnumerable<TEntity> GetAsNoTracking(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
        Task<IEnumerable<TEntity>> GetAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
        IQueryable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();
        IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters);
        Task<IEnumerable<TEntity>> GetWithRawSqlAsync(string query, params object[] parameters);
        int Insert(TEntity entity);
        Task<int> InsertAsync(TEntity entity);
        int InsertMany(IEnumerable<TEntity> entities);
        int Delete(TEntity entity);
        int DeleteMany(IEnumerable<TEntity> entities);
        Task<int> DeleteAsync(TEntity entity);
        int Delete(object id);
        Task<int> DeleteAsync(object id);
        int Update(TEntity entity);
        Task<int> UpdateAsync(TEntity entity);
        bool Any(Expression<Func<TEntity, bool>> filter);
        int Count(Expression<Func<TEntity, bool>> filter = null);
        void UpdateWithoutSaveChanges(TEntity entity);
        void InsertWithoutSaveChanges(TEntity entity);
        void InsertManyWithoutSaveChanges(IEnumerable<TEntity> entities);
        int SaveChanges();

    }
}
