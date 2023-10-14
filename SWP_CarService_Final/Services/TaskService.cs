using SWP_CarService_Final.Models;
using System.Data.SqlClient;
using SWP_CarService_Final.Models;

namespace SWP_CarService_Final.Services
{
    public class TaskService
    {
        private readonly DBContext _dbcontext;

        public TaskService(DBContext context)
        {
            _dbcontext = context;
        }

        public void GetListOfService(Appointment apppointment)
        {
            List<Models.Task> list = new List<Models.Task>();
            _dbcontext._connection().Open();
            SqlCommand command = new SqlCommand("SELECT * FROM [SWP].[dbo].[Task]", _dbcontext._connection());
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Models.Task task = new Models.Task { };
                    task.taskID = reader.GetString(0);
                    task.taskName = reader.GetString(1);
                    task.price = reader.GetDecimal(2);
                    task.actice = reader.GetBoolean(3);
                    task.img = (!reader.IsDBNull(4)) ? reader.GetString(4) : "";
                }
            }
            _dbcontext._connection().Close();

        }
    }
}
