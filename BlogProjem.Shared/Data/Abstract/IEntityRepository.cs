using BlogProjem.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogProjem.Shared.Data.Abstract
{
    public interface IEntityRepository<T> where T:class ,IEntity, new()
    {
        Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> AddAsync(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate=null); //Mesela admin panelinde elimizdeki verileri listelemek isteyebilirz kaç tane makale var vs


    }
}
