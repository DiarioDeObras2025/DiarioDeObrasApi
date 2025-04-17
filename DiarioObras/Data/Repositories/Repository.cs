using DiarioObras.Data.Context;
using DiarioObras.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace DiarioObras.Data.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>()
                             .Where(predicate)
                             .AsNoTracking()
                             .ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(
       Expression<Func<T, bool>> predicate,
       params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _context.Set<T>();

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return await query
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync();
    }
    public virtual async Task<T?> GetByIdAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>()
                             .FirstOrDefaultAsync(predicate);
    }

    public async Task<T> CreateAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }

    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);
        return entity;
    }

    public T Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        return entity;
    }
}
