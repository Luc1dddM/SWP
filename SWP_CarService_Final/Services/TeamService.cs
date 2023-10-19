using SWP_CarService_Final.Models;
using System.Data.SqlClient;


namespace SWP_CarService_Final.Services
{
    public class TeamService : DBContext
    {
        public List<Team> GetAllTeam()
        {
            List<Team> teams = new List<Team>();
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select * from [Team]", connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var team = new Team()
                        {
                            team_id = reader.GetString(0),
                            team_name = reader.GetString(1),
                            created = reader.GetDateTime(2)
                        };
                        teams.Add(team);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { connection.Close(); }
            return teams;
        }


        public int getNumberOfTeam()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("select count(*) from [Team]", connection);
            Int32 count = (Int32)command.ExecuteScalar();
            connection.Close();
            return count;
        }
        public void CreateTeam(Team team)
        {
            try
            {
                string id = (getNumberOfTeam() + 1).ToString(); 
                connection.Open();
                SqlCommand command = new SqlCommand("insert into [Team] ([Team].[team_id], [Team].[team_name], [Team].[created])" +
                                                                   "values (@team_id, @team_name, @created)", connection);
                command.Parameters.AddWithValue("team_id", id);
                command.Parameters.AddWithValue("team_name", team.team_name);
                command.Parameters.AddWithValue("created", DateTime.Now);

                command.ExecuteNonQuery();
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
            finally { connection.Close(); }
        }

        public void DeleteTeam(Team team)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("delete from [Team] where [Team].[team_name] = @team_name", connection);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { connection.Close(); }
        }
    }
}
