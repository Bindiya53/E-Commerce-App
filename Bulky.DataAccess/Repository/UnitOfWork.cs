using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;

namespace Bulky.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public ICategoryRepository Category {get; private set;}

        public IProductRepository Product{get; private set;}

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            Category =  new CategoryRepository(_dbContext);
            Product =  new ProductRepository(_dbContext);
        }
        //public ICategoryRepository CategoryRepo => throw new NotImplementedException();

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}