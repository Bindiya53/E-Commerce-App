using System.Runtime.InteropServices.ComTypes;
using System.Collections.Generic;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Model.Models;
using Bulky.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnv;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnv)
        {
            _unitOfWork =  unitOfWork;
            _webHostEnv = webHostEnv;
        }

        public IActionResult Index()
        {
            List<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return View(productList);
        }
        
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM =  new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.CategoryId.ToString()
                }),
                Product =  new Product()
            };
            if( id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.Product.Get(x => x.Id == id);
                return View(productVM);
            }
        }


        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if(ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnv.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Images\Product");

                    if(!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        };
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\Images\Product\" + fileName;
                }
                if(productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
                _unitOfWork.Save();
                TempData["Success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            else{
                
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.CategoryId.ToString()
                });
                return View(productVM);
            };
           
        }

        # region API CALL

            [HttpGet]
            public IActionResult GetAll()
            {
                List<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
                return Json(new {data = productList});
            }

            [HttpDelete]
            public IActionResult Delete(int? id)
            {
                var product = _unitOfWork.Product.Get(x => x.Id == id);
                if( product == null)
                {
                    return Json( new { success = "false", message = "Error while deleting"});
                }

                var oldImagePath = Path.Combine(_webHostEnv.WebRootPath, product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        };
                
                _unitOfWork.Product.Remove(product);
                _unitOfWork.Save();
                return Json(new { success = "true", message = "Deleted Successfully"});
            }
        #endregion


        
    }
}