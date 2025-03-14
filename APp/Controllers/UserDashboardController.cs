using FooddeliveryApp;
using FoodDeliveryMVC.Infra;
using FoodDeliveryMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryMVC.Controllers
{
    public class UserDashboardController : Controller
    {
        BusinessLayerfd bl;
        public UserDashboardController(BusinessLayerfd businessLayerfd)
        {
            bl = businessLayerfd;
        }
        public IActionResult Index()
        {

            string strUser = HttpContext.Session.GetString("LoggedinUser");
            if (strUser == null)
            {
                return RedirectToAction("Login", "Account");
            }
            usersDTO luser = ObjectJsonHelper.GetFromJson<usersDTO>(strUser);
            if (luser.roledal != usersDTO.role.user)
            {
                return RedirectToAction("Login", "Account");
            }
            List<restaurantDTO> rest = bl.SearchByloc(luser.ulocation);
            UserDashboardViewModel model = new UserDashboardViewModel();
            List<ordersDTO> orders = bl.ListMyOrders(luser.userid);
            model.rest = rest;
            model.orders = orders;

            return View(model);
        }

    }
}
