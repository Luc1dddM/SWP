﻿using SWP_CarService_Final.Models;
using System.Data.SqlClient;

namespace SWP_CarService_Final.Services
{
    public class AccountService : DBContext
    {
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
                    if(reader.Read())
                    {
                        customer = new Models.Customer()
                        {
                            user_name = reader.GetString(0),
                            password = reader.GetString(1),
                            fullName = reader.GetString(2),
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

        public void CreateCustomer (Customer customer)
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

    }
}
