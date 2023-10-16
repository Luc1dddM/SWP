using SWP_CarService_Final.Models;
using System.Data.SqlClient;
using SWP_CarService_Final.Models;
using System.Data;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace SWP_CarService_Final.Services
{
    public class TaskService : DBContext
    {
        readonly string rootFolder = @"D:\FPT\SWP391\Garage\SWP_CarService_Final\wwwroot\img";
        public List<Models.Task> getAllTasks()
        {
            List<Models.Task> tasks = new List<Models.Task>();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("Select * from Task", connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var task = new Models.Task()
                        {
                            taskID = reader.GetString(0),
                            taskName = reader.GetString(1),
                            price = reader.GetDecimal(2),
                            active = reader.GetBoolean(3),
                            img = (!reader.IsDBNull(4)) ? reader.GetString(4) : "",
                            Description = (!reader.IsDBNull(5)) ? reader.GetString(5) : "",
                        };
                        tasks.Add(task);
                    }
                }
            }
            finally { connection.Close(); }
            return tasks;
        }

        public Models.Task GetTaskByID(string taskID)
        {
            Models.Task task = null;

            try
            {
                connection.Open();
                string SQLSelect = "SELECT * FROM [SWP].[dbo].[Task] WHERE task_id = @task_id";
                SqlCommand command = new SqlCommand(SQLSelect, connection);
                command.Parameters.AddWithValue("task_id", taskID);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        task = new Models.Task()
                        {
                            taskID = reader.GetString(0),
                            taskName = reader.GetString(1),
                            price = reader.GetDecimal(2),
                            active = reader.GetBoolean(3),
                            img = (!reader.IsDBNull(4)) ? reader.GetString(4) : "",
                            Description = (!reader.IsDBNull(5)) ? reader.GetString(5) : "",
                        };
                    }
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally
            {
                connection.Close();
            }
            return task;
        }



        public void Remove(string taskId)
        {
            // Files to be deleted
            string imgFile = "";

            try
            {

                Models.Task task = GetTaskByID(taskId);
                connection.Open();

                if (task != null)
                {
                    if (task.img != null)
                    {
                        imgFile = task.img;
                        // Check if file exists with its full path
                        if (File.Exists(Path.Combine(rootFolder, imgFile)))
                        {
                            // If file found, delete it
                            File.Delete(Path.Combine(rootFolder, imgFile));
                            string SQLDelete = "DELETE FROM [SWP].[dbo].[Task] WHERE task_id = @task_id";
                            SqlCommand command = new SqlCommand(SQLDelete, connection);
                            command.Parameters.AddWithValue("task_id", taskId);
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            throw new Exception("The img does not already exist.");
                        }
                    }
                    else
                    {
                        string SQLDelete = "DELETE FROM [SWP].[dbo].[Task] WHERE task_id = @task_id";
                        SqlCommand command = new SqlCommand(SQLDelete, connection);
                        command.Parameters.AddWithValue("task_id", taskId);
                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    throw new Exception("The service does not already exist.");
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { connection.Close(); }
        }

        public int getNumberOfService()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("select count(*) from Task", connection);
            Int32 count = (Int32)command.ExecuteScalar();
            connection.Close();
            return count;
        }

        public void createService(Models.Task ntask)
        {
            Models.Task task = new Models.Task();
            try
            {
                string id = "TASK" + (getNumberOfService() + 1);
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO[SWP].[dbo].[Task] ([task_id],[task_name],[task_price],[task_active],[image],[Description]) " +
                    "values(@task_id,@task_name, @task_price, @task_active, @img,@description)", connection);
                command.Parameters.AddWithValue("task_id", id);
                command.Parameters.AddWithValue("task_name", ntask.taskName);
                command.Parameters.AddWithValue("task_price", ntask.price);
                command.Parameters.AddWithValue("task_active", ntask.active);
                command.Parameters.AddWithValue("img", ntask.img);
                command.Parameters.AddWithValue("description", ntask.Description);

                command.ExecuteNonQuery();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
        }

        public void editService(Models.Task ntask)
        {
            Models.Task task = new Models.Task();
            try
            {
                task = GetTaskByID(ntask.taskID);
                connection.Open();
                if (task != null)
                {
                    SqlCommand command = new SqlCommand("UPDATE [SWP].[dbo].[Task] SET[task_name] = @task_name, " +
                                                        "[task_price] = @task_price, [task_active] = @task_active, " +
                                                        "[image] = @img, [Description] = @description " +
                                                        "WHERE[task_id] = @task_id", connection);
                    command.Parameters.AddWithValue("task_id", ntask.taskID);
                    command.Parameters.AddWithValue("task_name", ntask.taskName);
                    command.Parameters.AddWithValue("task_price", ntask.price);
                    command.Parameters.AddWithValue("task_active", ntask.active);
                    command.Parameters.AddWithValue("img", ntask.img);
                    command.Parameters.AddWithValue("description", ntask.Description);
                    command.ExecuteNonQuery();

                }
                else
                {
                    throw new Exception("This service does not already exist.");
                }

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
        }
    }



}
