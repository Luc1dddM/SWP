using Microsoft.Extensions.Primitives;
using NuGet.Protocol.Plugins;
using SWP_CarService_Final.Models;
using System.Data.SqlClient;

namespace SWP_CarService_Final.Services
{
    public class TaskDetailService : DBContext
    {

        public string getTheLastID()
        {
            string result = null;
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM task_detail ORDER BY wod_id DESC;", connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = reader.GetString(0);
                    }
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
            return result;
        }
        public List<TaskDetail> GetTaskDetailsByWOID(string woID)
        {
            OrderService orderService = new OrderService();
            TaskService taskService = new TaskService();
            List<TaskDetail> result = new List<TaskDetail>();
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("select * from task_detail where WorkOrder_id = @woID", connection);
                cmd.Parameters.AddWithValue("woID", woID);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TaskDetail taskDetail = new TaskDetail()
                        {
                            wod_id = reader.GetString(0),
                            quantity = reader.GetInt32(1),
                            price = reader.GetDecimal(2),
                            status = reader.GetString(3),
                            createdAt = reader.GetDateTime(4),
                            updatedAt = reader.GetDateTime(5),
                            userName = reader.GetString(6),
                            task = taskService.GetTaskByID(reader.GetString(7)),
                        };
                        result.Add(taskDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { connection.Close(); }
            return result;
        }

        public TaskDetail getTaskDetailByID(string wodID)
        {
            OrderService orderService = new OrderService();
            TaskService taskService = new TaskService();
            TaskDetail taskDetail = null;
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("select * from task_detail where wod_id = @wod_id", connection);
                cmd.Parameters.AddWithValue("wod_id", wodID);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        taskDetail = new TaskDetail()
                        {
                            wod_id = reader.GetString(0),
                            quantity = reader.GetInt32(1),
                            price = reader.GetDecimal(2),
                            status = reader.GetString(3),
                            createdAt = reader.GetDateTime(4),
                            updatedAt = reader.GetDateTime(5),
                            userName = reader.GetString(6),
                            task = taskService.GetTaskByID(reader.GetString(7)),
                            WorkOrder = orderService.getWorkOrderById(reader.GetString(8)),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { connection.Close(); }
            return taskDetail;
        }

        public List<TaskDetail> getRequestCompleteList(string UserName)
        {
            OrderService orderService = new OrderService();
            TaskService taskService = new TaskService();
            List<TaskDetail> taskDetails = new List<TaskDetail>();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("select * from task_detail where task_detail.userName in (select [user].UserName from [user] join Team_Members on [user].UserName = Team_Members.userName where Team_Members.team_id = " +
                    "(select team_id from Team_Members join [user] on [user].UserName = Team_Members.userName where [User].UserName = @UserName)) " +
                    "and task_detail.status = 'Request Complete'", connection);
                cmd.Parameters.AddWithValue("UserName", UserName);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        taskDetails.Add(new TaskDetail()
                        {
                            wod_id = reader.GetString(0),
                            quantity = reader.GetInt32(1),
                            price = reader.GetDecimal(2),
                            status = reader.GetString(3),
                            createdAt = reader.GetDateTime(4),
                            updatedAt = reader.GetDateTime(5),
                            userName = reader.GetString(6),
                            task = taskService.GetTaskByID(reader.GetString(7)),
                            WorkOrder = orderService.getWorkOrderById(reader.GetString(8)),
                        });
                    }
                }
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return taskDetails;
        }

        public void createTaskDetail(TaskDetail taskDetail)
        {

            try
            {
                string IdFormat = "TD";
                string id;
                if (getTheLastID() != null)
                {
                    id = IdFormat + (int.Parse(getTheLastID().Trim().Substring(IdFormat.Length)) + 1);
                }
                else
                {
                    id = IdFormat + 1;
                }
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("Insert task_detail values(@id, @quantity, @amount, @status, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, @UserName, @task, @WorkOrder)", connection);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("quantity", taskDetail.quantity);
                cmd.Parameters.AddWithValue("amount", taskDetail.price);
                cmd.Parameters.AddWithValue("status", taskDetail.status);
                cmd.Parameters.AddWithValue("UserName", taskDetail.userName);
                cmd.Parameters.AddWithValue("task", taskDetail.task.taskID);
                cmd.Parameters.AddWithValue("WorkOrder", taskDetail.WorkOrder.WorkOrderID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { connection.Close(); }

        }

        public void updateTaskDetail(TaskDetail taskDetail)
        {
            try
            {
                if (getTaskDetailByID(taskDetail.wod_id) != null)
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("update task_detail set quantity = @quantity, amount = @amount, " +
                        "[status] = @status, created = @created, updated = CURRENT_TIMESTAMP, userName =  @UserName where wod_id = @wod_id", connection);
                    cmd.Parameters.AddWithValue("quantity", taskDetail.quantity);
                    cmd.Parameters.AddWithValue("amount", taskDetail.price);
                    cmd.Parameters.AddWithValue("status", taskDetail.status);
                    cmd.Parameters.AddWithValue("created", taskDetail.createdAt);
                    cmd.Parameters.AddWithValue("UserName", taskDetail.userName);
                    cmd.Parameters.AddWithValue("wod_id", taskDetail.wod_id);
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { connection.Close(); }
        }

        public List<TaskDetail> GetTaskDetailsOfMember(string userName)
        {
            OrderService orderService = new OrderService();
            TaskService taskService = new TaskService();
            List<TaskDetail> taskDetails = new List<TaskDetail>();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("select * from task_detail where userName = @UserName ", connection);
                cmd.Parameters.AddWithValue("@UserName", userName);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        taskDetails.Add(new TaskDetail()
                        {
                            wod_id = reader.GetString(0),
                            quantity = reader.GetInt32(1),
                            price = reader.GetDecimal(2),
                            status = reader.GetString(3),
                            createdAt = reader.GetDateTime(4),
                            updatedAt = reader.GetDateTime(5),
                            userName = reader.GetString(6),
                            task = taskService.GetTaskByID(reader.GetString(7)),
                            WorkOrder = orderService.getWorkOrderById(reader.GetString(8)),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { connection.Close(); }
            return taskDetails;
        }
    }
}
