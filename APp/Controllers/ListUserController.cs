using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryMVC.Controllers
{
    public class ListUserController : Controller
    {
       

        public IActionResult ListUser()
        {
            return View();
        }
    }
}
