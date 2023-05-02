using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Model.Models;
using E_Commerce_App.Bulky.DataAccess.Data;

namespace Bulky.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public void Update(Product product)
        {
            var productObj = _dbContext.Products.FirstOrDefault( x=> x.Id == product.Id);
            if(productObj != null)
            {
                productObj.Title = product.Title;
                productObj.ISBN = product.ISBN;
                productObj.Price = product.Price;
                productObj.Price50 = product.Price50;
                productObj.Price100 = product.Price100;
                productObj.ListPrice = product.ListPrice;
                productObj.Description = product.Description;
                productObj.CategoryId = product.CategoryId;
                productObj.Author = product.Author;
                if(product.ImageUrl != null )
                {
                    productObj.ImageUrl = product.ImageUrl;
                }
            }
        }
    }
}