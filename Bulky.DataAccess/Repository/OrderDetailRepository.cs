using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Model.Models;
using E_Commerce_App.Bulky.DataAccess.Data;

namespace Bulky.DataAccess.Repository
{
    public class OrderDetailRepository : Repository<OrderDetails>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public OrderDetailRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public void Update(OrderDetails orderDetails)
        {
            _dbContext.orderDetails.Update(orderDetails);
        }
    }
    
}