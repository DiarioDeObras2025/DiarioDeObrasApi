using System.Linq.Expressions;

namespace DiarioObras.Data.Interfaces;

public interface IRepository<T>
{
    T? GetById(Expression<Func<T, bool>> predicate);
    IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate);
    IEnumerable<T> GetAll(
    Expression<Func<T, bool>> predicate,
    params Expression<Func<T, object>>[] includeProperties);

    T Create(T entity);
    T Update(T entity);
    T Delete(T entity);
}
