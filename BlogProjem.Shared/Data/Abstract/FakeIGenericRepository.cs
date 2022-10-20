using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogProjem.Shared.Data.Abstract
{
    public interface FakeIGenericRepository<TEntity> where TEntity : class , new()
    {
        //GetAsync,GetAllAsync,UpdateAsync,DeleteAsync,AnyAsync,CountAsync
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity,bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
    }
}

