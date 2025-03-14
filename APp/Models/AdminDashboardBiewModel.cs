using FooddeliveryApp;

namespace FoodDeliveryMVC.Models
{
    public class AdminDashboardBiewModel
    {
        public List<usersDTO> users { get; set; }
        public List<restaurantDTO> rest { get; set; }

        public List<menusDTO> menu { get; set; }

        

    }
    
}
