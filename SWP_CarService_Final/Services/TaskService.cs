using System.Data.SqlClient;
using SWP_CarService_Final.Models;
using Task = SWP_CarService_Final.Models.Task;

namespace SWP_CarService_Final.Services
{
    public class TaskService : DBContext
    {

        public List<Task> getAllTasks()
        {
            List<Task> tasks = new List<Task>();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("Select * from Task", connection);
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var task = new Task()
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

        public Task getTaskByID(string id)
        {
            Task task = null;
            try
            {
                if(connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("Select * from Task where task_id = @id", connection);
                cmd.Parameters.AddWithValue("id", id);
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        task = new Task()
                        {
                            taskID = reader.GetString(0),
                            taskName = reader.GetString(1),
                            price = reader.GetDecimal(2),
                            actice = reader.GetBoolean(3),
                            img = (!reader.IsDBNull(4)) ? reader.GetString(4) : "",
                        };
                    }
                }
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return task;
        }
    }
}
