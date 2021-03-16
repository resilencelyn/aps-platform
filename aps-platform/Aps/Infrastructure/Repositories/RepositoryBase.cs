using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Aps.Infrastructure.Repositories
{
    /// <summary>
    /// 默认仓储的通用功能实现，用于所有的领域模型
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public class RepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        protected readonly ApsContext _apsContext;

        /// <summary>
        /// 通过反省，从数据上下文中获得领域模型
        /// </summary>
        public virtual DbSet<TEntity> Table => _apsContext.Set<TEntity>();


        public RepositoryBase(ApsContext apsContext)
        {
            _apsContext = apsContext;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return Table.AsQueryable();
        }

        public virtual IList<TEntity> GetAllList()
        {
            return GetAll().ToList();
        }

        public virtual async Task<List<TEntity>> GetAllListAsync()
        {
            return await GetAll().ToListAsync();
        }

        public virtual List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ToList();
        }

        public virtual async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).ToListAsync();
        }

        public virtual TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Single(predicate);
        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().SingleAsync(predicate);
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().FirstOrDefaultAsync(predicate);
        }

        public virtual TEntity Insert(TEntity entity)
        {
            var newEntity = Table.Add(entity).Entity;
            Save();

            return newEntity;
        }


        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            var entityEntry = await Table.AddAsync(entity);

            await SaveAsync();
            return entityEntry.Entity;
        }

        public virtual TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            _apsContext.Entry(entity).State = EntityState.Modified;
            Save();
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            AttachIfNot(entity);
            _apsContext.Entry(entity).State = EntityState.Modified;

            await SaveAsync();
            return entity;
        }

        public virtual void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
            Save();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
            await SaveAsync();
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var entity in GetAll().Where(predicate).ToList())
            {
                Delete(entity);
            }
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var entity in GetAll().Where(predicate).ToList())
            {
                await DeleteAsync(entity);
            }
        }

        public int Count()
        {
            return GetAll().Count();
        }

        public async Task<int> CountAsync()
        {
            return await GetAll().CountAsync();
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Count(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().CountAsync(predicate);
        }

        public long LongCount()
        {
            return GetAll().LongCount();
        }

        public async Task<long> LongCountAsync()
        {
            return await GetAll().LongCountAsync();
        }

        public long LongCount(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().LongCount(predicate);
        }

        public async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().LongCountAsync(predicate);
        }


        protected virtual void AttachIfNot(TEntity entity)
        {
            var entry = _apsContext.ChangeTracker.Entries().FirstOrDefault(ent => ent.Entity == entity);

            if (entry != null)
            {
                return;
            }

            Table.Attach(entity);
        }

        public void Save()
        {
            _apsContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _apsContext.SaveChangesAsync();
        }
    }
}