using FooddeliveryApp;
using FoodDeliveryMVC.Infra;
using FoodDeliveryMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryMVC.Controllers
{
    public class OrderCollectionController : Controller
    {
        BusinessLayerfd bl;
        public OrderCollectionController(BusinessLayerfd businessLayerfd)
        {
            bl = businessLayerfd;
        }

        public IActionResult Index()
        {
            string strOrder = HttpContext.Session.GetString("OrderItems");
            menuDTO Order = ObjectJsonHelper.GetFromJson<menuDTO>(strOrder);
            UserDashboardViewModel model = new UserDashboardViewModel();
            model.menu = new List<menusDTO>();
            model.newmenu2 = new List<menuDTO2>(); // Initialize the newmenu2 property

            foreach (var item in Order.itemname)
            {
                if(Order.quantity[Order.itemname.IndexOf(item)] == 0)
                {
                    continue;
                }
                model.newmenu2.Add(new menuDTO2
                {
                    menuid = Order.menuid[Order.itemname.IndexOf(item)],
                    rid = Order.rid,
                    itemname = item,
                    price = Order.price[Order.itemname.IndexOf(item)],
                    quantity = Order.quantity[Order.itemname.IndexOf(item)]
                });
            }

            return View(model);
        }

        public IActionResult PlaceOrder()
        {
            string strOrder = HttpContext.Session.GetString("OrderItems");
            menuDTO Order = ObjectJsonHelper.GetFromJson<menuDTO>(strOrder);
            string strUser = HttpContext.Session.GetString("LoggedinUser");
            usersDTO luser = ObjectJsonHelper.GetFromJson<usersDTO>(strUser);

            ordersDTO or = new ordersDTO();
            or.rid = Order.rid;
            or.userid = luser.userid;
            bl.Addorders(or);
            long orid = bl.getlastordid();
            List<orderitemDTO> orderitem = new List<orderitemDTO>();
            foreach (var item in Order.itemname)
            {
                if (Order.quantity[Order.itemname.IndexOf(item)] == 0)
                {
                    continue;
                }

                

                orderitem.Add(new orderitemDTO
                {
                    orderid = orid,

                    menuid = Order.menuid[Order.itemname.IndexOf(item)],

                    quantity = Order.quantity[Order.itemname.IndexOf(item)]
                });

            }

            bl.AddMultipleOrderItems(orderitem);

            return View();
        }

    }
}
