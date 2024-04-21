using System.Linq.Expressions;
using Best.DataAccess.IRepository.IRepository;
using BestApp.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace Best.DataAccess.Repository.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _db;
    internal DbSet<T> dbSet;
    public Repository(ApplicationDbContext db){
        _db = db;
        this.dbSet = _db.Set<T>();
        _db.Products.Include(u => u.Category);
    }

    public void Add(T entity )
    {
        dbSet.Add(entity);
    }

    public T Get(Expression<Func<T, bool>> filter,string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;
        query = query.Where(filter);
        if(!string.IsNullOrEmpty(includeProperties)){
            foreach(var includeProp in includeProperties.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries)){
                query = query.Include(includeProp);
            }
        }
        return query.FirstOrDefault();

    }

    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter,string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;
        if(filter != null){
        query = query.Where(filter);
        }
        if(!string.IsNullOrEmpty(includeProperties)){
            foreach(var includeProp in includeProperties.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries)){
                query = query.Include(includeProp);
            }
        }
        return query.ToList();
    }

    public void Remove(T entity)
    {
        dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entity)
    {
        dbSet.RemoveRange(entity);
    }
}
