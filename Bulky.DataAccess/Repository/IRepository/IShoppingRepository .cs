using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulky.Model.Models;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IShoppingRepository : IRepository<ShoppingCart> 
    {
        void Update(ShoppingCart shoppingCart);
    }
}