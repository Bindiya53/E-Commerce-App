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
                    Value = x.Id.ToString()
                }),
                Product =  new Product()
            };
            if( id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.Product.Get(x => x.Id == id, includeProperties : "ProductImages");
                return View(productVM);
            }
        }


        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, List<IFormFile> files)
        {
            if(ModelState.IsValid)
            {

                if(productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                } 

                _unitOfWork.Save();

                string wwwRootPath = _webHostEnv.WebRootPath;
                if(files != null)
                {
                    foreach(var file in files)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = @"Images\Product\Products-" + productVM.Product.Id;
                        string finalPath = Path.Combine(wwwRootPath, productPath);


                        if (!Directory.Exists(finalPath))
                            Directory.CreateDirectory(finalPath);

                        using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create)) {
                            file.CopyTo(fileStream);

                            ProductImage productImage = new() {
                            ImageUrl = @"\" + productPath + @"\" + fileName,
                            ProductId=productVM.Product.Id,
                        };

                        if (productVM.Product.ProductImages == null)
                            productVM.Product.ProductImages = new List<ProductImage>();

                        productVM.Product.ProductImages.Add(productImage);
                        }
                    }

                     _unitOfWork.Product.Update(productVM.Product);
                    _unitOfWork.Save();
                }
                
               
                TempData["success"] = "Product created/updated successfully";
                return RedirectToAction("Index");
            }
            else{
                
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
                return View(productVM);
            };
           
        }

        public IActionResult DeleteImage(int imageId) {
            var imageToBeDeleted = _unitOfWork.ProductImage.Get(u => u.Id == imageId);
            int productId = imageToBeDeleted.ProductId;
            if (imageToBeDeleted != null) {
                if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl)) {
                    var oldImagePath =
                                   Path.Combine(_webHostEnv.WebRootPath,
                                   imageToBeDeleted.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath)) {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unitOfWork.ProductImage.Remove(imageToBeDeleted);
                _unitOfWork.Save();

                TempData["success"] = "Deleted successfully";
            }

            return RedirectToAction(nameof(Upsert), new { id = productId });
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

                // var oldImagePath = Path.Combine(_webHostEnv.WebRootPath, product.ImageUrl.TrimStart('\\'));
                        // if(System.IO.File.Exists(oldImagePath))
                        // {
                        //     System.IO.File.Delete(oldImagePath);
                        // };

                
                string productPath = @"Images\Product\Products-" + id;
                string finalPath = Path.Combine(_webHostEnv.WebRootPath, productPath);


                if (Directory.Exists(finalPath)) {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach (string filePath in filePaths) {
                    System.IO.File.Delete(filePath);
                }

                    Directory.Delete(finalPath);
                }

                
                _unitOfWork.Product.Remove(product);
                _unitOfWork.Save();
                return Json(new { success = "true", message = "Deleted Successfully"});
            }
        #endregion


        
    }
}