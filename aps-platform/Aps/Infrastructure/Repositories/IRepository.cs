using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Aps.Shared.Model;
using Microsoft.AspNetCore.Mvc;

namespace Aps.Infrastructure.Repositories
{
    /// <summary>
    /// 此接口是所有仓储的约定，此接口仅作为约定，用于标记它们
    /// </summary>
    /// <typeparam name="TEntity">当前传入仓储的实体类型</typeparam>
    /// <typeparam name="TPrimaryKey">传入仓储的主键类别</typeparam>
    public interface IRepository<TEntity, TPrimaryKey> where TEntity : class
    {
        #region 查询

        /// <summary>
        /// 获取用于从整个表中检索实体的IQueryable
        /// </summary>
        /// <returns>可用于从数据库中选择实体</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// 用于获取所有实体
        /// </summary>
        /// <returns>所有实体列表</returns>
        IList<TEntity> GetAllList();

        /// <summary>
        /// 用于获取所有实体的异步实现
        /// </summary>
        /// <returns>所有实体列表</returns>
        Task<List<TEntity>> GetAllListAsync();

        /// <summary>
        /// 用于获取传入本方法的所有实体<paramref name="predicate"/>
        /// </summary>
        /// <param name="predicate">筛选实体的条件</param>
        /// <returns>所有实体列表</returns>
        List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 用于获取传入本方法的所有实体<paramref name="predicate"/>
        /// </summary>
        /// <param name="predicate">筛选实体的条件</param>
        /// <returns>所有实体列表</returns>
        Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 通过传入的筛选条件来获取实体信息，如果查询不到返回值，则会引发异常
        /// </summary>
        /// <param name="predicate">Entity</param>
        TEntity Single(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 通过传入的筛选条件来获取实体信息，如果查询不到返回值，则会引发异常
        /// </summary>
        /// <param name="predicate">Entity</param>
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 通过传入的筛选条件来获取实体信息，如果没有找到，则返回null
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 通过传入的筛选条件来获取实体信息，如果没有找到，则返回null
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion

        #region Insert

        /// <summary>
        /// 添加一个新实体信息 
        /// </summary>
        /// <param name="entity">被添加的实体</param>
        /// <returns></returns>
        TEntity Insert(TEntity entity);

        /// <summary>
        /// 添加一个新实体信息 
        /// </summary>
        /// <param name="entity">被添加的实体</param>
        /// <returns></returns>
        Task<TEntity> InsertAsync(TEntity entity);

        #endregion

        #region Update

        /// <summary>
        /// 更新现有实体 
        /// </summary>
        /// <param name="entity">Entity</param>
        TEntity Update(TEntity entity);

        /// <summary>
        /// 更新现有实体 
        /// </summary>
        /// <param name="entity">Entity</param>
        Task<TEntity> UpdateAsync(TEntity entity);

        #endregion

        #region Delete

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(TEntity entity);

        /// <summary>
        /// 删除一个实体
        /// </summary>
        /// <param name="entity">Entity</param>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// 按传日的条件可删除多个实体
        /// 注意：所有符合给定条件的实体都将被检索和删除
        /// 如果条件比较多，则待删除的实体也比较多，这可能会导致主要的性能问题
        /// </summary>
        /// <param name="predicate">Entity</param>
        void Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 按传日的条件可删除多个实体
        /// 注意：所有符合给定条件的实体都将被检索和删除
        /// 如果条件比较多，则待删除的实体也比较多，这可能会导致主要的性能问题
        /// </summary>
        /// <param name="predicate">Entity</param>
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion

        #region 总和计算

        int Count();
        Task<int> CountAsync();
        int Count(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        long LongCount();
        Task<long> LongCountAsync();
        long LongCount(Expression<Func<TEntity, bool>> predicate);
        Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion

        void Save();
        Task SaveAsync();
    }
}