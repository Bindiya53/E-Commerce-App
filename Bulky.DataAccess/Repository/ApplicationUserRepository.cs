using Bulky.DataAccess.Repository.IRepository;
using Bulky.Model.Models;
using E_Commerce_App.Bulky.DataAccess.Data;

namespace Bulky.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ApplicationUserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

    
        
    }
}