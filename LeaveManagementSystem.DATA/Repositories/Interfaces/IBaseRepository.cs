using System.Linq.Expressions;

namespace LeaveManagementSystem.DATA.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity>
         where TEntity : class
    {
        Task<TEntity> GetByIdAsync(object id);
        IQueryable<TEntity> GetZ(Expression<Func<TEntity, bool>> filter);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<int> InsertAsync(TEntity entity);
        Task<int> UpdateAsync(TEntity entity);
        Task<int> DeleteAsync(TEntity entity);

    }
}
