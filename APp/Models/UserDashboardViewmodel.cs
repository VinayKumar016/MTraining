using FooddeliveryApp;

namespace FoodDeliveryMVC.Models
{
    public class UserDashboardViewModel
    {
        public List<usersDTO> users { get; set; }
        public List<restaurantDTO> rest { get; set; }
        public List<menusDTO> menu { get; set; }

        public List<menuDTO> newmenu { get; set; }
        public List<menuDTO2> newmenu2 { get; set; }

        public List<orderitemDTO> orderitem { get; set; }

        public List<ordersDTO> orders { get; set; }


    }
    public class menuDTO
    {
        public List<long> menuid { get; set; }
        public long rid { get; set; }
        public List<string> itemname { get; set; }
        public List<long> price { get; set; }

        
        public List<long> quantity { get; set; }
    }
    public class menuDTO2
    {
        public long menuid { get; set; }
        public long rid { get; set; }
        public string itemname { get; set; }
        public long price { get; set; }
        public long quantity { get; set; }
    }
}
