using System.Data;
using System.Data.SqlClient;

namespace SWP_CarService_Final.Services
{
    public class AppointmentService
    {
        public int getNumberOfAppointment()
        {
            DBContext context = new DBContext();
            context._connection().Open();
            SqlCommand command = new SqlCommand("select * from Appointment", context._connection());
            using(SqlDataReader reader = command.ExecuteReader())
            {
                DataTable schemaTable = reader.GetSchemaTable();

                if (schemaTable != null)
                {
                    return  schemaTable.Rows.Count;
                }
                else
                {
                    return 0;
                }
            }
        }


    }
}
