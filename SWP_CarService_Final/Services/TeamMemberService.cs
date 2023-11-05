using SWP_CarService_Final.Areas.User.Models;
using SWP_CarService_Final.Models;
using System.Data.SqlClient;

namespace SWP_CarService_Final.Services
{
    public class TeamMemberService : DBContext
    {

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
                                                    "join [Role] on [Role].[role_id] = [User_role].[role_id] " +
                                                    "where [Team_Members].[team_id] = @team_id " +
                                                    "order by [Role].[role_name]", connection);
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


        public bool CheckLeaderExist(string teamId)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM [Team_Members] " +
                                            "WHERE [team_id] = @team_id " +
                                            "AND [userName] IN (SELECT [userName] FROM [User_role] " +
                                            "WHERE [role_id] = (SELECT [role_id] FROM [Role] WHERE [role_name] = 'leader'))", connection);
                command.Parameters.AddWithValue("team_id", teamId);
                int leaderCount = (int)command.ExecuteScalar();
                return leaderCount > 0;
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

        public bool CheckIfListMemberExistLeader(List<string> username)
        {
            User user = null;
            foreach (string member in username)
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("select [User].*, [Role].[role_name] " +
                                                        "from [User] " +
                                                        "join [User_role] on [User_role].[userName] = [User].[UserName] " +
                                                        "join [Role] on [Role].[role_id] = [User_role].[role_id] " +
                                                        "where [User].[UserName] = @username and [Role].[role_name] = 'leader'", connection);
                    command.Parameters.AddWithValue("username", member);
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
                    if (user != null)
                    {
                        return true;
                    }
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
                finally { connection.Close(); }
            }
            return false;
        }

        public User GetRoleNameByRoleID(string roleId)
        {
            User user = null;
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select [Role].[role_id], [Role].[role_name], [User].[UserName] " +
                                                    "from [Role] " +
                                                    "join[User_role] on [User_role].[role_id] = [Role].[role_id] " +
                                                    "join [User] on [User].[UserName] = [User_role].[userName] " +
                                                    "join [Team_Members] on [Team_Members].[userName] = [User].[UserName] " +
                                                    "join [Team] on [Team].[team_id] = [Team_Members].[team_id] " +
                                                    "where [Role].[role_id] = @roleId", connection);
                command.Parameters.AddWithValue("roleId", roleId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User()
                        {
                            role_name = reader.GetString(1).Trim(),
                            UserName = reader.GetString(2).Trim(),
                        };
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { connection.Close(); }
            return user;
        }

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


        public List<User> getMembersOfLeader(string UserName)
        {
            var users = new List<User>();
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select * from [user] " +
                    "join Team_Members on [user].UserName = Team_Members.userName where Team_Members.team_id = " +
                    "(select team_id from Team_Members join [user] on [user].UserName = Team_Members.userName where [User].UserName = @username) " +
                    "and [user].UserName not in " +
                    "(select [user].UserName from [user] join User_role on [user].UserName = User_role.userName join [Role] on User_role.role_id = [Role].role_id " +
                    "where [Role].role_name = 'leader')" +
                    "and [user].UserName not in (select task_detail.userName from task_detail where task_detail.status = 'Process')", connection);
                command.Parameters.AddWithValue("username", UserName);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User user = new User()
                        {
                            UserName = reader.GetString(0),
                            User_fullname = reader.GetString(1),
                            phone_number = reader.GetString(2),
                            email = reader.GetString(3),
                            password = reader.GetString(4),
                            account_status = reader.GetBoolean(5),
                            created = reader.GetDateTime(6),
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

    }
}
