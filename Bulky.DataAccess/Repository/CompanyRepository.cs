using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Bulky.DataAccess.Repository.IRepository;
using Bulky.Model.Models;
using E_Commerce_App.Bulky.DataAccess.Data;

namespace Bulky.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CompanyRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public void Update(Company company)
        {
            _dbContext.Update(company);
        }
    }
}