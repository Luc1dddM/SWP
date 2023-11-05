using Microsoft.SqlServer.Server;
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
        public string getLastId()
        {
            string id = null;
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT top 1 appointment_id FROM Appointment ORDER BY CAST(SUBSTRING(appointment_id, PATINDEX('%[0-9]%', appointment_id), LEN(appointment_id)) AS INT) desc", connection);
            using(SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    id = reader.GetString(0);
                }
            }
            connection.Close();
            return id;
        }

        public void createAppointment(Appointment apppointment, List<string> detailIDs)
        {
            string idFormat = "APM";
            try
            {
                string lastId = getLastId();
                string id;
                if(lastId != null)
                {
                    id = idFormat + (int.Parse(lastId.Substring(idFormat.Length)) + 1);
                }
                else
                {
                    id = idFormat + "1";
                }
                connection.Open();
                SqlCommand command = new SqlCommand("insert into Appointment(appointment_id, [description], time_arrived, created_at, [status], user_name) " +
                    "values(@id, @description, @arrived, @create, @status, @userName)", connection);
                command.Parameters.AddWithValue("id", id.Trim());
                command.Parameters.AddWithValue("description", apppointment.description);
                command.Parameters.AddWithValue("arrived", apppointment.timeArrived);
                command.Parameters.AddWithValue("create", DateTime.Now);
                command.Parameters.AddWithValue("status", "pending");
                command.Parameters.AddWithValue("userName", apppointment.customer.user_name);
                command.ExecuteNonQuery();
                createAppointmentDetails(id, detailIDs);
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }finally { connection.Close(); }
            
        }

        public void createAppointmentDetails(string apmID, List<string> detailIDs)
        {
            foreach (string detail in detailIDs)
            {
                try
                {
                    if (connection.State == System.Data.ConnectionState.Closed)
                    {
                        connection.Open();
                    }
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
                            task = _taskService.GetTaskByID(reader1.GetString(1).Trim()),
                        };
                        details.Add(detail);
                    }
                }
            }
            catch (Exception ex)
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM Appointment where Appointment.user_name = @username ORDER BY CAST(SUBSTRING(appointment_id, PATINDEX('%[0-9]%', appointment_id), LEN(appointment_id)) AS INT) desc ", connection);
                cmd.Parameters.AddWithValue("username", cCustomer.user_name);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var appointment = new Appointment()
                        {
                            appointmentID = reader.GetString(0),
                            description = reader.GetString(1),
                            timeArrived = reader.GetDateTime(2),
                            createdAt = reader.GetDateTime(3),
                            status = reader.GetString(4),
                            customer = cCustomer,
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

        public List<Appointment> getAllApppointments()
        {
            UserServices userService = new UserServices();
            List<Appointment> appointments = new List<Appointment>();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT  *  FROM Appointment ORDER BY CAST(SUBSTRING(appointment_id, PATINDEX('%[0-9]%', appointment_id), LEN(appointment_id)) AS INT) desc", connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var appointment = new Appointment()
                        {
                            appointmentID = reader.GetString(0),
                            description = reader.GetString(1),
                            timeArrived = reader.GetDateTime(2),
                            createdAt = reader.GetDateTime(3),
                            status = reader.GetString(4),
                            customer = userService.getCustomerByUserName(reader.GetString(5)),
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
            UserServices userService = new UserServices();
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
                            description = reader.GetString(1),
                            timeArrived = reader.GetDateTime(2),
                            createdAt = reader.GetDateTime(3),
                            status = reader.GetString(4),
                            customer = userService.getCustomerByUserName(reader.GetString(5)),
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


        public void updateStatus(string id, string status)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("update Appointment set status = @status where appointment_id = @id", connection);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("status", status);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { connection.Close(); }
        }


    }
}
