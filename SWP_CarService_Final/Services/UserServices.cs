using NuGet.Protocol.Plugins;
using SWP_CarService_Final.Areas.User.Models;
using SWP_CarService_Final.Models;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

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

        public User getUserByUsername(string userName)
        {
            User user = null;
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("select * from [user] where [user].UserName = @username", connection);
                cmd.Parameters.AddWithValue("username", userName);
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    if(reader.Read()) {
                        user = new User();
                        user.UserName = reader.GetString(0);
                        user.User_fullname = reader.GetString(1);
                        user.phone_number = reader.GetString(2);
                        user.email = reader.GetString(3);
                        user.password = reader.GetString(4);
                        user.account_status = reader.GetBoolean(5);
                        user.created = reader.GetDateTime(6);
                    }
                }
            }catch (Exception ex)
            {
                throw new Exception (ex.Message);
            }finally { connection.Close(); }
            return user;
        }

        public bool checkUserByEmail(string email)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("select * from Customer where Customer.email = @email", connection);
                cmd.Parameters.AddWithValue("email", email);
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        connection.Close();
                        Random random = new Random();
                        int myRandom = random.Next(10000000, 99999999);
                        string forgot_otp = myRandom.ToString();

                        connection.Open();
                        SqlCommand cmd1 = new SqlCommand("update Customer set forgot_otp = @value where email = @email", connection);
                        cmd1.Parameters.AddWithValue("value", forgot_otp);
                        cmd1.Parameters.AddWithValue("email", email);
                        cmd1.ExecuteNonQuery();
                       

                        MailMessage mail = new MailMessage();
                        mail.To.Add(email);
                        mail.From = new MailAddress("Lamnsce170617@fpt.edu.vn");
                        mail.Subject = "Reset passwork link";

                        string emailBody = "";

                        emailBody += "<h1>Hello  User,</h1>";
                        emailBody += "<h1>Your OTP is: "+ forgot_otp + "</h1>";
                        emailBody += "Thakyou...";

                        mail.Body = emailBody;
                        mail.IsBodyHtml = true;

                        SmtpClient smtp = new SmtpClient();
                        smtp.Port = 587; //25 465
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.Host = "smtp.gmail.com";
                        smtp.Credentials = new System.Net.NetworkCredential("Lamnsce170617@fpt.edu.vn", "axam pqux obnm nhwz");
                        smtp.Send(mail);
                        connection.Close();
                        return true;
                    }
                }
            }catch(Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
            finally { connection.Close(); }
            return false;
        }

        public void resetPassword(string password, string otp, string email)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("select * from Customer where Customer.email = @email", connection);
                cmd.Parameters.AddWithValue("email", email);
                using(SqlDataReader reader  = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader.GetString(7).Trim() == otp)
                        {
                            connection.Close();

                            connection.Open();
                            SqlCommand cmd1 = new SqlCommand("update Customer set password = @value where email = @email", connection);
                            cmd1.Parameters.AddWithValue("value", password);
                            cmd1.Parameters.AddWithValue("email", email);
                            cmd1.ExecuteNonQuery();
                            connection.Close();
                        }
                        else
                        {
                            throw new Exception("OTP is not match!");
                        }
                    }
                }
            }catch(Exception ex)
            {
                throw new Exception (ex.Message);
            }
        }
    }
}
