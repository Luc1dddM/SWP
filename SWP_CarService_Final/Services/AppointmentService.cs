using SWP_CarService_Final.Models;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;


namespace SWP_CarService_Final.Services
{

    public class AppointmentService
    {
        private readonly DBContext _dbcontext;

        public AppointmentService(DBContext context)
        {
            _dbcontext = context;
        }
        public int getNumberOfAppointment()
        {
            _dbcontext._connection().Open();
            SqlCommand command = new SqlCommand("select count(*) from Appointment", _dbcontext._connection());
            Int32 count = (Int32)command.ExecuteScalar();
            _dbcontext._connection().Close();
            return count;
        }

        public void createAppointment(Appointment apppointment, List<string> detailIDs)
        {
            string id = "APM" + (getNumberOfAppointment() + 1);
            _dbcontext._connection().Open();
            SqlCommand command = new SqlCommand("insert into Appointment(appointment_id, vehical_type, [description], time_arrived, created_at, [status], user_name) " +
                "values(@id, @type, @description, @arrived, @create, @status, @userName)", _dbcontext._connection());
            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("type", apppointment.vehicalType);
            command.Parameters.AddWithValue("description", apppointment.description);
            command.Parameters.AddWithValue("arrived", apppointment.timeArrived);
            command.Parameters.AddWithValue("create", DateTime.Now);
            command.Parameters.AddWithValue("status", "pending");
            command.Parameters.AddWithValue("userName", apppointment.customer.user_name);
            command.ExecuteNonQuery();
            _dbcontext._connection().Close();
            createAppointmentDetails(id, detailIDs);
        }

        public void createAppointmentDetails(string apmID, List<string> detailIDs)
        {
            foreach (string detail in detailIDs)
            {
                try
                {
                    _dbcontext._connection().Open();
                    SqlCommand cmd = new SqlCommand("insert Appointment_Detail values(@appointmentID, @taskID)", _dbcontext._connection());
                    cmd.Parameters.AddWithValue("appointmentID", apmID);
                    cmd.Parameters.AddWithValue("taskID", detail);
                    cmd.ExecuteNonQuery();
                }finally { _dbcontext._connection().Close(); }
            }
        }
    }
}
