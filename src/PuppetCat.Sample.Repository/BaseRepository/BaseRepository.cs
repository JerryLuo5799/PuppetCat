using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;
using System.Data.Common;

namespace PuppetCat.Sample.Repository
{
    /// <summary>
    /// General operation class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseRepository<T,TContext> 
        where T : class, new()
        where TContext: DbContext,new()
    {
        #region Database Context

        /// <summary>
        /// Database Context
        /// </summary>
        //protected OneCallDbContext _dbContext;

        protected TContext _dbContext;


        public BaseRepository()
        {
            //_dbContext = new OneCallDbContext();
            _dbContext = new TContext();
        }

        public BaseRepository(TContext context)
        {
            _dbContext = context;
        }

        #endregion

        #region Single CRUD operation


        /// <summary>
        /// Add record
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="IsCommit"></param>
        /// <returns></returns>
        public virtual bool Save(T entity, bool IsCommit = true)
        {
            _dbContext.Set<T>().Add(entity);
            if (IsCommit)
                return _dbContext.SaveChanges() > 0;
            else
                return false;
        }
        /// <summary>
        /// Add record(async)
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="IsCommit"></param>
        /// <returns></returns>
        public virtual async Task<bool> SaveAsync(T entity, bool IsCommit = true)
        {
            _dbContext.Set<T>().Add(entity);
            if (IsCommit)
                return await _dbContext.SaveChangesAsync() > 0;
            else
                return false;
        }

        /// <summary>
        /// update record
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="IsCommit"></param>
        /// <returns></returns>
        public virtual bool Update(T entity, bool IsCommit = true)
        {
            _dbContext.Set<T>().Attach(entity);
            _dbContext.Entry<T>(entity).State = EntityState.Modified;
            if (IsCommit)
                return _dbContext.SaveChanges() > 0;
            else
                return false;
        }
        /// <summary>
        /// update record（async）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="IsCommit"></param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(T entity, bool IsCommit = true)
        {
            _dbContext.Set<T>().Attach(entity);
            _dbContext.Entry<T>(entity).State = EntityState.Modified;
            if (IsCommit)
                return await _dbContext.SaveChangesAsync() > 0;
            else
                return false;
        }

        /// <summary>
        /// add or update record（async）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="IsSave"></param>
        /// <param name="IsCommit"></param>
        /// <returns></returns>
        public virtual bool SaveOrUpdate(T entity, bool IsSave, bool IsCommit = true)
        {
            return IsSave ? Save(entity, IsCommit) : Update(entity, IsCommit);
        }
        /// <summary>
        /// add or update record（async）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="IsSave"></param>
        /// <param name="IsCommit"></param>
        /// <returns></returns>
        public virtual async Task<bool> SaveOrUpdateAsync(T entity, bool IsSave, bool IsCommit = true)
        {
            return IsSave ? await SaveAsync(entity, IsCommit) : await UpdateAsync(entity, IsCommit);
        }

        /// <summary>
        /// Get entity by Lamda
        /// </summary>
        /// <param name="predicate">Lamda（p=>p.Id==Id）</param>
        /// <returns></returns>
        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().AsNoTracking().SingleOrDefault(predicate);
        }

        /// <summary>
        /// Get entity by Lamda
        /// </summary>
        /// <param name="predicate">Lamda（p=>p.Id==Id）</param>
        /// <returns></returns>
        public virtual T Get<TKey>(Expression<Func<T, bool>> predicate, QueryableOrderEntry<T, TKey> orderQuery)
        {
            if (orderQuery.OrderDirection == OrderDirection.DESC)
            {
               
                return _dbContext.Set<T>().AsNoTracking().OrderByDescending(orderQuery.Expression).FirstOrDefault(predicate);
            }
            else
            {
                return _dbContext.Set<T>().AsNoTracking().OrderBy(orderQuery.Expression).FirstOrDefault(predicate);
            }
           
        }

