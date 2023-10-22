using SWP_CarService_Final.Areas.User.Models;
using System.Data.SqlClient;

namespace SWP_CarService_Final.Services
{
    public class UserProfileServices : DBContext
    {
        public User GetUserByUserName(string userName)
        {
            User user = null;
            try
            { 
                connection.Open();
                string SQLSelect = "SELECT * FROM [SWP].[dbo].[User] WHERE [UserName] = @UserName";
                SqlCommand command = new SqlCommand(SQLSelect, connection);
                command.Parameters.AddWithValue("UserName", userName);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User()
                        {
                            UserName = reader.GetString(0),
                            User_fullname = reader.GetString(1),
                            phone_number = reader.GetString(2),
                            email = reader.GetString(3),
                            password = reader.GetString(4),
                            account_status = reader.GetBoolean(5),
                            created = reader.GetDateTime(6),
                        };
                    }
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally
            {
                connection.Close();
            }
            return user;
        }

        public void editUserProfile (User user)
        {
            User us = new User();
            try
            {
                us = GetUserByUserName(user.UserName);
                connection.Open();
                if (us != null)
                {
                    SqlCommand command = new SqlCommand("UPDATE [dbo].[User] SET [User_fullname] = @User_fullname, " +
                        "[phone_number] = @phone_number, " +
                        "[email] = @email, " +
                        "[account_status] = @account_status " +
                        "WHERE [UserName] = @UserName", connection);

                    command.Parameters.AddWithValue("UserName", user.UserName);
                    command.Parameters.AddWithValue("User_fullname", user.User_fullname);
                    command.Parameters.AddWithValue("phone_number", user.phone_number);
                    command.Parameters.AddWithValue("email", user.email);
                    command.Parameters.AddWithValue("account_status", user.account_status);

                    command.ExecuteNonQuery();
                }
                else
                {
                    throw new Exception("This user does not already exsist.");
                }
            }
            catch (Exception ex) { throw new Exception(); }
            finally {  connection.Close(); }
            }
        }
    }

