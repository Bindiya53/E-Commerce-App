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
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork =  unitOfWork;
        }

        public IActionResult Index()
        {
            List<Company> companyList = _unitOfWork.Company.GetAll().ToList();
            return View(companyList);
        }
        
        public IActionResult Upsert(int? id)
        {
           
            if( id == null || id == 0)
            {
                return View(new Company());
            }
            else
            {
                Company company = _unitOfWork.Company.Get(x => x.Id == id);
                return View(company);
            }
        }


        [HttpPost]
        public IActionResult Upsert(Company company)
        {
            if(ModelState.IsValid)
            {
                if(company.Id == 0)
                {
                    _unitOfWork.Company.Add(company);
                }
                else
                {
                    _unitOfWork.Company.Update(company);
                }
                _unitOfWork.Save();
                TempData["Success"] = "Company created successfully";
                return RedirectToAction("Index");
            }
            else{
                return View(company);
            };
           
        }

        # region API CALL

            [HttpGet]
            public IActionResult GetAll()
            {
                List<Company> companyList = _unitOfWork.Company.GetAll().ToList();
                return Json(new {data = companyList});
            }

            [HttpDelete]
            public IActionResult Delete(int? id)
            {
                var company = _unitOfWork.Company.Get(x => x.Id == id);
                if( company == null)
                {
                    return Json( new { success = "false", message = "Error while deleting"});
                }
                
                _unitOfWork.Company.Remove(company);
                _unitOfWork.Save();
                return Json(new { success = "true", message = "Deleted Successfully"});
            }
        #endregion


        
    }
}