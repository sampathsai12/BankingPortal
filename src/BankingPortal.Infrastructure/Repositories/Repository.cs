using System.Linq.Expressions;
using BankingPortal.Application.Interfaces;
using BankingPortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BankingPortal.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly BankingPortalDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(BankingPortalDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(long id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }
}