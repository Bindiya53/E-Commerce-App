using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Model.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BulkyWeb.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region API CALLS

        [HttpGet]
		public IActionResult GetAll(string status)
        {

            IEnumerable<OrderHeader> objOrderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();
            //IEnumerable<OrderHeader> objOrderHeaders;


            // if(User.IsInRole(SD.Role_Admin)|| User.IsInRole(SD.Role_Employee)) {
            //     objOrderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();
            // }
            // else {

            //     var claimsIdentity = (ClaimsIdentity)User.Identity;
            //     var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            //     objOrderHeaders = _unitOfWork.OrderHeader
            //         .GetAll(u => u.ApplicationUserId == userId, includeProperties: "ApplicationUser");
            // }


            switch (status) {
                case "pending":
                    objOrderHeaders = objOrderHeaders.Where(u => u.PaymentStatus == SD.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;
                default:
                    break;

            }


            return Json(new { data = objOrderHeaders });
		}


		#endregion

        
    }
}