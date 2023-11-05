using Humanizer;
using NuGet.Protocol.Plugins;
using SWP_CarService_Final.Areas.User.Models;
using SWP_CarService_Final.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SWP_CarService_Final.Services
{
    public class OrderService : DBContext
    {
        private readonly TaskDetailService _taskDetailService;
        private readonly AppointmentService _appointmentService;
        private readonly PartDetailService _partDetailService;
        private readonly UserServices _userService;
        public OrderService()
        {
            TaskService taskService = new TaskService();
            _taskDetailService = new TaskDetailService();
            _appointmentService = new AppointmentService(taskService);
            _partDetailService = new PartDetailService();
            _userService = new UserServices();
        }
        public string getTheLastId()
        {
            string id = null;
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT top 1 Work_order.WorkOrder_id FROM Work_order ORDER BY CAST(SUBSTRING(WorkOrder_id, PATINDEX('%[0-9]%', WorkOrder_id), LEN(WorkOrder_id)) AS INT) desc", connection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    id = reader.GetString(0);
                }
            }
            connection.Close();
            return id;
        }

        public List<WorkOrder> getAllWorkOrders()
        {
            List<WorkOrder> workOrders = new List<WorkOrder>();

            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Work_order ORDER BY CAST(SUBSTRING(WorkOrder_id, PATINDEX('%[0-9]%', WorkOrder_id), LEN(WorkOrder_id)) AS INT) desc", connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        workOrders.Add(new WorkOrder()
                        {
                            WorkOrderID = reader.GetString(0),
                            brand = reader.GetString(1),
                            Total = reader.GetDecimal(2),
                            CustomerName = reader.GetString(3),
                            CreatedBy = reader.GetString(4),
                            createdAt = reader.GetDateTime(5),
                            customer = _userService.getCustomerByUserName(reader.GetString(3)),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { connection.Close(); }
            return workOrders;
        }

        public List<WorkOrder> getAllWorkOrdersCreatedByUser(string createdBy)
        {
            List<WorkOrder> workOrders = new List<WorkOrder>();
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("SELECT * FROM Work_order where [user] = @CreatedBy ORDER BY CAST(SUBSTRING(WorkOrder_id, PATINDEX('%[0-9]%', WorkOrder_id), LEN(WorkOrder_id)) AS INT) desc", connection);
                cmd.Parameters.AddWithValue("CreatedBy", createdBy);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        workOrders.Add(new WorkOrder()
                        {
                            WorkOrderID = reader.GetString(0),
                            brand = reader.GetString(1),
                            model = reader.GetString(2),
                            YoM = reader.GetInt32(3),
                            Total = reader.GetDecimal(4),
                            CustomerName = reader.GetString(5),
                            CreatedBy = reader.GetString(6),
                            createdAt = reader.GetDateTime(7),
                            customer = _userService.getCustomerByUserName(reader.GetString(5)),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { connection.Close(); }
            return workOrders;
        }

        public List<WorkOrder> getAllWorkOrdersOfOwner(string owner)
        {
            List<WorkOrder> workOrders = new List<WorkOrder>();


            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Work_order where customer = @owner ORDER BY CAST(SUBSTRING(WorkOrder_id, PATINDEX('%[0-9]%', WorkOrder_id), LEN(WorkOrder_id)) AS INT) desc", connection);
                cmd.Parameters.AddWithValue("owner", owner);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        workOrders.Add(new WorkOrder()
                        {
                            WorkOrderID = reader.GetString(0),
                            brand = reader.GetString(1),
                            model = reader.GetString(2),
                            YoM = reader.GetInt32(3),
                            Total = reader.GetDecimal(4),
                            CustomerName = owner,
                            CreatedBy = reader.GetString(6),
                            createdAt = reader.GetDateTime(7),
                            customer = _userService.getCustomerByUserName(owner),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { connection.Close(); }
            return workOrders;
        }

        public WorkOrder getWorkOrderById(string id)
        {
            WorkOrder WorkOrder = null;
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("Select * from Work_order where WorkOrder_id = @id", connection);
                cmd.Parameters.AddWithValue("id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        WorkOrder = new WorkOrder()
                        {
                            WorkOrderID = reader.GetString(0),
                            brand = reader.GetString(1),
                            model = reader.GetString(2),
                            YoM = reader.GetInt32(3),
                            Total = reader.GetDecimal(4),
                            CustomerName = reader.GetString(5),
                            CreatedBy = reader.GetString(6),
                            createdAt = reader.GetDateTime(7),
                            customer = _userService.getCustomerByUserName(reader.GetString(5)),
                            taskDetails = _taskDetailService.GetTaskDetailsByWOID(reader.GetString(0)),
                            partDetails = _partDetailService.GetPartDetailsByOrderID(reader.GetString(0)),
                        };
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
            return WorkOrder;
        }

        public WorkOrder getWorkOrderById(string id, User user)
        {
            WorkOrder WorkOrder = null;
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("Select * from Work_order where WorkOrder_id = @id", connection);
                cmd.Parameters.AddWithValue("id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        WorkOrder = new WorkOrder()
                        {
                            WorkOrderID = reader.GetString(0),
                            brand = reader.GetString(1),
                            model = reader.GetString(2),
                            YoM = reader.GetInt32(3),
                            Total = reader.GetDecimal(4),
                            CustomerName = reader.GetString(5),
                            CreatedBy = reader.GetString(6),
                            createdAt = reader.GetDateTime(7),
                            customer = _userService.getCustomerByUserName(reader.GetString(5)),
                            taskDetails = _taskDetailService.GetTaskDetailsByWOID(reader.GetString(0), user.UserName),
                            partDetails = _partDetailService.GetPartDetailsByOrderID(reader.GetString(0), user.UserName),
                        };
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
            return WorkOrder;
        }

        public void createWorkOrderByAPM(Appointment appointment, string createdBy, string Brand, string Model, int YoM)
        {
            string idFormat = "WOD";
            string id;

            try
            {
                string lastId = getTheLastId();
                if (lastId != null)
                {
                    id = idFormat + (int.Parse(lastId.Substring(idFormat.Length)) + 1);
                }
                else
                {
                    id = idFormat + "1";
                }
                if (appointment == null || appointment.status.Equals("Cancel"))
                {
                    throw new Exception("Appointmet was canceled or deleted!");
                }
                else
                {
                    if (connection.State == System.Data.ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    WorkOrder workOrder = new WorkOrder()
                    {
                        WorkOrderID = id,
                        brand = Brand,
                        model = Model,
                        YoM = YoM,
                        Total = 0,
                        CustomerName = appointment.customer.user_name,
                        CreatedBy = createdBy,
                        createdAt = DateTime.Now,
                    };
                    SqlCommand cmd = new SqlCommand("Insert into Work_order values(@id, @Brand, @Model, @YoManufacture, @total, @customerName, @CreatedBy, current_timestamp)", connection);
                    cmd.Parameters.AddWithValue("id", workOrder.WorkOrderID);
                    cmd.Parameters.AddWithValue("Brand", workOrder.brand);
                    cmd.Parameters.AddWithValue("Model", workOrder.model);
                    cmd.Parameters.AddWithValue("YoManufacture", workOrder.YoM);
                    cmd.Parameters.AddWithValue("total", workOrder.Total);
                    cmd.Parameters.AddWithValue("customerName", workOrder.CustomerName);
                    cmd.Parameters.AddWithValue("CreatedBy", workOrder.CreatedBy);
                    cmd.ExecuteNonQuery();
                    TaskDetail detail;
                    foreach (var taskDetail in appointment.details)
                    {
                        detail = new TaskDetail()
                        {
                            quantity = 1,
                            price = taskDetail.task.price,
                            taskId = taskDetail.task.taskID,
                            status = "Process",
                            description = "",
                            createdAt = DateTime.Now,
                            updatedAt = DateTime.Now,
                            userName = createdBy,
                            WorkOrder = workOrder,
                        };
                        _taskDetailService.createTaskDetail(detail);
                    }
                    _appointmentService.updateStatus(appointment.appointmentID, "Done");
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
        }


        public void updateTotalWordOrder(string workOrderID)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("Update Work_order set Total = ( SELECT COALESCE(SUM(quantity * amount), 0) FROM task_detail WHERE WorkOrder_id = @WorkOrderID) " +
                    "+ (SELECT COALESCE(SUM(quantity * price), 0) FROM part_detail WHERE WorkOrder_id = @WorkOrderID and part_detail.[status] = 'Accepted') where WorkOrder_id = @WorkOrderID", connection);
                cmd.Parameters.AddWithValue("WorkOrderID", workOrderID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { connection.Close(); }
        }

        public List<WorkOrder> getWorkOrderIdMemberWorkIn(string UserName)
        {
            List<WorkOrder> result = new List<WorkOrder>();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("select * from Work_order where WorkOrder_id in (select task_detail.WorkOrder_id from task_detail where task_detail.userName = @UserName)", connection);
                cmd.Parameters.AddWithValue("UserName", UserName);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        WorkOrder workOrder = new WorkOrder()
                        {
                            WorkOrderID = reader.GetString(0),
                            brand = reader.GetString(1),
                            model = reader.GetString(2),
                            YoM = reader.GetInt32(3),
                            Total = reader.GetDecimal(4),
                            CustomerName = reader.GetString(5),
                            CreatedBy = reader.GetString(6),
                            createdAt = reader.GetDateTime(7),
                            customer = _userService.getCustomerByUserName(reader.GetString(5)),
                        };
                        result.Add(workOrder);
                    }
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally
            {
                connection.Close();
            }
            return result;
        }

        public bool checkWorkIn(string userName, string wodId)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("select * from task_detail where task_detail.[status] = 'Process' " +
                    "and task_detail.WorkOrder_id = @id and task_detail.userName = @userName", connection);
                cmd.Parameters.AddWithValue("id", wodId);
                cmd.Parameters.AddWithValue("userName", userName);
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return true;
                    }
                }
            }catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
            return false;
        }   
    }


}
