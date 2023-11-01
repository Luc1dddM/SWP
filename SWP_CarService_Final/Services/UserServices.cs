using NuGet.Protocol.Plugins;
using SWP_CarService_Final.Areas.User.Models;
using SWP_CarService_Final.Models;
using System.Data;
using System.Data.SqlClient;

namespace SWP_CarService_Final.Services
{
    public class UserServices : DBContext
    {

        



        public Customer CustomerLogin(string username, string password)
        {

            connection.Open();
            SqlCommand command = new SqlCommand("select * from [Customer] where [user_name] = @username and [password] = @password", connection);
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
                    connection.Close();
                    return customer;
                }
            }
            return null;
        }

        public User UserLogin(string username, string password)
        {
            connection.Open();

            SqlCommand command = new SqlCommand("select * from [User] " +
                                                    "join [User_role] on [User_role].[userName] = [User].[UserName] " +
                                                    "join [Role] on [Role].[role_id] = [User_role].[role_id] " +
                                                    "where [User].[UserName] = @username and [User].[password] = @password", connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
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
                    user.role_name = read.GetString(10);
                    connection.Close();
                    return user;
                }
            }

            return null;
        }

        public Customer getCustomerByUserName(string userName)
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("select * from [Customer] where [user_name] = @username", connection);
                command.Parameters.AddWithValue("username", userName);
                using(SqlDataReader reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        Customer customer = new Customer();
                        customer.user_name = reader.GetString(0);
                        customer.fullName = reader.GetString(1);
                        customer.password = reader.GetString(2);
                        customer.email = reader.GetString(3);
                        customer.phone_number = reader.GetString(4);
                        customer.account_status = reader.GetBoolean(5);
                        customer.img = (!reader.IsDBNull(6)) ? reader.GetString(6) : "";
                        connection.Close();
                        return customer;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally {
                connection.Close();
            }
            return null;
        }
    }
}
