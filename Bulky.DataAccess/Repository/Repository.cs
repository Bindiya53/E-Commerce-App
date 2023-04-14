using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        internal DbSet<T> _dbSet;
        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            this._dbSet = _dbContext.Set<T>();
            _dbContext.Products.Include(x => x.Category).Include(u => u.CategoryId);
            
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
           IQueryable<T> query = _dbSet;
           if(!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var Include in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(Include);
                }
            }
           return query.FirstOrDefault();
        }


        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            if(!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var Include in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(Include);
                }
            }
            return query.ToList();
        }

        //  public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string? includeProperties = null)
        // {
        //     IQueryable<T> query = dbSet;
        //     if (filter != null) {
        //         query = query.Where(filter);
        //     }
		// 	if (!string.IsNullOrEmpty(includeProperties))
        //     {
        //         foreach(var includeProp in includeProperties
        //             .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        //         {
        //             query = query.Include(includeProp);
        //         }
        //     }
        //     return query.ToList();
        // }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            _dbSet.RemoveRange(entity);
        }
    }
}