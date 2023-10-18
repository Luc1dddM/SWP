using SWP_CarService_Final.Models;
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
                            CustomerName = reader.GetString(2),
                            CreatedBy = reader.GetString(3),
                            createdAt = reader.GetDateTime(4)
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

        public WorkOrder getAllWorkOrderById(string id)
        {
            WorkOrder WorkOrder = null;
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
                            CustomerName = reader.GetString(2),
                            CreatedBy = reader.GetString(3),
                            createdAt = reader.GetDateTime(4)
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
