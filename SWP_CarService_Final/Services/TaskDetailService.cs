using SWP_CarService_Final.Models;
using System.Data.SqlClient;

namespace SWP_CarService_Final.Services
{
    public class TaskDetailService : DBContext
    {
        public List<TaskDetail> GetTaskDetailsByWOID(string woID)
        {
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
                using(SqlDataReader reader  = cmd.ExecuteReader())
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
                            WorkOrderID = woID,
                        };
                        result.Add(taskDetail);
                    }
                }
            }catch(Exception ex) {
                throw new Exception(ex.Message);
            }finally { connection.Close(); }
            return result;
        }

        public TaskDetail getTaskDetailByID(string wodID)
        {
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
                            WorkOrderID = reader.GetString(8),
                        };
                    }
                }
            }
            catch(Exception ex) { throw new Exception(ex.Message, ex); } 
            finally { connection.Close(); };
            return taskDetail;
        }
    }
}
