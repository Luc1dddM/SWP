using SWP_CarService_Final.Models;
using System.Data.SqlClient;

namespace SWP_CarService_Final.Services
{
    public class CustomerAccountService : DBContext
    {
       /* readonly string rootFolder = @"D:\FPT\SWP391\Garage\SWP_CarService_Final\wwwroot\img";*/
        public Models.Customer GetCustomerByUsername (string username)
        {
            Models.Customer customer = null;
            try
            {
                connection.Open();
                String SQLSelect = "SELECT * FROM [SWP].[dbo].[CUSTOMER] WHERE user_name = @user_name";
                SqlCommand command = new SqlCommand(SQLSelect, connection);
                command.Parameters.AddWithValue("user_name", username);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        customer = new Models.Customer()
                        {
                            user_name = reader.GetString(0),
                            fullName = reader.GetString(1),
                            password = reader.GetString(2),
                            email = reader.GetString(3),
                            phone_number = reader.GetString(4),
                            account_status = reader.GetBoolean(5),
                            img = (!reader.IsDBNull(4) ? reader.GetString(4) : "")
                        };
                    }
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally
            {
                connection.Close();
            }
            return customer;
        }

        public void CreateCustomer(Customer customer)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO [dbo].[Customer] ([user_name],[fullname]," +
                    "[password],[email],[phone_number],[account_status]) " +
                    "VALUES (@user_name, @fullname, @password, @email, @phone_number, @account_status)", connection);
                command.Parameters.AddWithValue("user_name", customer.user_name);
                command.Parameters.AddWithValue("fullname", customer.fullName);
                command.Parameters.AddWithValue("password", customer.password);
                command.Parameters.AddWithValue("email", customer.email);
                command.Parameters.AddWithValue("phone_number", customer.phone_number);
                command.Parameters.AddWithValue("account_status", customer.account_status);
                /* command.Parameters.AddWithValue("img", customer.img);*/

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public void editProfile (Customer customer)
        {
            Customer cus = new Customer();
            try
            {
                cus = GetCustomerByUsername(customer.user_name);
                connection.Open();
                if (cus != null)
                {
                    SqlCommand command = new SqlCommand("UPDATE [dbo].[Customer] SET [fullname] = @fullname," +
                        "[email] = @email," +
                        "[phone_number] = @phone_number, " +
                        "[account_status] = @account_status, " +
                        "[image] = @image " +
                        "WHERE [user_name] = @user_name", connection);
                    command.Parameters.AddWithValue("user_name", customer.user_name);
                    command.Parameters.AddWithValue("fullname", customer.fullName);
                    command.Parameters.AddWithValue("email", customer.email);
                    command.Parameters.AddWithValue("phone_number", customer.phone_number);
                    command.Parameters.AddWithValue("account_status", customer.account_status);
                    command.Parameters.AddWithValue("image", customer.img);

                    command.ExecuteNonQuery();
                }
                else
                {
                    throw new Exception("This user does not already exsist.");
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
        }

        public List<Customer> GetAllCustomer()
        {
            List<Customer> customers = new List<Customer>();
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select * from [Customer]", connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var customer = new Customer()
                        {
                            user_name = reader.GetString(0),
                            fullName = reader.GetString(1),
                            password = reader.GetString(2),
                            email = reader.GetString(3),
                            phone_number = reader.GetString(4),
                            account_status = reader.GetBoolean(5),
                            img = (!reader.IsDBNull(6)) ? reader.GetString(6) : "",
                        };
                        customers.Add(customer);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { connection.Close(); }
            return customers;
        }

        public void DeleteCustomer(string user_name)
        {
            try
            {
                Customer customer = GetCustomerByUsername(user_name);
                connection.Open();
                if (user_name != null)
                {

                    SqlCommand command = new SqlCommand("DELETE FROM [dbo].[Customer] WHERE user_name = @user_name", connection);
                    command.Parameters.AddWithValue("user_name", user_name);
                    command.ExecuteNonQuery();
                }
                else
                {
                    throw new Exception("The customer does not already exist.");
                }
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
           finally
            {
                connection.Close();
            }
        }
    }
}
