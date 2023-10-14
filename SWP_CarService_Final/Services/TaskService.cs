using System.Data.SqlClient;
using SWP_CarService_Final.Models;

namespace SWP_CarService_Final.Services
{
    public class TaskService : DBContext
    {

        public List<Models.Task> getAllTasks()
        {
            List<Models.Task> tasks = new List<Models.Task>();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("Select * from Task", connection);
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var task = new Models.Task()
                        {
                           taskID = reader.GetString(0),
                           taskName = reader.GetString(1),
                           price = reader.GetDecimal(2),
                           actice = reader.GetBoolean(3),
                           img = (!reader.IsDBNull(4)) ? reader.GetString(4) : "",
                        };
                        tasks.Add(task);
                    }
                }
            }finally { connection.Close(); }
            return tasks;
        }
    }
}
