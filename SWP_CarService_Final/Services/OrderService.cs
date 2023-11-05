using SWP_CarService_Final.Models;
using System.Data.SqlClient;

namespace SWP_CarService_Final.Services
{
    public class OrderService : DBContext
    {
        private readonly TaskDetailService _taskDetailService;
        private readonly AppointmentService _appointmentService;
        private readonly PartDetailService  _partDetailService;
        private readonly UserServices _userService;
        public OrderService()
        {
            TaskService taskService = new TaskService();
            _taskDetailService= new TaskDetailService();
            _appointmentService= new AppointmentService(taskService);
            _partDetailService = new PartDetailService();
            _userService = new UserServices();
        }
        public int getNumberOfWorkOrder()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("select count(*) from Work_order", connection);
            Int32 count = (Int32)command.ExecuteScalar();
            connection.Close();
            return count;
        }

        public List<WorkOrder> getAllWorkOrders()
        {
            List<WorkOrder> workOrders = new List<WorkOrder>();
       
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("Select * from Work_order", connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        workOrders.Add(new WorkOrder()
                        {
                            WorkOrderID = reader.GetString(0),
                            VehicleType = reader.GetString(1),
                            Total = reader.GetDecimal(2),
                            CustomerName = reader.GetString(3),
                            CreatedBy = reader.GetString(4),
                            createdAt = reader.GetDateTime(5),
                            customer = _userService.getCustomerByUserName(reader.GetString(3)),
                            /*taskDetails = _taskDetailService.GetTaskDetailsByWOID(reader.GetString(0)),*/
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }finally { connection.Close(); }
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
                SqlCommand cmd = new SqlCommand("Select * from Work_order where [user] = @CreatedBy", connection);
                cmd.Parameters.AddWithValue("CreatedBy", createdBy);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        workOrders.Add(new WorkOrder()
                        {
                            WorkOrderID = reader.GetString(0),
                            VehicleType = reader.GetString(1),
                            Total = reader.GetDecimal(2),
                            CustomerName = reader.GetString(3),
                            CreatedBy = reader.GetString(4),
                            createdAt = reader.GetDateTime(5),
                            customer = _userService.getCustomerByUserName(reader.GetString(3)),
                            /*taskDetails = _taskDetailService.GetTaskDetailsByWOID(reader.GetString(0)),*/
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }finally { connection.Close(); }
            return workOrders;
        }

        public List<WorkOrder> getAllWorkOrdersOfOwner(string owner)
        {
            List<WorkOrder> workOrders = new List<WorkOrder>();
      

            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("Select * from Work_order where customer = @owner", connection);
                cmd.Parameters.AddWithValue("owner", owner);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        workOrders.Add(new WorkOrder()
                        {
                            WorkOrderID = reader.GetString(0),
                            VehicleType = reader.GetString(1),
                            Total = reader.GetDecimal(2),
                            CustomerName = owner,
                            CreatedBy = reader.GetString(4),
                            createdAt = reader.GetDateTime(5),
                            customer = _userService.getCustomerByUserName(owner),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }finally { connection.Close(); }
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
                            VehicleType = reader.GetString(1),
                            Total = reader.GetDecimal(2),
                            CustomerName = reader.GetString(3),
                            CreatedBy = reader.GetString(4),
                            createdAt = reader.GetDateTime(5),
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

        public void createWorkOrderByAPM(Appointment appointment, string createdBy)
        {
            string id = "WOD" + (getNumberOfWorkOrder() + 1);
            try
            {
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
                        VehicleType = appointment.vehicalType,
                        Total = 0,
                        CustomerName = appointment.customer.user_name,
                        CreatedBy = createdBy,
                        createdAt = DateTime.Now,
                    };
                    SqlCommand cmd = new SqlCommand("Insert into Work_order values(@id, @vehicleType, @total, @customerName, @CreatedBy, current_timestamp)", connection);
                    cmd.Parameters.AddWithValue("id", workOrder.WorkOrderID);
                    cmd.Parameters.AddWithValue("vehicleType", workOrder.VehicleType);
                    cmd.Parameters.AddWithValue("total", workOrder.Total);
                    cmd.Parameters.AddWithValue("customerName", workOrder.CustomerName);
                    cmd.Parameters.AddWithValue("CreatedBy", workOrder.CreatedBy);
                    cmd.ExecuteNonQuery();
                    TaskDetail detail;
                    foreach (var taskDetail in appointment.details)
                    {
                        detail = new TaskDetail()
                        {
                            quantity = 0,
                            price = taskDetail.task.price,
                            task = taskDetail.task,
                            status = "Process",
                            createdAt = DateTime.Now,
                            updatedAt = DateTime.Now,
                            userName = createdBy,
                            WorkOrder = workOrder,
                        };
                        _taskDetailService.createTaskDetail(detail);
                    }
                    createOrderAppointment(appointment.appointmentID, id);
                    _appointmentService.updateStatus(appointment.appointmentID, "Done");
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
        }

        public void createOrderAppointment(string appointmentId, string workOrderId)
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("insert into Appointment_WorkOrder values(@apmId, @WOID)", connection);
                cmd.Parameters.AddWithValue("apmId", appointmentId);
                cmd.Parameters.AddWithValue("WOID", workOrderId);
                cmd.ExecuteNonQuery();
            }catch (Exception ex) { throw new Exception(ex.Message); }
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
            }catch(Exception ex) { throw new Exception(ex.Message);
            }finally { connection.Close(); }
        }

        public string getCurrentWorkOrderId(string UserName)
        {
            string OrderId = null;
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("select task_detail.WorkOrder_id from task_detail where task_detail.[status] = 'Process' and task_detail.userName = @UserName", connection);
                cmd.Parameters.AddWithValue("UserName", UserName);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        OrderId = reader.GetString(0);
                    }
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return OrderId;
        }

    }
}
