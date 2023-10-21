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

        public List<User> GetAllTeamMember()
        {
            List<User> userlist = new List<User>();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("select * from [User] " +
                    "join [User_role] on [User_role].[userName] = [User].[UserName] " +
                    "join [Role] on [Role].[role_id] = [User_role].[role_id] " +
                    "where [Role].[role_name] = 'member' khhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh", connection);
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
                                                           "[User].[created] " +
                                                    "from [Team_Members] " +
                                                    "join [Team] on [Team].[team_id] = [Team_Members].[team_id] " +
                                                    "join [User] on [User].[UserName] = [Team_Members].[userName] " +
                                                    "where [Team_Members].[team_id] = @team_id", connection);
                command.Parameters.AddWithValue("team_id", teamId);
                using (SqlDataReader reader = command.ExecuteReader())
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



        /*public int GetNumberOfMember()
        {
            User user = null;
            connection.Open();
            SqlCommand cmd = new SqlCommand("select top 1 * from [User] order by [team_id] desc");
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = new User()
                    {

                    };
                }
            }
        }*/


        public void AddTeamMember(string teamId, List<string> username)
        {
            foreach (var member in username)
            {
                try
                {
                    connection.Open();
                    /*SqlCommand command = new SqlCommand("INSERT INTO Team_Members (userName, team_id, created) " +
                                                        "VALUES ((SELECT U.UserName FROM [User] U WHERE U.[UserName] = @UserName), " +
                                                                "(SELECT T.team_id FROM Team T WHERE T.[team_id] = @team_id), " +
                                                                "@created");*/
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


    }
}
