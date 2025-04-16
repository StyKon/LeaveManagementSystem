using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        public virtual IQueryable<TEntity> Fetch() => dbSet;

        public virtual TEntity GetById(object id) => dbSet.Find(id);

        public virtual async Task<TEntity> GetByIdAsync(object id) => await dbSet.FindAsync(id);

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

            return orderBy != null ? orderBy(query).ToList() : query.ToList();
        }

        public virtual IQueryable<TEntity> GetZ(Expression<Func<TEntity, bool>> filter, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet.Where(filter);

            foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

            return query;
        }

        public virtual IEnumerable<TEntity> GetAsNoTracking(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

            return orderBy != null ? orderBy(query).ToList() : query.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

            return orderBy != null ? await orderBy(query).ToListAsync() : await query.ToListAsync();
        }

        public virtual IQueryable<TEntity> GetAll() => dbSet;

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync() => await dbSet.ToListAsync();

        public virtual bool Any(Expression<Func<TEntity, bool>> filter) => dbSet.Any(filter);

        public virtual int Count(Expression<Func<TEntity, bool>> filter = null) =>
            filter == null ? dbSet.Count() : dbSet.Count(filter);

        public virtual int Insert(TEntity entity)
        {
            dbSet.Add(entity);
            return context.SaveChanges();
        }

        public virtual async Task<int> InsertAsync(TEntity entity)
        {
            dbSet.Add(entity);
            return await context.SaveChangesAsync();
        }

        public virtual int InsertMany(IEnumerable<TEntity> entities)
        {
            dbSet.AddRange(entities);
            return context.SaveChanges();
        }

        public virtual void InsertManyWithoutSaveChanges(IEnumerable<TEntity> entities)
        {
            dbSet.AddRange(entities);
        }

        public virtual int Delete(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);

            dbSet.Remove(entity);
            return context.SaveChanges();
        }

        public virtual int DeleteMany(IEnumerable<TEntity> entities)
        {
            dbSet.RemoveRange(entities);
            return context.SaveChanges();
        }

        public virtual async Task<int> DeleteAsync(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);

            dbSet.Remove(entity);
            return await context.SaveChangesAsync();
        }

        public virtual int Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            return Delete(entityToDelete);
        }

        public virtual async Task<int> DeleteAsync(object id)
        {
            TEntity entityToDelete = await dbSet.FindAsync(id);
            return await DeleteAsync(entityToDelete);
        }

        public virtual int Update(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            return context.SaveChanges();
        }

        public virtual async Task<int> UpdateAsync(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }

        public virtual int SaveChanges() => context.SaveChanges();

        public void UpdateWithoutSaveChanges(TEntity entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public void InsertWithoutSaveChanges(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters)
        {
            return dbSet.FromSqlRaw(query, parameters).ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetWithRawSqlAsync(string query, params object[] parameters)
        {
            return await dbSet.FromSqlRaw(query, parameters).ToListAsync();
        }

        public IQueryable<TEntity> GetZ(Expression<Func<TEntity, bool>> filter)
        {
            return dbSet.Where(filter);
        }

    }
}
