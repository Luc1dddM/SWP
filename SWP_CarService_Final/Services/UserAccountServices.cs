using SWP_CarService_Final.Areas.User.Models;
using System.Data;
using System.Data.SqlClient;
using User = SWP_CarService_Final.Areas.User.Models.User;

namespace SWP_CarService_Final.Services
{
    public class UserAccountServices : DBContext
    {
        public List<User> getAllListUserAccounts()
        {
            List<User> userAccounts = new List<User>();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT [User].*, [Role].role_name " +
                    "FROM [User], User_role,[Role] WHERE [User].UserName = User_role.userName " +
                    "AND User_role.role_id = [Role].role_id", connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new User()
                        {
                            UserName = reader.GetString(0).Trim(),
                            User_fullname = reader.GetString(1).Trim(),
                            phone_number = reader.GetString(2).Trim(),
                            email = reader.GetString(3).Trim(),
                            password = reader.GetString(4).Trim(),
                            account_status = reader.GetBoolean(5),
                            created = reader.GetDateTime(6),
                            role_name = reader.GetString(7).Trim()
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
                SqlCommand cmd = new SqlCommand(SQLSelect, connection);
                cmd.Parameters.AddWithValue("UserName", userName.Trim());
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User()
                        {
                            UserName = reader.GetString(0).Trim(),
                            User_fullname = reader.GetString(1).Trim(),
                            phone_number = reader.GetString(2).Trim(),
                            email = reader.GetString(3).Trim(),
                            password = reader.GetString(4).Trim(),
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
        public User getUserAndRoleNameByUserName(string userName)
        {
            User user = null;
            try
            {
                connection.Open();
                string SQLSelect = "SELECT u.*, r.role_name FROM [User] u INNER JOIN User_role ur ON u.UserName = ur.userName INNER JOIN [Role] r ON ur.role_id = r.role_id WHERE u.UserName = @UserName";
                SqlCommand cmd = new SqlCommand(SQLSelect, connection);
                cmd.Parameters.AddWithValue("UserName", userName.Trim());
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User()
                        {
                            UserName = reader.GetString(0).Trim(),
                            User_fullname = reader.GetString(1).Trim(),
                            phone_number = reader.GetString(2).Trim(),
                            email = reader.GetString(3).Trim(),
                            password = reader.GetString(4).Trim(),
                            account_status = reader.GetBoolean(5),
                            created = reader.GetDateTime(6),
                            role_name = reader.GetString(7).Trim()
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

        public void setUserRoleByUsername(string userName, string roleId)
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("INSERT INTO User_Role (userName, role_id) VALUES (@UserName, @RoleId)", connection);
                cmd.Parameters.AddWithValue("UserName", userName.Trim());
                cmd.Parameters.AddWithValue("RoleId", roleId.Trim());
                cmd.ExecuteNonQuery();
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

        public void createAccount(User user, string roleId)
        {
            try
            {
                User uc = getUserByUserName(user.UserName);
                if (uc == null)
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("INSERT [User] ([UserName], [User_fullname], [phone_number], [email], [password], [account_status], [created]) " +
                        "values (@UserName, @User_fullname, @phone_number, @email, @password, @account_status, CURRENT_TIMESTAMP);", connection);
                    command.Parameters.AddWithValue("UserName", user.UserName.Trim());
                    command.Parameters.AddWithValue("User_fullname", user.User_fullname.Trim());
                    command.Parameters.AddWithValue("phone_number", user.phone_number.Trim());
                    command.Parameters.AddWithValue("email", user.email.Trim());
                    command.Parameters.AddWithValue("password", user.password.Trim());
                    command.Parameters.AddWithValue("account_status", user.account_status);
                    command.Parameters.AddWithValue("created", user.created);
                    command.ExecuteNonQuery();
                    setUserRoleByUsername(user.UserName, roleId);
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


        public void deleteUserRole(string UserName)
        {
            try
            {
                    connection.Open();
                    SqlCommand deleteRoleCommand = new SqlCommand("DELETE FROM User_Role WHERE userName = @UserName", connection);
                    deleteRoleCommand.Parameters.AddWithValue("UserName", UserName.Trim());
                    deleteRoleCommand.ExecuteNonQuery();
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
                User user = getUserByUserName(UserName.Trim());
                
                if (user != null)
                {
                    deleteUserRole(UserName.Trim());
                    connection.Open();
                    SqlCommand command = new SqlCommand("DELETE FROM [User] WHERE UserName = @UserName", connection);
                    command.Parameters.AddWithValue("UserName", UserName.Trim());
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

        public void EditUserRoleByUserName(string userName, string roleId)
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("UPDATE User_role SET role_id = @RoleId WHERE userName = @UserName", connection);
                cmd.Parameters.AddWithValue("UserName", userName.Trim());
                cmd.Parameters.AddWithValue("RoleId", roleId.Trim());

                cmd.ExecuteNonQuery();
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
        public void editAccount(User uName, string roleId)
        {
            User user = new User();
            try
            {
                user = getUserByUserName(uName.UserName.Trim());
                connection.Open();
                if (user != null)
                {
                    SqlCommand command = new SqlCommand("UPDATE [User] SET User_fullname = @User_fullname, " +
                        "phone_number = @phone_number, email = @email, [password] = @password, " +
                        "account_status = @account_status, created = @created where " +
                        "UserName = @UserName", connection);
                    command.Parameters.AddWithValue("UserName", uName.UserName.Trim());
                    command.Parameters.AddWithValue("User_fullname", uName.User_fullname.Trim());
                    command.Parameters.AddWithValue("phone_number", uName.phone_number.Trim());
                    command.Parameters.AddWithValue("email", uName.email.Trim());
                    command.Parameters.AddWithValue("password", uName.password.Trim());
                    command.Parameters.AddWithValue("account_status", uName.account_status);
                    command.Parameters.AddWithValue("created", uName.created);

                    command.ExecuteNonQuery();
                    EditUserRoleByUserName(uName.UserName, roleId);


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
