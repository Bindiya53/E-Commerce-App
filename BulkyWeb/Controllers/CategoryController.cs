using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BulkyWeb.Controllers
{
    //[Route("[controller]")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public CategoryController(ApplicationDbContext dbContext)
        {
            _dbContext =  dbContext;
        }

        public IActionResult Index()
        {
            List<Category> categoryList = _dbContext.Categories.ToList();
            return View(categoryList);
        }

        
    }
}