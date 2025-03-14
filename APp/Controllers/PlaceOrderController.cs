using FooddeliveryApp;
using FoodDeliveryMVC.Infra;
using FoodDeliveryMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryMVC.Controllers
{
    public class PlaceOrderController : Controller
    {
        BusinessLayerfd bl;
        public PlaceOrderController(BusinessLayerfd businessLayerfd)
        {
            bl = businessLayerfd;
        }

        public IActionResult Index(long Id)
        {
            List<menusDTO> menuItems = bl.ListMyMenu(Id);
            UserDashboardViewModel model = new UserDashboardViewModel();
            model.menu = menuItems;
            
            
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(menuDTO newmenu)
        {
            if (newmenu != null)
            {
                string strOrderItems = ObjectJsonHelper.ToJson<menuDTO>(newmenu);
                HttpContext.Session.SetString("OrderItems", strOrderItems);
                return RedirectToAction("Index", "OrderCollection");

            }

            return View();
        }



    }
}
