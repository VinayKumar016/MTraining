using FooddeliveryApp;
using FoodDeliveryMVC.Infra;
using FoodDeliveryMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryMVC.Controllers
{
    public class OwnerDashboardController : Controller
    {
        public IActionResult Index()
        {
            string strUser=HttpContext.Session.GetString("LoggedinUser");
            if (strUser == null)
            {
                return RedirectToAction("Login", "Account");
            }
            usersDTO luser = ObjectJsonHelper.GetFromJson<usersDTO>(strUser);
            if (luser.roledal != usersDTO.role.owner)
            {
                return RedirectToAction("Login", "Account");
            }

            BusinessLayerfd bl = new BusinessLayerfd();
            List<restaurantDTO> rest = bl.ListMyRest(luser.userid);
            AdminDashboardBiewModel model = new AdminDashboardBiewModel();
            model.rest = rest;


            return View(model);
        }
    }
}
