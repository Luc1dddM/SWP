using System.Data.SqlClient;

namespace SWP_CarService_Final.Services
{
    public class DBContext
    {
        SqlConnection connection;

        public DBContext() { 
            connection = new SqlConnection();
            connection.ConnectionString = "Data Source=.;Initial Catalog=SWP;Integrated Security=True;Trusted_Connection=true";
        }
    }
}
