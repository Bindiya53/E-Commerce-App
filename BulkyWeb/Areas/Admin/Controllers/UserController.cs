using Bulky.DataAccess.Repository.IRepository;
using Bulky.Model.Models;
using Bulky.Model.ViewModels;
using Bulky.Utility;
using E_Commerce_App.Bulky.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(ApplicationDbContext dbContext,
                             IUnitOfWork unitOfWork,
                             UserManager<IdentityUser> userManager,
                             RoleManager<IdentityRole> roleManager)
        {
            _dbContext =  dbContext;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RoleManagement(string userId)
        {
            string RoleID = _dbContext.UserRoles.FirstOrDefault(x => x.UserId == userId).RoleId;
            RoleManagementVM RoleVM = new RoleManagementVM() {
                ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId, includeProperties: "Company"),
                RoleList = _roleManager.Roles.Select(i => new SelectListItem {
                    Text = i.Name,
                    Value = i.Name
                }),
                CompanyList = _unitOfWork.Company.GetAll().Select(i => new SelectListItem {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };

            RoleVM.ApplicationUser.Role = _userManager.GetRolesAsync(_unitOfWork.ApplicationUser.Get(x => x.Id == userId))
            .GetAwaiter().GetResult().FirstOrDefault();
            return View(RoleVM);
        }

        [HttpPost]
        public IActionResult RoleManagment(RoleManagementVM roleManagmentVM)
        {
            string oldRole  = _userManager.GetRolesAsync(_unitOfWork.ApplicationUser.Get(u => u.Id == roleManagmentVM.ApplicationUser.Id))
                    .GetAwaiter().GetResult().FirstOrDefault();
        

            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == roleManagmentVM.ApplicationUser.Id);


            if (!(roleManagmentVM.ApplicationUser.Role == oldRole)) {
                //a role was updated
                if (roleManagmentVM.ApplicationUser.Role == SD.Role_Company) {
                    applicationUser.CompanyId = roleManagmentVM.ApplicationUser.CompanyId;
                }
                if (oldRole == SD.Role_Company) {
                    applicationUser.CompanyId = null;
                }
                _unitOfWork.ApplicationUser.Update(applicationUser);
                _unitOfWork.Save();

                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, roleManagmentVM.ApplicationUser.Role).GetAwaiter().GetResult();

            }
            else {
                if(oldRole==SD.Role_Company && applicationUser.CompanyId != roleManagmentVM.ApplicationUser.CompanyId) {
                    applicationUser.CompanyId = roleManagmentVM.ApplicationUser.CompanyId;
                    _unitOfWork.ApplicationUser.Update(applicationUser);
                    _unitOfWork.Save();
                }
            }
            return RedirectToAction("Index");
        }


        # region API CALL

            [HttpGet]
            public IActionResult GetAll()
            {
                List<ApplicationUser> userList = _unitOfWork.ApplicationUser.GetAll(includeProperties: "Company").ToList();

                foreach(var user in userList)
                {
                    user.Role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();
                    if(user.Company == null)
                    {
                        user.Company =  new Company()
                        {
                            Name = ""
                        };
                    }
                }
                return Json(new {data = userList});
            }

            [HttpDelete]
            public IActionResult Delete(int? id)
            {
                return Json(new { success = "true", message = "Deleted Successfully"});
            }

            [HttpPost]
            public IActionResult LockUnlock([FromBody]string id)
            {

                var objFromDb = _unitOfWork.ApplicationUser.Get(u => u.Id == id);
                if (objFromDb == null) 
                {
                    return Json(new { success = false, message = "Error while Locking/Unlocking" });
                }

                if(objFromDb.LockoutEnd!=null && objFromDb.LockoutEnd > DateTime.Now) {
                    //user is currently locked and we need to unlock them
                    objFromDb.LockoutEnd = DateTime.Now;
                }
                else {
                    objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
                }
                _unitOfWork.ApplicationUser.Update(objFromDb);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Operation Successful" });
            }

            
        #endregion


        
    }
}