using System.Linq.Expressions;

namespace DiarioObras.Data.Interfaces;

public interface IRepository<T>
{
    Task<T?> GetByIdAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includeProperties);

    Task<T> CreateAsync(T entity);

    T Update(T entity);
    T Delete(T entity);
}
