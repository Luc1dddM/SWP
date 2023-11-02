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
                            team_id = reader.GetString(0).Trim(),
                            team_name = reader.GetString(1).Trim(),
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
            Team team = null;
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT top 1 *  FROM [Team] order by team_id desc", connection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    team = new Team()
                    {
                        team_id = reader.GetString(0).Trim(),
                        team_name = reader.GetString(1).Trim(),
                        created = reader.GetDateTime(2),
                    };
                }
            }
            connection.Close();
            return team != null ? int.Parse(team.team_id.Substring(2).Trim()) : 0;
        }
        public void CreateTeam(Team team)
        {
            try
            {
                string id = "TM" + (getNumberOfTeam() + 1);
                connection.Open();
                SqlCommand command = new SqlCommand("insert into [Team] ([Team].[team_id], [Team].[team_name], [Team].[created])" +
                                                                   "values (@team_id, @team_name, @created)", connection);
                command.Parameters.AddWithValue("team_id", id.Trim());
                command.Parameters.AddWithValue("team_name", team.team_name.Trim());
                command.Parameters.AddWithValue("created", DateTime.Now);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { connection.Close(); }
        }

        public Team GetTeamByID(string teamID)
        {
            Team team = new Team();
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select * from [Team] where [Team].[team_id] = @team_Id", connection);
                command.Parameters.AddWithValue("team_Id", teamID.Trim());
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        team = new Team()
                        {
                            team_id = reader.GetString(0).Trim(),
                            team_name = reader.GetString(1).Trim(),
                            created = reader.GetDateTime(2),
                        };
                    }
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
            return team;
        }

        public void DeleteTeam(string teamID)
        {
            try
            {
                Team team = GetTeamByID(teamID);
                if (team != null)
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("delete from [Team] where [Team].[team_name] = @team_name", connection);
                    command.Parameters.AddWithValue("team_name", team.team_name.Trim());
                    command.ExecuteNonQuery();
                }
                else
                {
                    throw new Exception("Team is not exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { connection.Close(); }
        }

        public void Edit_Team(Team team)
        {
            try
            {
                Team Eteam = GetTeamByID(team.team_id);
                if (Eteam != null)
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("update [Team]" +
                                                        "set [team_name] = @team_name " +
                                                        "where [team_id] = @team_id", connection);
                    command.Parameters.AddWithValue("team_id", team.team_id.Trim());
                    command.Parameters.AddWithValue("team_name", team.team_name.Trim());
                    command.Parameters.AddWithValue("created", DateTime.Now);
                    command.ExecuteNonQuery();
                }
                else
                {
                    throw new Exception("Team is not exist.");
                }
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            }
            finally { connection.Close(); }
        }
    }
}
