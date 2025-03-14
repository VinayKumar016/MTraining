using Microsoft.AspNetCore.Mvc;
using FooddeliveryApp;
using FoodDeliveryMVC.Infra;
namespace FoodDeliveryMVC.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Login(string uemail,string upwd)
        {
            BusinessLayerfd bl = new BusinessLayerfd();
            usersDTO Status = bl.AuthenticateUser(uemail, upwd);
            if (Status != null)
            {
                string strloggedinUser = ObjectJsonHelper.ToJson<usersDTO>(Status);
                HttpContext.Session.SetString("LoggedinUser", strloggedinUser);
                if (Status.roledal == usersDTO.role.admin) 
                { 
                    return RedirectToAction("Index", "AdminDashboard");
                }
                else if (Status.roledal == usersDTO.role.owner)
                {
                    return RedirectToAction("Index", "OwnerDashboard");
                }
                else if (Status.roledal == usersDTO.role.user)
                {
                    return RedirectToAction("Index", "UserDashboard");
                }
         
            }
            return View();
        }
    }
}

