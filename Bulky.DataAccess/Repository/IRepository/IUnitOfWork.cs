using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category {get;}
        IProductRepository Product {get;}
        ICompanyRepository Company{get;}
        IShoppingRepository ShoppingCart{get;}
        IApplicationUserRepository ApplicationUser{get;}
        IOrderDetailRepository OrderDetail{get;}
        IOrderHeaderRepository OrderHeader{get;}
        void Save();
    }
}