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

        public UserController(ApplicationDbContext dbContext, IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _dbContext =  dbContext;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RoleManagement(string userId)
        {
            string RoleID = _dbContext.UserRoles.FirstOrDefault(x => x.UserId == userId).RoleId;
            RoleManagementVM RoleVM = new RoleManagementVM() {
                ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId),
                RoleList = _dbContext.Roles.Select(i => new SelectListItem {
                    Text = i.Name,
                    Value = i.Name
                }),
                CompanyList = _unitOfWork.Company.GetAll().Select(i => new SelectListItem {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };

            RoleVM.ApplicationUser.Role = _dbContext.Roles.FirstOrDefault(u=>u.Id== RoleID).Name;
            return View(RoleVM);
        }

        [HttpPost]
        public IActionResult RoleManagment(RoleManagementVM roleManagmentVM)
        {
            string RoleID = _dbContext.UserRoles.FirstOrDefault(x => x.UserId == roleManagmentVM.ApplicationUser.Id).RoleId;

            string oldRole  = _dbContext.Roles.FirstOrDefault(u => u.Id == RoleID).Name;
        

            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == roleManagmentVM.ApplicationUser.Id);


            if (!(roleManagmentVM.ApplicationUser.Role == oldRole)) {
                //a role was updated
                if (roleManagmentVM.ApplicationUser.Role == SD.Role_Company) {
                    applicationUser.CompanyId = roleManagmentVM.ApplicationUser.CompanyId;
                }
                if (oldRole == SD.Role_Company) {
                    applicationUser.CompanyId = null;
                }
                //_unitOfWork.ApplicationUser.Update(applicationUser);
                _dbContext.SaveChanges();

                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, roleManagmentVM.ApplicationUser.Role).GetAwaiter().GetResult();

            }
            return RedirectToAction("Index");
        }


        # region API CALL

            [HttpGet]
            public IActionResult GetAll()
            {
                List<ApplicationUser> userList = _dbContext.ApplicationUsers.Include(x => x.Company).ToList();

                var userRoles = _dbContext.UserRoles.ToList();
                var roles = _dbContext.Roles.ToList();

                foreach(var user in userList)
                {
                    var roleId = userRoles.FirstOrDefault(X => X.UserId == user.Id).RoleId;
                    user.Role = roles.FirstOrDefault(x => x.Id == roleId).Name;

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

                var objFromDb = _dbContext.ApplicationUsers.FirstOrDefault(u => u.Id == id);
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
                //_dbContext.ApplicationUsers.Update(objFromDb);
                _dbContext.SaveChanges();
                return Json(new { success = true, message = "Operation Successful" });
            }

            
        #endregion


        
    }
}