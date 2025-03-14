using FooddeliveryApp;
using FoodDeliveryMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryMVC.Controllers
{
    public class MenuManagerController : Controller
    {
        BusinessLayerfd bl;
        public MenuManagerController(BusinessLayerfd businessLayerfd)
        {
            bl=businessLayerfd;
        }

        public IActionResult Index(long Id)
        {
            List<menusDTO> menuItems = bl.ListMyMenu(Id);
            AdminDashboardBiewModel model = new AdminDashboardBiewModel();
            model.menu = menuItems; 

            return View(model);
        }
    }
}
