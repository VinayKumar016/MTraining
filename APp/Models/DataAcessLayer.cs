using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FooddeliveryApp
{
    public class DataAcessLayer
    {
        string constring = "Data Source=.;Initial Catalog=FOODDEL;Integrated Security=SSPI";

        SqlConnection con;
        public DataAcessLayer()
        {
            con = new SqlConnection(constring);

        }
        public void OpenConnection()
        {
            con.Open();
        }
        public void CloseConnection()
        {
            con.Close();
        }
        public bool Addorderitems(orderitemDTO orderitems)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = $"Insert into Orderitems(orderid,menuid,quantity) values(,{orderitems.orderid},{orderitems.menuid},{orderitems.quantity})";
            int res = cmd.ExecuteNonQuery();
            if (res > 0)
                return true;
            else
                return false;
        }
        public bool Addmenu(menusDTO menus)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = $"Insert into Menus(rid,itemname,price) values({menus.rid},'{menus.itemname}',{menus.price})";
            int res = cmd.ExecuteNonQuery();
            if (res > 0)
                return true;
            else
                return false;
        }
       
        public bool Adduser(usersDTO user)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = $"Insert into Users(uname,email,password,role,ulocation) values('{user.uname}','{user.email}','{user.password}',{(long)user.roledal},'{user.ulocation}')";
            int res = cmd.ExecuteNonQuery();
            if (res > 0)
                return true;
            else
                return false;
        }
        public bool Addorders(ordersDTO ord)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = $"Insert into Orders(userid,rid) values({ord.userid},{ord.rid})";
            int res = cmd.ExecuteNonQuery();
            if (res > 0)
                return true;
            else
                return false;
        }


        public long lastorderedorderid()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "Select top 1 orderid from Orders order by orderid desc";
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            long orderid = dr.GetInt64(0);
            dr.Close();
            return orderid;
        }
        public bool AddMultipleOrderItems(List<orderitemDTO> orderitems)
        {
            int res = 0;
            foreach (var item in orderitems)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = $"Insert into Orderitem(orderid,menuid,quantity) values({item.orderid},{item.menuid},{item.quantity})";
                res = cmd.ExecuteNonQuery();
            }
            if (res > 0)
                return true;
            else
                return false;
        }

        public List<ordersDTO> ListOrderbyId(long userid)
        {
            List<ordersDTO> orders = new List<ordersDTO>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = $"Select * from orders o left join restaurants r on o.rid=r.rid where o.userid={userid}";
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ordersDTO order = new ordersDTO();
                order.orderid = dr.GetInt64(0);
                order.userid = dr.GetInt64(1);
                order.rid = dr.GetInt64(2);
                order.orderstatus = dr.GetString(3);
                order.rname = dr.GetString(5);
                orders.Add(order);
            }
            dr.Close();
            return orders;
        }
        public bool Addrestaurant(restaurantDTO res)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = $"Insert into Restaurants(rname,rlocation,ownerid) values('{res.rname}','{res.rlocation}',{res.ownerid})";
            int ress = cmd.ExecuteNonQuery();
            if (ress > 0)
                return true;
            else
                return false;
        }

        public usersDTO LoginUser(string email, string password)
        {
            List<usersDTO> users = new List<usersDTO>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = $"select * from users where Email='{email}' AND Password='{password}'";
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                usersDTO user = new usersDTO();
                user.userid = dr.GetInt64(0);
                user.uname = dr.GetString(1);
                user.email = dr.GetString(2);
                user.password = dr.GetString(3);
                user.roledal = (usersDTO.role)dr.GetInt64(4);
                user.ulocation = dr.GetString(5);
                dr.Close();
                return user;
            }
            else
            {
                dr.Close();
                return null;

            }
        }
        public List<usersDTO> Listusers()
        {
            List<usersDTO> users = new List<usersDTO>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "Select * from Users";
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                usersDTO user = new usersDTO();
                user.userid = dr.GetInt64(0);
                user.uname = dr.GetString(1);
                user.email = dr.GetString(2);
                user.password = dr.GetString(3);
                user.roledal = (usersDTO.role)dr.GetInt64(4);
                user.ulocation = dr.GetString(5);
                users.Add(user);
            }
            dr.Close();
            return users;
        }
        public bool DeleteUser(long userid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = $"Delete from Users where userid={userid}";
            int res = cmd.ExecuteNonQuery();
            if (res > 0)
                return true;
            else
                return false;
        }
        public List<ordersDTO> Listorders()
        {
            List<ordersDTO> orders = new List<ordersDTO>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "Select * from Orders";
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ordersDTO order = new ordersDTO();
                order.orderid = dr.GetInt64(0);
                order.userid = dr.GetInt64(1);
                order.rid = dr.GetInt64(2);
                order.orderstatus = dr.GetString(3);
                orders.Add(order);
            }
            dr.Close();
            return orders;
        }
        public List<orderitemDTO> Listorderitems()
        {
            List<orderitemDTO> orderitems = new List<orderitemDTO>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "Select * from Orderitems";
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                orderitemDTO orderitem = new orderitemDTO();
                orderitem.orderitemid = dr.GetInt64(0);
                orderitem.orderid = dr.GetInt64(1);
                orderitem.menuid = dr.GetInt64(2);
                orderitem.quantity = dr.GetInt64(3);
                orderitems.Add(orderitem);
            }
            dr.Close();
            return orderitems;
        }

        public List<menusDTO> ListMenu()
        {
            List<menusDTO> menuitems = new List<menusDTO>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "Select * from Menus";
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                menusDTO menu = new menusDTO();
                menu.menuid = dr.GetInt64(0);
                menu.rid = dr.GetInt64(1);
                menu.itemname = dr.GetString(2);
                menu.price = dr.GetInt64(3);
                menuitems.Add(menu);
            }
            dr.Close();

            return menuitems;
        }

        public List<menusDTO> ListRestMenu(long rid)
        {
            List<menusDTO> menuitems = new List<menusDTO>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = $"Select * from Menus where rid={rid}";
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                menusDTO menu = new menusDTO();
                menu.menuid = dr.GetInt64(0);
                menu.rid = dr.GetInt64(1);
                menu.itemname = dr.GetString(2);
                menu.price = dr.GetInt64(3);
                menuitems.Add(menu);
            }
            dr.Close();
            return menuitems;
        }

        public List<restaurantDTO> Listrestaurant()
        {
            List<restaurantDTO> res = new List<restaurantDTO>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "Select * from Restaurants R JOIN users U on R.ownerid = U.userid";
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                restaurantDTO ress = new restaurantDTO();
                ress.resid = dr.GetInt64(0);
                ress.rname = dr.GetString(1);
                ress.rlocation = dr.GetString(2);
                ress.ownerid = dr.GetInt64(3);
                ress.ownername = dr.GetString(5);
                res.Add(ress);
            }
            dr.Close();
            return res;
        }

        public List<usersDTO> getowners()
        {
            List<usersDTO> users = new List<usersDTO>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "Select * from Users where role=3";
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                usersDTO user = new usersDTO();
                user.userid = dr.GetInt64(0);
                user.uname = dr.GetString(1);
                user.email = dr.GetString(2);
                user.password = dr.GetString(3);
                user.roledal = (usersDTO.role)dr.GetInt64(4);
                user.ulocation = dr.GetString(5);
                users.Add(user);
            }
            dr.Close();
            return users;
        }

        public bool edituser(usersDTO user)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = $"Update Users set uname='{user.uname}',email='{user.email}',password='{user.password}',role={(long)user.roledal},ulocation='{user.ulocation}' where userid={user.userid}";
            int res = cmd.ExecuteNonQuery();
            if (res > 0)
                return true;
            else
                return false;
        }

        public bool editrestaurant(restaurantDTO res)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = $"Update Restaurants set rname='{res.rname}',rlocation='{res.rlocation}',ownerid={res.ownerid} where rid={res.resid}";
            int ress = cmd.ExecuteNonQuery();
            if (ress > 0)
                return true;
            else
                return false;
        }

        public List<restaurantDTO> ListMyRest(long ownerid)
        {
            List<restaurantDTO> res = new List<restaurantDTO>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = $"Select * from Restaurants where ownerid={ownerid}";
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                restaurantDTO ress = new restaurantDTO();
                ress.resid = dr.GetInt64(0);
                ress.rname = dr.GetString(1);
                ress.rlocation = dr.GetString(2);
                ress.ownerid = dr.GetInt64(3);
                res.Add(ress);
            }
            dr.Close();
            return res;
        }

        public bool deleterestaurant(long resid)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = $"Delete from Restaurants where rid={resid}";
            int res = cmd.ExecuteNonQuery();
            if (res > 0)
                return true;
            else
                return false;
        }
        public List<restaurantDTO> Searchrestaurant(string location)
        {
            List<restaurantDTO> res = new List<restaurantDTO>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = $"Select * from Restaurants where rlocation='{location}'";
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                restaurantDTO ress = new restaurantDTO();
                ress.resid = dr.GetInt64(0);
                ress.rname = dr.GetString(1);
                ress.rlocation = dr.GetString(2);
                ress.ownerid = dr.GetInt64(3);
                res.Add(ress);
            }
            dr.Close();
            return res;
        }
    }
}
