using System.Data.SqlClient;
using User = SWP_CarService_Final.Areas.User.Models.User;

namespace SWP_CarService_Final.Services
{
    public class UserAccount : DBContext
    {
        public List<User> getAllListUserAccounts()
        {
            List<User> userAccounts = new List<User>();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("Select * from [User]", connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new User()
                        {
                            UserName = reader.GetString(0),
                            User_fullname = reader.GetString(1),
                            phone_number = reader.GetString(2),
                            email = reader.GetString(3),
                            password = reader.GetString(4),
                            account_status = reader.GetBoolean(5),
                            created = reader.GetDateTime(6),
                        };
                        userAccounts.Add(user);
                    }

                }

            }
            finally
            {
                connection.Close();
            }
            return userAccounts;
        }
        public User getUserByUserName(string userName)
        {
            User user = null;
            try
            {
                connection.Open();
                string SQLSelect = "SELECT * FROM [User] WHERE UserName = @UserName";
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
        public void createAccount(User user)
        {
            try
            {
                User uc = getUserByUserName(user.UserName);
                if (uc == null)
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("INSERT [User] ([UserName], [User_fullname], [phone_number], [email], [password], [account_status], [created]) " +
                        "values (@UserName, @User_fullname, @phone_number, @email, @password, @account_status, CURRENT_TIMESTAMP);", connection);
                    command.Parameters.AddWithValue("UserName", user.UserName);
                    command.Parameters.AddWithValue("User_fullname", user.User_fullname);
                    command.Parameters.AddWithValue("phone_number", user.phone_number);
                    command.Parameters.AddWithValue("email", user.email);
                    command.Parameters.AddWithValue("password", user.password);
                    command.Parameters.AddWithValue("account_status", user.account_status);
                    command.Parameters.AddWithValue("created", user.created);

                    command.ExecuteNonQuery();
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
        public void deleteAccount(string UserName)
        {
            try
            {
                User user = getUserByUserName(UserName);
                if (user != null)
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("DELETE FROM [User] WHERE UserName = @UserName", connection);
                    command.Parameters.AddWithValue("UserName", UserName);
                    command.ExecuteNonQuery();
                }
                else
                {
                    throw new Exception("The Username does not already exist.");
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
        public void editAccount(User uName)
        {
            User user = new User();
            try
            {
                user = getUserByUserName(uName.UserName);
                connection.Open();
                if (user != null)
                {
                    SqlCommand command = new SqlCommand("UPDATE [User] SET User_fullname = @User_fullname, " +
                        "phone_number = @phone_number, email = @email, [password] = @password, " +
                        "account_status = @account_status where " +
                        "UserName = @UserName", connection);
                    command.Parameters.AddWithValue("UserName", uName.UserName);
                    command.Parameters.AddWithValue("User_fullname", uName.User_fullname);
                    command.Parameters.AddWithValue("phone_number", uName.phone_number);
                    command.Parameters.AddWithValue("email", uName.email);
                    command.Parameters.AddWithValue("password", uName.password);
                    command.Parameters.AddWithValue("account_status", uName.account_status);

                    command.ExecuteNonQuery();
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
