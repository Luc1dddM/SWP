using SWP_CarService_Final.Models;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;


namespace SWP_CarService_Final.Services
{

    public class AppointmentService : DBContext
    {

        private readonly TaskService _taskService;

        public AppointmentService(TaskService taskService)
        {
            _taskService = taskService;
        }
        public int getNumberOfAppointment()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("select count(*) from Appointment", connection);
            Int32 count = (Int32)command.ExecuteScalar();
            connection.Close();
            return count;
        }

        public void createAppointment(Appointment apppointment, List<string> detailIDs)
        {
            string id = "APM" + (getNumberOfAppointment() + 1);
            connection.Open();
            SqlCommand command = new SqlCommand("insert into Appointment(appointment_id, vehical_type, [description], time_arrived, created_at, [status], user_name) " +
                "values(@id, @type, @description, @arrived, @create, @status, @userName)", connection);
            command.Parameters.AddWithValue("id", id.Trim());
            command.Parameters.AddWithValue("type", apppointment.vehicalType);
            command.Parameters.AddWithValue("description", apppointment.description);
            command.Parameters.AddWithValue("arrived", apppointment.timeArrived);
            command.Parameters.AddWithValue("create", DateTime.Now);
            command.Parameters.AddWithValue("status", "pending");
            command.Parameters.AddWithValue("userName", apppointment.customer.user_name);
            command.ExecuteNonQuery();
            connection.Close();
            createAppointmentDetails(id, detailIDs);
        }

        public void createAppointmentDetails(string apmID, List<string> detailIDs)
        {
            foreach (string detail in detailIDs)
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("insert Appointment_Details values(@appointmentID, @taskID)", connection);
                    cmd.Parameters.AddWithValue("appointmentID", apmID);
                    cmd.Parameters.AddWithValue("taskID", detail);
                    cmd.ExecuteNonQuery();
                }
                finally { connection.Close(); }
            }
        }

        public List<AppointmentDetail> getAppointmentDetailByAPMID(string APMID)
        {
            List<AppointmentDetail> details = new List<AppointmentDetail>();
            try
            {

                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }


                SqlCommand cmd = new SqlCommand("Select * from Appointment_Details where appointment_id = @APMID", connection);
                cmd.Parameters.AddWithValue("APMID", APMID);
                using (SqlDataReader reader1 = cmd.ExecuteReader())
                {
                    while (reader1.Read())
                    {
                        var detail = new AppointmentDetail()
                        {
                            appointmentID = reader1.GetString(0),
                            task = _taskService.getTaskByIDForAppointment(reader1.GetString(1)),
                        };
                        details.Add(detail);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return details;
        }

        public List<Appointment> getAllApppointments(Customer cCustomer)
        {
            List<Appointment> appointments = new List<Appointment>();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("select Appointment.appointment_id, Appointment.vehical_type, Appointment.[description], " +
                    "Appointment.time_arrived, Appointment.created_at, Appointment.[status], Appointment.[user_name], Work_order.WorkOrder_id " +
                    "from Appointment " +
                    "left join Appointment_WorkOrder on Appointment.appointment_id = Appointment_WorkOrder.Appointment_ID " +
                    "left join Work_order on Appointment_WorkOrder.WorkOrder_ID = Work_order.WorkOrder_id " +
                    " where user_name = @userName", connection);
                cmd.Parameters.AddWithValue("userName", cCustomer.user_name);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var appointment = new Appointment()
                        {
                            appointmentID = reader.GetString(0),
                            vehicalType = reader.GetString(1),
                            description = reader.GetString(2),
                            timeArrived = reader.GetDateTime(3),
                            createdAt = reader.GetDateTime(4),
                            status = reader.GetString(5),
                            WorkOrderID = (!reader.IsDBNull(7)) ? reader.GetString(7) : "",
                            customer = cCustomer,
                            details = getAppointmentDetailByAPMID(reader.GetString(0)),
                        };
                        appointments.Add(appointment);
                    }
                }
            }catch( Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { connection.Close(); }
            return appointments;
        }

        public List<Appointment> getAllApppointments()
        {
            List<Appointment> appointments = new List<Appointment>();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("Select * from Appointment", connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var appointment = new Appointment()
                        {
                            appointmentID = reader.GetString(0),
                            vehicalType = reader.GetString(1),
                            description = reader.GetString(2),
                            timeArrived = reader.GetDateTime(3),
                            createdAt = reader.GetDateTime(4),
                            status = reader.GetString(5),
                            customer = new Customer() { fullName = "test" },
                            details = getAppointmentDetailByAPMID(reader.GetString(0)),
                        };
                        appointments.Add(appointment);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { connection.Close(); }
            return appointments;
        }

        public Appointment getAppointmentByID(string id)
        {
            var appointment = new Appointment();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("Select * from Appointment where appointment_id = @id", connection);
                cmd.Parameters.AddWithValue("id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                         appointment = new Appointment()
                        {
                            appointmentID = reader.GetString(0),
                            vehicalType = reader.GetString(1),
                            description = reader.GetString(2),
                            timeArrived = reader.GetDateTime(3),
                            createdAt = reader.GetDateTime(4),
                            status = reader.GetString(5),
                            customer = new Customer() { user_name = "hvuthai"},
                            details = getAppointmentDetailByAPMID(reader.GetString(0)),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { connection.Close(); }
            return appointment;
        }


        public void cancelAppointment(string id)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("update Appointment set status = 'Cancel' where appointment_id = @id", connection);
                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
            }catch( Exception ex )
            {
                throw new Exception(ex.Message);
            }finally { connection.Close(); }
        }
    }
}
