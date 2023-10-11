using NuGet.Protocol.Plugins;
using SWP_CarService_Final.Models;
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

        public Customer login(string username, string password)
        {

            _dbContext._connection().Open();
            SqlCommand command = new SqlCommand("select * from Customer where user_name = @username and password = @password", _dbContext._connection());
            command.Parameters.AddWithValue("username", username);
            command.Parameters.AddWithValue("password", password);  
            using(SqlDataReader reader = command.ExecuteReader())
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

                    
                    return customer;
                }
            }
            _dbContext._connection().Close();
            return null;
        }
    }
}
