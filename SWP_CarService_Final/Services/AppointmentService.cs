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
            command.Parameters.AddWithValue("id", id);
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
                SqlCommand cmd = new SqlCommand("Select * from appointment_Detail", connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var detail = new AppointmentDetail()
                        {
                            appointmentID = reader.GetString(0),
                            task = _taskService.getTaskByIDForAppointment(reader.GetString(1)),
                        };
                        details.Add(detail);
                    }
                }
            }catch(Exception ex)
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
                SqlCommand cmd = new SqlCommand("Select * from Appointment where user_name = @userName", connection);
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
                            customer = cCustomer,
                            details = getAppointmentDetailByAPMID(reader.GetString(6)),
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
    }
}
