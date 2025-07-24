using System.Linq.Expressions;
using TrainingAPI.Context;
using Microsoft.EntityFrameworkCore;

namespace TrainingAPI.Repositories.Interfaces;

public class Repository<T> : IRepository<T> where T : class
{   
    protected TrainingAPIContext _context;

    public Repository(TrainingAPIContext context)
    {
        _context = context;
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().AsNoTracking().ToListAsync();
    }

    public T Create(T entity)
    {
        _context.Set<T>().Add(entity);
        return entity;
    }

    public T Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        return entity;
    }

    public T Delete(T entity)
    {
        _context.Remove(entity);
        return entity;
    }
}