        /// <summary>
        /// Get entity by Lamda（async）
        /// </summary>
        /// <param name="predicate">Lamda（p=>p.Id==Id）</param>
        /// <returns></returns>
        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().AsNoTracking().SingleOrDefaultAsync(predicate);
        }

        /// <summary>
        /// delete record
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="IsCommit"></param>
        /// <returns></returns>
        public virtual bool Delete(T entity, bool IsCommit = true)
        {
            if (entity == null) return false;
            _dbContext.Set<T>().Attach(entity);
            _dbContext.Set<T>().Remove(entity);

            if (IsCommit)
                return _dbContext.SaveChanges() > 0;
            else
                return false;
        }
        /// <summary>
        /// delete record（async）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="IsCommit"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(T entity, bool IsCommit = true)
        {
            if (entity == null) return false;
            _dbContext.Set<T>().Attach(entity);
            _dbContext.Set<T>().Remove(entity);
            if (IsCommit)
                return await _dbContext.SaveChangesAsync() > 0;
            else
                return false;
        }

        #endregion

        #region Multiple operation

        /// <summary>
        /// Add multiple record ，same entity
        /// </summary>
        /// <param name="list"></param>
        /// <param name="IsCommit"></param>
        /// <returns></returns>
        public virtual bool SaveList(List<T> list, bool IsCommit = true)
        {
            if (list == null || list.Count == 0) return false;

            list.ToList().ForEach(item =>
            {
                _dbContext.Set<T>().Add(item);
            });

            if (IsCommit)
                return _dbContext.SaveChanges() > 0;
            else
                return false;
        }
        /// <summary>
        /// Add multiple record ，same entity（async）
        /// </summary>
        /// <param name="list"></param>
        /// <param name="IsCommit"></param>
        /// <returns></returns>
        public virtual async Task<bool> SaveListAsync(List<T> list, bool IsCommit = true)
        {
            if (list == null || list.Count == 0) return false;

            list.ToList().ForEach(item =>
            {
                _dbContext.Set<T>().Add(item);
            });

            if (IsCommit)
                return await _dbContext.SaveChangesAsync() > 0;
            else
                return false;
        }

        /// <summary>
        /// Add multiple record ，diff entity
        /// </summary>
        /// <param name="T1"></param>
        /// <param name="IsCommit"></param>
        /// <returns></returns>
        public virtual bool SaveList<T1>(List<T1> list, bool IsCommit = true) where T1 : class
        {
            if (list == null || list.Count == 0) return false;
            var tmp = _dbContext.ChangeTracker.Entries<T>().ToList();
            foreach (var x in tmp)
            {
                var properties = typeof(T).GetTypeInfo().GetProperties();
                foreach (var y in properties)
                {
                    var entry = x.Property(y.Name);
                    entry.CurrentValue = entry.OriginalValue;
                    entry.IsModified = false;
                    y.SetValue(x.Entity, entry.OriginalValue);
                }
                x.State = EntityState.Unchanged;
            }
            list.ToList().ForEach(item =>
            {
                _dbContext.Set<T1>().Add(item);
            });
            if (IsCommit)
                return _dbContext.SaveChanges() > 0;
            else
                return false;
        }
        /// <summary>
        /// Add multiple record ，diff entity（async）
        /// </summary>
        /// <param name="T1"></param>
        /// <param name="IsCommit"></param>
        /// <returns></returns>
        public virtual async Task<bool> SaveListAsync<T1>(List<T1> list, bool IsCommit = true) where T1 : class
        {
            if (list == null || list.Count == 0) return false;
            var tmp = _dbContext.ChangeTracker.Entries<T>().ToList();
            foreach (var x in tmp)
            {
                var properties = typeof(T).GetTypeInfo().GetProperties();
                foreach (var y in properties)
                {
                    var entry = x.Property(y.Name);
                    entry.CurrentValue = entry.OriginalValue;
                    entry.IsModified = false;
                    y.SetValue(x.Entity, entry.OriginalValue);
                }
                x.State = EntityState.Unchanged;
            }
            list.ToList().ForEach(item =>
            {
                _dbContext.Set<T1>().Add(item);
            });
            if (IsCommit)
                return await _dbContext.SaveChangesAsync() > 0;
            else
                return false;
        }

        /// <summary>
        /// update multiple record ，same entity
        /// </summary>
        /// <param name="list"></param>
        /// <param name="IsCommit"></param>
        /// <returns></returns>
        public virtual bool UpdateList(List<T> list, bool IsCommit = true)
        {
            if (list == null || list.Count == 0) return false;

            list.ToList().ForEach(item =>
            {
                _dbContext.Set<T>().Attach(item);
                _dbContext.Entry<T>(item).State = EntityState.Modified;
            });

            if (IsCommit)
                return _dbContext.SaveChanges() > 0;
            else
                return false;
        }
        /// <summary>
        /// update multiple record ，same entity(async)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="IsCommit"></param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateListAsync(List<T> list, bool IsCommit = true)
        {
            if (list == null || list.Count == 0) return false;

            list.ToList().ForEach(item =>
            {
                _dbContext.Set<T>().Attach(item);
                _dbContext.Entry<T>(item).State = EntityState.Modified;
            });

            if (IsCommit)
                return await _dbContext.SaveChangesAsync() > 0;
            else
                return false;
        }

        /// <summary>
        /// update multiple record ，diff entity
        /// </summary>
        /// <param name="T1"></param>
        /// <param name="IsCommit"></param>
        /// <returns></returns>
        public virtual bool UpdateList<T1>(List<T1> list, bool IsCommit = true) where T1 : class
        {
            if (list == null || list.Count == 0) return false;

            list.ToList().ForEach(item =>
            {
                _dbContext.Set<T1>().Attach(item);
                _dbContext.Entry<T1>(item).State = EntityState.Modified;
            });

            if (IsCommit)
                return _dbContext.SaveChanges() > 0;
            else
                return false;
        }
        /// <summary>
        /// update multiple record ，diff entity(async)
        /// </summary>
        /// <param name="T1">实体模型集合</param>
        /// <param name="IsCommit">是否提交（默认提交）</param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateListAsync<T1>(List<T1> list, bool IsCommit = true) where T1 : class
        {
            if (list == null || list.Count == 0) return false;

            list.ToList().ForEach(item =>
            {
                _dbContext.Set<T1>().Attach(item);
                _dbContext.Entry<T1>(item).State = EntityState.Modified;
            });

            if (IsCommit)
                return await _dbContext.SaveChangesAsync() > 0;
            else
                return false;
        }

        /// <summary>
        /// delete multiple record ，same entity
        /// </summary>
        /// <param name="list"></param>
        /// <param name="IsCommit"></param>
        /// <returns></returns>
        public virtual bool DeleteList(List<T> list, bool IsCommit = true)
        {
            if (list == null || list.Count == 0) return false;

            list.ToList().ForEach(item =>
            {
                _dbContext.Set<T>().Attach(item);
                _dbContext.Set<T>().Remove(item);
            });

            if (IsCommit)
                return _dbContext.SaveChanges() > 0;
            else
                return false;
        }
        /// <summary>
        /// delete multiple record ，same entity（async）
        /// </summary>
        /// <param name="list">实体模型集合</param>
        /// <param name="IsCommit">是否提交（默认提交）</param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteListAsync(List<T> list, bool IsCommit = true)
        {
            if (list == null || list.Count == 0) return false;

            list.ToList().ForEach(item =>
            {
                _dbContext.Set<T>().Attach(item);
                _dbContext.Set<T>().Remove(item);
            });

            if (IsCommit)
                return await _dbContext.SaveChangesAsync() > 0;
            else
                return false;
        }

        /// <summary>
        /// delete multiple record ，diff entity
        /// </summary>
        /// <param name="T1"></param>
        /// <param name="IsCommit"></param>
        /// <returns></returns>
        public virtual bool DeleteList<T1>(List<T1> list, bool IsCommit = true) where T1 : class
        {
            if (list == null || list.Count == 0) return false;

            list.ToList().ForEach(item =>
            {
                _dbContext.Set<T1>().Attach(item);
                _dbContext.Set<T1>().Remove(item);
            });

            if (IsCommit)
                return _dbContext.SaveChanges() > 0;
            else
                return false;
        }
        /// <summary>
        /// delete multiple record ，diff entity（async）
        /// </summary>
        /// <param name="T1">实体模型集合</param>
        /// <param name="IsCommit">是否提交（默认提交）</param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteListAsync<T1>(List<T1> list, bool IsCommit = true) where T1 : class
        {
            if (list == null || list.Count == 0) return false;

            list.ToList().ForEach(item =>
            {
                _dbContext.Set<T1>().Attach(item);
                _dbContext.Set<T1>().Remove(item);
            });

            if (IsCommit)
                return await _dbContext.SaveChangesAsync() > 0;
            else
                return false;
        }

        /// <summary>
        /// delete multiple record by Lamda
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="IsCommit"></param>
        /// <returns></returns>
        public virtual bool Delete(Expression<Func<T, bool>> predicate, bool IsCommit = true)
        {
            IQueryable<T> entry = (predicate == null) ? _dbContext.Set<T>().AsQueryable() : _dbContext.Set<T>().Where(predicate);
            List<T> list = entry.ToList();

            if (list != null && list.Count == 0) return false;
            list.ForEach(item => {
                _dbContext.Set<T>().Attach(item);
                _dbContext.Set<T>().Remove(item);
            });

            if (IsCommit)
                return _dbContext.SaveChanges() > 0;
            else
                return false;
        }
        /// <summary>
        /// delete multiple record by Lamda（async）
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="IsCommit"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate, bool IsCommit = true)
        {
            IQueryable<T> entry = (predicate == null) ? _dbContext.Set<T>().AsQueryable() : _dbContext.Set<T>().Where(predicate);
            List<T> list = await entry.ToListAsync();

            if (list != null && list.Count == 0) return false;
            list.ForEach(item => {
                _dbContext.Set<T>().Attach(item);
                _dbContext.Set<T>().Remove(item);
            });

            if (IsCommit)
                return await _dbContext.SaveChangesAsync() > 0;
            else
                return false;
        }

        #endregion

        #region Get multiple record


        /// <summary>
        /// paging by linq
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="whereQuery">condition</param>
        /// <param name="orderQuery">sorting</param>
        /// <param name="pageId">pageId,form 0</param>
        /// <param name="pageSizes">pageSize</param>
        ///   /// <param name="selector">select</param>
        /// <returns></returns>
        public virtual PagedynamicResult<T> GetLinqPage<TKey>(Expression<Func<T, bool>> whereQuery, QueryableOrderEntry<T, TKey> orderQuery, int pageId, int pageSizes)
        {
            pageId = pageId < 1 ? 1 : pageId - 1;
            List<T> data = new List<T>();
            _dbContext.Database.SetCommandTimeout(300);
            var query = _dbContext.Set<T>().Where(whereQuery);// repository.TableNoTracking.Where(whereQuery);
            int count = query.Count();
            if (orderQuery.OrderDirection == OrderDirection.DESC)
            {
                data = query.OrderByDescending(orderQuery.Expression)
                    .Skip(pageSizes * pageId)
                    .Take(pageSizes).ToList<T>();
            }
            else
            {
                data = query
                    .OrderBy(orderQuery.Expression)
                    .Skip(pageSizes * pageId)
                    .Take(pageSizes).ToList<T>();
            }

            return new PagedynamicResult<T> { Data = data, ItemCount = count, PageSize = pageSizes, PageIndex = pageId + 1 };
        }



        /// <summary>
        /// IQueryable by Lamda，lazy load
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IQueryable<T> LoadAll(Expression<Func<T, bool>> predicate)
        {
            _dbContext.Database.SetCommandTimeout(300);
            return predicate != null ? _dbContext.Set<T>().Where(predicate).AsNoTracking<T>() : _dbContext.Set<T>().AsQueryable<T>().AsNoTracking<T>();
        }
        /// <summary>
        /// IQueryable by Lamda，lazy load（async）
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual Task<IQueryable<T>> LoadAllAsync(Expression<Func<T, bool>> predicate)
        {
            return Task.FromResult(predicate != null ? _dbContext.Set<T>().Where(predicate).AsNoTracking<T>() : _dbContext.Set<T>().AsQueryable<T>().AsNoTracking<T>());
        }

        /// <summary>
        /// List<T> by Lamda
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual List<T> LoadListAll(Expression<Func<T, bool>> predicate = null)
        {
            return predicate != null ? _dbContext.Set<T>().Where(predicate).AsNoTracking().ToList() : _dbContext.Set<T>().AsQueryable<T>().AsNoTracking().ToList();
        }

        /// <summary>
        /// Get entity by Lamda
        /// </summary>
        /// <param name="predicate">Lamda（p=>p.Id==Id）</param>
        /// <returns></returns>
        public virtual List<T> LoadListAll<TKey>(QueryableOrderEntry<T, TKey> orderQuery,Expression<Func<T, bool>> predicate = null)
        {
            if (orderQuery.OrderDirection == OrderDirection.DESC)
            {
                return predicate != null ? _dbContext.Set<T>().Where(predicate).OrderByDescending(orderQuery.Expression).AsNoTracking().ToList() : _dbContext.Set<T>().AsQueryable<T>().OrderByDescending(orderQuery.Expression).AsNoTracking().ToList();
            }
            else
            {
                return predicate != null ? _dbContext.Set<T>().Where(predicate).OrderBy(orderQuery.Expression).AsNoTracking().ToList() : _dbContext.Set<T>().AsQueryable<T>().OrderBy(orderQuery.Expression).AsNoTracking().ToList();
            }

        }

        // <summary>
        /// List<T> by Lamda（async）
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<List<T>> LoadListAllAsync(Expression<Func<T, bool>> predicate = null)
        {
            return predicate != null ? await _dbContext.Set<T>().Where(predicate).AsNoTracking().ToListAsync() : await _dbContext.Set<T>().AsQueryable<T>().AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// IQueryable<T> by T-Sql
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="para">Parameters参数</param>
        /// <returns></returns>
        public virtual IQueryable<T> LoadAllBySql(string sql, params DbParameter[] para)
        {
            _dbContext.Database.SetCommandTimeout(300);
            return _dbContext.Set<T>().FromSqlRaw(sql, para);
        }
        /// <summary>
        /// IQueryable<T> by T-Sql（async）
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="para">Parameters参数</param>
        /// <returns></returns>
        public virtual Task<IQueryable<T>> LoadAllBySqlAsync(string sql, params DbParameter[] para)
        {
            return Task.FromResult(_dbContext.Set<T>().FromSqlRaw(sql, para));
        }


        /// <summary>
        ///  List<T> by T-Sql
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="para">Parameters参数</param>
        /// <returns></returns>
        public virtual List<T> LoadListAllBySql(string sql, params DbParameter[] para)
        {
            return _dbContext.Set<T>().FromSqlRaw(sql, para).Cast<T>().ToList();
        }
        /// <summary>
        /// List<T> by T-Sql（async）
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="para">Parameters参数</param>
        /// <returns></returns>
        public virtual async Task<List<T>> LoadListAllBySqlAsync(string sql, params DbParameter[] para)
        {
            return await _dbContext.Set<T>().FromSqlRaw(sql, para).Cast<T>().ToListAsync();
        }

        #endregion

        #region Exist

        /// <summary>
        /// IsExist by Lamda
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual bool IsExist(Expression<Func<T, bool>> predicate)
        {
            var entry = _dbContext.Set<T>().Where(predicate);
            return (entry.Any());
        }
        /// <summary>
        /// IsExist by Lamda（async）
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<bool> IsExistAsync(Expression<Func<T, bool>> predicate)
        {
            var entry = _dbContext.Set<T>().Where(predicate);
            return await entry.AnyAsync();
        }

        /// <summary>
        ///  IsExist by T-SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public virtual bool IsExist(string sql, params DbParameter[] para)
        {
            return _dbContext.Database.ExecuteSqlRaw(sql, para) > 0;
        }
        /// <summary>
        /// IsExist by T-SQL (async)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        public virtual async Task<bool> IsExistAsync(string sql, params DbParameter[] para)
        {
            return await _dbContext.Database.ExecuteSqlRawAsync(sql, para) > 0;
        }

        #endregion
    }
}
