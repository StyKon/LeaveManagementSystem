using System.Linq.Expressions;
using LeaveManagementSystem.DATA.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.DATA.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext context;
        protected readonly DbSet<TEntity> dbSet;

        public BaseRepository(DbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.dbSet = context.Set<TEntity>();
        }

        public virtual async Task<TEntity> GetByIdAsync(object id) => await dbSet.FindAsync(id);

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync() => await dbSet.ToListAsync();

        public virtual async Task<int> InsertAsync(TEntity entity)
        {
            dbSet.Add(entity);
            return await context.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteAsync(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);

            dbSet.Remove(entity);
            return await context.SaveChangesAsync();
        }

        public virtual async Task<int> UpdateAsync(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }

        public IQueryable<TEntity> GetZ(Expression<Func<TEntity, bool>> filter)
        {
            return dbSet.Where(filter);
        }

    }
}
