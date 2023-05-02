using Bulky.DataAccess.Repository.IRepository;
using Bulky.Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork =  unitOfWork;
        }

        public IActionResult Index()
        {
            List<Category> categoryList = _unitOfWork.Category.GetAll().ToList();
            return View(categoryList);
        }
        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if(category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name","The DisplayOrder cannot exactly match the Name");
            }
            if(ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                TempData["Success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult Edit(int? id)
        {
            if(id == 0 || id == null)
            {
                return NotFound();
            }
            Category? category = _unitOfWork.Category.Get(x => x.Id == id);
            if( category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if(ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();
                TempData["Success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if(id == 0 || id == null)
            {
                return NotFound();
            }
            Category? category =  _unitOfWork.Category.Get(x => x.Id == id);
            if( category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteCategory(int? id)
        {
            Category category = _unitOfWork.Category.Get(x => x.Id == id);
            if( category == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(category);
            _unitOfWork.Save();
            TempData["Success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }

        

        
    }
}