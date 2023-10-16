using NuGet.Protocol.Plugins;
using SWP_CarService_Final.Areas.Team.Models;
using SWP_CarService_Final.Areas.User.Models;
using SWP_CarService_Final.Models;
using System.Data;
using System.Data.SqlClient;

namespace SWP_CarService_Final.Services
{
    public class UserServices
    {

        private readonly DBContext _dbContext;

        public UserServices(DBContext dbContext)
        {
            _dbContext = dbContext;
        }


        public Customer CustomerLogin(string username, string password)
        {

            _dbContext._connection().Open();
            SqlCommand command = new SqlCommand("select * from Customer where user_name = @username and password = @password", _dbContext._connection());
            command.Parameters.AddWithValue("username", username);
            command.Parameters.AddWithValue("password", password);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {

                    Customer customer = new Customer();
                    customer.user_name = reader.GetString(0);
                    customer.fullName = reader.GetString(1);
                    customer.password = reader.GetString(2);
                    customer.email = reader.GetString(3);
                    customer.phone_number = reader.GetString(4);
                    customer.account_status = reader.GetBoolean(5);
                    customer.img = (!reader.IsDBNull(6)) ? reader.GetString(6) : "";

                    Console.WriteLine(customer.user_name);
                    Console.WriteLine(customer.password);

                    return customer;
                }
            }
            _dbContext._connection().Close();
            return null;
        }

        public User UserLogin(string username, string password)
        {
            _dbContext._connection().Open();

            SqlCommand command = new SqlCommand("select * from [User] where UserName = @username and password = @password", _dbContext._connection());
            command.Parameters.AddWithValue("username", username);
            command.Parameters.AddWithValue("password", password);
            using (SqlDataReader read = command.ExecuteReader())
            {
                if (read.Read())
                {
                    User user = new User();
                    user.UserName = read.GetString(0);
                    user.User_fullname = read.GetString(1);
                    user.phone_number = read.GetString(2);
                    user.email = read.GetString(3);
                    user.password = read.GetString(4);
                    user.account_status = read.GetBoolean(5);
                    user.created = read.GetDateTime(6);

                    Console.WriteLine(user.UserName);
                    Console.WriteLine(user.password);

                    return user;
                }

            }

            _dbContext._connection().Close();
            return null;
        }

        /*public object login(string username, string password)
        {
            _dbContext._connection().Open();
            SqlCommand command = new SqlCommand("select role_name" +
                                            "from [User]" +
                                            "join [User_role] on [User].UserName = [User_role].userName" +
                                            "join [Role] on [User_role].role_id = [Role].role_id ");

            if (command.Equals("admin") || command.Equals("leader") || command.Equals("member"))
            {
                command = new SqlCommand("select * from User where UserName = @username and Password = @password", _dbContext._connection());
                command.Parameters.AddWithValue("username", username);
                command.Parameters.AddWithValue("password", password);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        User user = new User();
                        user.UserName = reader.GetString(0);
                        user.User_fullname = reader.GetString(1);
                        user.phone_number = reader.GetString(2);
                        user.email = reader.GetString(3);
                        user.password = reader.GetString(4);
                        user.account_status = reader.GetBoolean(5);
                        user.created = reader.GetDateTime(6);

                        Console.WriteLine(user.UserName);
                        Console.WriteLine(user.password);

                        return user;
                    }
                }
            }

            else
            {
                command = new SqlCommand("select * from Customer where user_name = @username and password = @password", _dbContext._connection());
                command.Parameters.AddWithValue("username", username);
                command.Parameters.AddWithValue("password", password);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Customer customer = new Customer();
                        customer.user_name = reader.GetString(0);
                        customer.fullName = reader.GetString(1);
                        customer.password = reader.GetString(2);
                        customer.email = reader.GetString(3);
                        customer.phone_number = reader.GetString(4);
                        customer.account_status = reader.GetBoolean(5);
                        customer.img = (!reader.IsDBNull(6)) ? reader.GetString(6) : "";

                        Console.WriteLine(customer.user_name);
                        Console.WriteLine(customer.password);

                        return customer;
                    }
                }
            }
            _dbContext._connection().Close();
            return null;
        }*/



        /*public Team addTeam(string teamId, string teamName, DateTime created)
        {
            _dbContext._connection().Open();
            SqlCommand command = new SqlCommand("Add", _dbContext._connection());
            _dbContext._connection().Close();
        }*/

    }
}
