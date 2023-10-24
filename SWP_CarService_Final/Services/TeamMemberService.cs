using SWP_CarService_Final.Areas.User.Models;
using SWP_CarService_Final.Models;
using System.Data.SqlClient;

namespace SWP_CarService_Final.Services
{
    public class TeamMemberService : DBContext
    {
        /*private UserAccountServices userAccountServices;
        public TeamMemberService()
        {
            userAccountServices = new UserAccountServices();
        }
        public List<User> GetAllUser()
        {
            List<User> users = userAccountServices.getAllListUserAccounts();
            return users;
        }*/

        public List<User> GetAllTeamMemberNotInTeam()
        {
            List<User> userlist = new List<User>();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("select * from [User] " +
                                                "join [User_role] on [User_role].[userName] = [User].[UserName] " +
                                                "join [Role] on [Role].[role_id] = [User_role].[role_id] " +
                                                "left join [Team_Members] on [Team_Members].[userName] = [User].[UserName] " +
                                                "left join [Team] on [Team].[team_id] = [Team_Members].[team_id] " +
                                                "where [Role].[role_name] in ('member', 'leader') and [Team_Members].[team_id] is null", connection);
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
                            created = reader.GetDateTime(6)
                        };
                        userlist.Add(user);
                    }
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
            return userlist;
        }

        public List<User> GetMemberByTeamID(string teamId)
        {
            List<User> users = new List<User>();
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("select [User].[UserName], " +
                                                           "[User].[User_fullname], " +
                                                           "[User].[phone_number], " +
                                                           "[User].[email], " +
                                                           "[User].[password], " +
                                                           "[User].[account_status], " +
                                                           "[User].[created], " +
                                                           "[Role].[role_name] " +
                                                    "from [Team_Members] " +
                                                    "join [Team] on [Team].[team_id] = [Team_Members].[team_id] " +
                                                    "join [User] on [User].[UserName] = [Team_Members].[userName] " +
                                                    "join [User_role] on [User_role].[userName] = [User].[UserName] " +
                                                    "join [Role] on [Role].[role_id] = [User_role].[role_id]" +
                                                    "where [Team_Members].[team_id] = @team_id", connection);
                command.Parameters.AddWithValue("team_id", teamId);
                using (SqlDataReader reader = command.ExecuteReader())
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
                            role_name = reader.GetString(7).Trim(),
                        };
                        users.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { connection.Close(); }
            return users;
        }

        public User GetTeamMemberByUserName(string username)
        {
            User user = new User();
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select [User].*, [Role].[role_name] " +
                                                    "from [User] " +
                                                    "join [User_role] on [User_role].[userName] = [User].[UserName] " +
                                                    "join [Role] on [Role].[role_id] = [User_role].[role_id] " +
                                                    "where [User].[UserName] = @Username", connection);
                command.Parameters.AddWithValue("Username", username);
                using (SqlDataReader reader = command.ExecuteReader())
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
                            role_name = reader.GetString(7).Trim(),
                        };
                    }
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
            return user;
        }

        /*public List<User> GetAllUserRole()
        {
            List<User> users = new List<User>();
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select distinct [Role].[role_name] " +
                                                    "from [User] " +
                                                    "join [User_role] on [User_role].[userName] = [User].[UserName] " +
                                                    "join [Role] on [Role].[role_id] = [User_role].[role_id] " +
                                                    "where [Role].[role_name] in ('leader', 'member')", connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new User()
                        {
                            role_name = reader.GetString(0).Trim(),
                        };
                        users.Add(user);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { connection.Close(); }
            return users;
        }*/

        public void AddTeamMember(string teamId, List<string> username)
        {
            foreach (var member in username)
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("insert into [Team_Members] ([userName], [team_id], [created]) " +
                                                                            "values (@userName, @team_id, @created)", connection);

                    command.Parameters.AddWithValue("team_id", teamId);
                    command.Parameters.AddWithValue("userName", member);
                    command.Parameters.AddWithValue("created", DateTime.Now);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally { connection.Close(); }
            }
        }

        public void DeteleMemberFromTeam(string username)
        {
            try
            {

                User user = GetTeamMemberByUserName(username);
                if (user != null)
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("delete " +
                                                        "from [Team_Members] " +
                                                        "where [Team_Members].[userName] = @username", connection);
                    command.Parameters.AddWithValue("username", username);
                    command.ExecuteNonQuery();
                }
                else { throw new Exception("Member with username: " + username + " is not exist."); }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
        }

        public void EditTeamMemberRoleByUserName(string username, string role_id)
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("UPDATE [User_role] SET [User_role].[role_id] = @role_id " +
                                                    "from [User_role] " +
                                                    "WHERE [User_role].[userName] = @username", connection);
                command.Parameters.AddWithValue("@role_id", role_id);
                command.Parameters.AddWithValue("@username", username);
                command.ExecuteNonQuery();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
        }

        public void EditMemberTeam(string username, string team_id)
        {
            try
            {

                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("UPDATE [Team_Members] SET [team_id] = @team_id " +
                                                    "from [Team_Members] " +
                                                    "WHERE [Team_Members].userName = @username", connection);
                command.Parameters.AddWithValue("@team_id", team_id);
                command.Parameters.AddWithValue("@username", username);
                command.ExecuteNonQuery();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
        }

    }
}
