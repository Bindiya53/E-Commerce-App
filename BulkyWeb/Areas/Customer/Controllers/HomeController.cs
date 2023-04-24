using System.Security.Claims;
using System.Collections.Generic;
using System.Diagnostics;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Model.Models;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BulkyWeb.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<Product>  productList= _unitOfWork.Product.GetAll(includeProperties:"Category");
        return View(productList);
    }

    public IActionResult Details(int productId)
    {
        ShoppingCart cart = new ShoppingCart(){
            Product = _unitOfWork.Product.Get(x => x.Id == productId, includeProperties:"Category"),
            Count = 1,
            ProductId = productId
        };
       
        return View(cart);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Details(ShoppingCart cart)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        cart.ApplicationUserId = userId;

        ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(x => x.ApplicationUserId == userId && x.ProductId == cart.ProductId);

        if(cartFromDb != null)
        {
            cartFromDb.Count += cart.Count;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
        }
        else{
            _unitOfWork.ShoppingCart.Add(cart);
        }
        TempData["success"] = "Cart Updated Successfully";
        _unitOfWork.Save();
       
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
