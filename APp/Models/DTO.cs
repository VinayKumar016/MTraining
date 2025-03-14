using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FooddeliveryApp
{
    public class usersDTO
    {
        public long userid { get; set; }
        public string uname { get; set; }

        public string email { get; set; }
        public string password { get; set; }
        public role roledal{ get; set; }
        public string ulocation { get; set; }
        public enum role
        {
            admin = 1, // Assigning 1 to admin
            user = 2  , // Assigning 2 to user
            owner = 3  // Assigning 3 to owner
        }

    }
    public class restaurantDTO
    {
        public long resid { get; set; }
        public string rname { get; set; }
        public string rlocation { get; set; }
        public long ownerid { get; set; }

        public string ownername { get; set; }
    }
    public class ordersDTO
    {
        public long orderid { get; set; }
        public long userid { get; set; }
        public long rid { get; set; }
        public string orderstatus { get; set; }
        public string rname { get; set; }
    }
    public class menusDTO
    {
        public long menuid { get; set; }
        public long rid { get; set; }
        public string itemname { get; set; }
        public long price { get; set; }
    }
    public class orderitemDTO
    {
        public long orderitemid { get; set; }
        public long orderid { get; set; }
        public long menuid { get; set; }
        public long quantity { get; set; }
    }
}
