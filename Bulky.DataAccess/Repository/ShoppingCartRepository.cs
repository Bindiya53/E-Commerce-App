using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Model.Models;
using E_Commerce_App.Bulky.DataAccess.Data;

namespace Bulky.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ShoppingCartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public void Update(ShoppingCart company)
        {
            _dbContext.ShoppingCarts.Update(company);
        }
    }
}