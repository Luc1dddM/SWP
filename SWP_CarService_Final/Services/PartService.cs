using System.Data.SqlClient;
using SWP_CarService_Final.Models;

namespace SWP_CarService_Final.Services
{
    public class PartService : DBContext
    {
        public List<Part> GetAllPart(int pageNumber)
        {
            List<Part> parts = new List<Part>();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM [SWP].[dbo].[Part] ORDER BY part_id OFFSET @pagenumber ROWS FETCH NEXT 10 ROWS ONLY ", connection);
                /*ORDER BY part_id OFFSET @pagenumber ROWS FETCH NEXT 10 ROWS ONLY;*/
                cmd.Parameters.AddWithValue("pagenumber", ((pageNumber-1)*10));
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var part = new Part()
                        {
                            part_id = reader.GetString(0),
                            part_name = reader.GetString(1),
                            price = reader.GetDecimal(2),
                            quantity = reader.GetInt32(3),
                            img = (!reader.IsDBNull(4)) ? reader.GetString(4) : null
                        };
                        parts.Add(part);
                    }
                }
            }
            finally { connection.Close(); }
            return parts;
        }

        public int GetNumberOfPage()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("select count(*) from Part", connection);
            Int32 count = (Int32)command.ExecuteScalar()%10 ==0? (Int32)command.ExecuteScalar()/10 : (Int32)command.ExecuteScalar() / 10 +1;
            connection.Close();
            return count;
        }

        
    }
}
