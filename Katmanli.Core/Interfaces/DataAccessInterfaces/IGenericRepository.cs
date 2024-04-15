using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.Core.Interfaces.DataAccessInterfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        // public-private vb. olarak belirtmedim default olarak interface özellikleri publictir.
        Task<TEntity> GetByIdAsync(int id);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        IQueryable<TEntity> Queryable();

    }
}
