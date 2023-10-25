﻿using SWP_CarService_Final.Models;
using System.Data.SqlClient;

namespace SWP_CarService_Final.Services
{
    public class OrderService : DBContext
    {

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
            TaskDetailService TDservice = new TaskDetailService();
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
                            taskDetails = TDservice.GetTaskDetailsByWOID(reader.GetString(0)),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return workOrders;
        }

        public List<WorkOrder> getAllWorkOrdersCreatedByUser(string createdBy)
        {
            List<WorkOrder> workOrders = new List<WorkOrder>();
            TaskDetailService TDservice = new TaskDetailService();

            try
            {
                connection.Open();
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
                            taskDetails = TDservice.GetTaskDetailsByWOID(reader.GetString(0)),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return workOrders;
        }

        public List<WorkOrder> getAllWorkOrdersOfOwner(string owner)
        {
            List<WorkOrder> workOrders = new List<WorkOrder>();
            TaskDetailService TDservice = new TaskDetailService();

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
                            CustomerName = reader.GetString(3),
                            CreatedBy = reader.GetString(4),
                            createdAt = reader.GetDateTime(5),
                            taskDetails = TDservice.GetTaskDetailsByWOID(reader.GetString(0)),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return workOrders;
        }


        public WorkOrder getWorkOrderById(string id)
        {
            WorkOrder WorkOrder = null;
            TaskDetailService TDservice = new TaskDetailService();

            try
            {
                connection.Open();
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
                            taskDetails = TDservice.GetTaskDetailsByWOID(reader.GetString(0)),
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
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("Insert into Work_order values(@id, @vehicleType, @total, @customerName, @CreatedBy, current_timestamp)", connection);
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.AddWithValue("vehicleType", appointment.vehicalType);
                    cmd.Parameters.AddWithValue("total", 0);
                    cmd.Parameters.AddWithValue("customerName", appointment.customer.user_name);
                    cmd.Parameters.AddWithValue("CreatedBy", createdBy);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
        }
    }
}
