using SWP_CarService_Final.Models;
using System.Data.SqlClient;
using SWP_CarService_Final.Models;
using Task = SWP_CarService_Final.Models.Task;
using System.Data;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using NuGet.Protocol.Plugins;

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
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var task = new Task()
                        {
                            taskID = reader.GetString(0),
                            taskName = reader.GetString(1),
                            price = reader.GetDecimal(2),
                            active = reader.GetBoolean(3),
                            Description = reader.GetString(4)
                        };
                        tasks.Add(task);
                    }
                }
            }
            finally { connection.Close(); }
            return tasks;
        }

        public Task getTaskByIDForAppointment(string id)
        {
            Task task = null;
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("Select * from Task where task_id = @id", connection);
                cmd.Parameters.AddWithValue("id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        task = new Task()
                        {
                            taskID = reader.GetString(0),
                            taskName = reader.GetString(1),
                            price = reader.GetDecimal(2),
                            active = reader.GetBoolean(3),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return task;
        }


        public Task GetTaskByID(string taskID)
        {
            Task task = null;
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                string SQLSelect = "SELECT * FROM [SWP].[dbo].[Task] WHERE task_id = @task_id";
                SqlCommand command = new SqlCommand(SQLSelect, connection);
                command.Parameters.AddWithValue("task_id", taskID);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        task = new Task()
                        {
                            taskID = reader.GetString(0),
                            taskName = reader.GetString(1),
                            price = reader.GetDecimal(2),
                            active = reader.GetBoolean(3),
                            Description = reader.GetString(4)
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



        /*        public void Remove(string taskId)
                {
                    // Files to be deleted
                    string imgFile = "";

                    try
                    {

                        Task task = GetTaskByID(taskId);
                        connection.Open();

                        if (task != null)
                        {
                            if (task.img != null)
                            {
                                imgFile = task.img;
                                // Check if file exists with its full path
                                if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", imgFile)))
                                {
                                    // If file found, delete it
                                    File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", imgFile));
                                    string SQLDelete = "DELETE FROM [SWP].[dbo].[Task] WHERE task_id = @task_id";
                                    SqlCommand command = new SqlCommand(SQLDelete, connection);
                                    command.Parameters.AddWithValue("task_id", taskId);
                                    command.ExecuteNonQuery();
                                }
                                else
                                {
                                    string SQLDelete = "DELETE FROM [SWP].[dbo].[Task] WHERE task_id = @task_id";
                                    SqlCommand command = new SqlCommand(SQLDelete, connection);
                                    command.Parameters.AddWithValue("task_id", taskId);
                                    command.ExecuteNonQuery();
                                }
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
                }*/

        public int getNumberOfService()
        {
            Task task = null;
            try
            {
                connection.Open();
                string SQLSelect = "SELECT top 1 * FROM Task ORDER BY CAST(SUBSTRING(task_id, PATINDEX('%[0-9]%', task_id), LEN(task_id)) AS INT) desc";
                SqlCommand command = new SqlCommand(SQLSelect, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        task = new Task()
                        {
                            taskID = reader.GetString(0),
                            taskName = reader.GetString(1),
                            price = reader.GetDecimal(2),
                            active = reader.GetBoolean(3),
                            Description = reader.GetString(4)
                        };
                    }
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally
            {
                connection.Close();
            }
            return task == null ? 0 : int.Parse(task.taskID.Substring(4));
        }

        public void createService(Task ntask)
        {
            Task task = new Task();
            try
            {
                string id = "TASK" + (getNumberOfService() + 1);
                connection.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO[SWP].[dbo].[Task] ([task_id],[task_name],[task_price],[task_active],[Description]) " +
                                    "values(@task_id,@task_name, @task_price, @task_active,@description)", connection);
                    command.Parameters.AddWithValue("task_id", id);
                    command.Parameters.AddWithValue("task_name", ntask.taskName);
                    command.Parameters.AddWithValue("task_price", ntask.price);
                    command.Parameters.AddWithValue("task_active", ntask.active);
                    command.Parameters.AddWithValue("description", ntask.Description);
                    command.ExecuteNonQuery();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
        }

        public void editService(Task ntask)
        {
            Task task = new Task();
            try
            {
                task = GetTaskByID(ntask.taskID);
                connection.Open();
                if (task != null)
                {
                    SqlCommand command = new SqlCommand("UPDATE [SWP].[dbo].[Task] SET[task_name] = @task_name, " +
                                                        "[task_price] = @task_price, [task_active] = @task_active, " +
                                                        "[Description] = @description " +
                                                        "WHERE[task_id] = @task_id", connection);
                    command.Parameters.AddWithValue("task_id", ntask.taskID);
                    command.Parameters.AddWithValue("task_name", ntask.taskName);
                    command.Parameters.AddWithValue("task_price", ntask.price);
                    command.Parameters.AddWithValue("task_active", ntask.active);
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
