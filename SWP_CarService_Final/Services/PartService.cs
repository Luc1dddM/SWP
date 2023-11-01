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
                cmd.Parameters.AddWithValue("pagenumber", ((pageNumber - 1) * 10));
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


        public List<Part> GetAllPartFilter(int pageNumber, List<string> filterString, string StartPrice, string EndPrice, string searchText)
        {
            List<Part> parts = new List<Part>();
            string filter = null;
            try
            {
                if (filterString != null)
                {
                    foreach (var i in filterString)
                    {
                        filter += " AND c.part_id IN (SELECT part_id FROM [SWP].[dbo].[Part_category]WHERE category_id = '" + i.Trim() + "')";
                    }
                }
                if(StartPrice != null && EndPrice != null)
                {
                    filter += " and c.price between "+StartPrice+" and " + EndPrice;
                }
                if (searchText != null)
                {
                    filter += " and c.part_name like '%" + searchText + "%' ";
                }
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                if (filter != null)
                {
                    cmd = new SqlCommand("SELECT c.part_id, c.part_name,c.price, c.Quantity, c.img " +
                    "FROM [SWP].[dbo].[Part_category] a " +
                    "join [SWP].[dbo].[Part] c on c.part_id = a.part_id " +
                    "WHERE 1=1" + filter +
                    "group by c.part_id, c.part_name,c.price, c.Quantity, c.img " +
                    "ORDER BY CAST(SUBSTRING(c.part_id, PATINDEX('%[0-9]%', c.part_id), LEN(c.part_id)) AS INT) desc " +
                    "OFFSET @pagenumber ROWS FETCH NEXT 10 ROWS ONLY ", connection);
                }
                else
                {
                    cmd = new SqlCommand("SELECT c.part_id, c.part_name,c.price, c.Quantity, c.img " +
                    "FROM [SWP].[dbo].[Part_category] a " +
                    "join [SWP].[dbo].[Part] c on c.part_id = a.part_id " +
                    "WHERE 1=1 " +
                    "group by c.part_id, c.part_name,c.price, c.Quantity, c.img " +
                    "ORDER BY CAST(SUBSTRING(c.part_id, PATINDEX('%[0-9]%', c.part_id), LEN(c.part_id)) AS INT) desc " +
                    "OFFSET @pagenumber ROWS FETCH NEXT 10 ROWS ONLY ", connection);
                }

                cmd.Parameters.AddWithValue("pagenumber", ((pageNumber - 1) * 10));
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

        public List<Part> GetAllPartFilterRaw(int pageNumber, List<string> filterString, string StartPrice, string EndPrice, string searchText)
        {
            List<Part> parts = new List<Part>();
            string filter = null;
            try
            {
                if (filterString != null)
                {
                    foreach (var i in filterString)
                    {
                        filter += " AND c.part_id IN (SELECT part_id FROM [SWP].[dbo].[Part_category]WHERE category_id = '" + i.Trim() + "')";
                    }
                }
                if (StartPrice != null && EndPrice != null)
                {
                    filter += " and c.price between " + StartPrice + " and " + EndPrice;
                }
                if(searchText != null)
                {
                    filter += " and c.part_name like '%" + searchText + "%' ";
                }
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                if (filter != null)
                {
                    cmd = new SqlCommand("SELECT c.part_id, c.part_name,c.price, c.Quantity, c.img " +
                    "FROM [SWP].[dbo].[Part_category] a " +
                    "join [SWP].[dbo].[Part] c on c.part_id = a.part_id " +
                    "WHERE 1=1" + filter +
                    "group by c.part_id, c.part_name,c.price, c.Quantity, c.img " +
                    "ORDER BY CAST(SUBSTRING(c.part_id, PATINDEX('%[0-9]%', c.part_id), LEN(c.part_id)) AS INT) desc ", connection);
                }
                else
                {
                    cmd = new SqlCommand("SELECT c.part_id, c.part_name,c.price, c.Quantity, c.img " +
                    "FROM [SWP].[dbo].[Part_category] a " +
                    "join [SWP].[dbo].[Part] c on c.part_id = a.part_id " +
                    "WHERE 1=1 " +
                    "group by c.part_id, c.part_name,c.price, c.Quantity, c.img " +
                    "ORDER BY CAST(SUBSTRING(c.part_id, PATINDEX('%[0-9]%', c.part_id), LEN(c.part_id)) AS INT) desc " 
                    , connection);
                }


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

        public int GetNumberOfPage(List<Part> listPart)
        {
            Int32 count = listPart.Count%10 == 0? listPart.Count / 10 : (listPart.Count / 10) +1;
            return count;
        }

        public int getNumberOfPart()
        {
            Part part = null;
            try
            {
                connection.Open();
                string SQLSelect = "SELECT top 1 * FROM Part ORDER BY CAST(SUBSTRING(part_id, PATINDEX('%[0-9]%', part_id), LEN(part_id)) AS INT) desc";
                SqlCommand command = new SqlCommand(SQLSelect, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        part = new Part()
                        {
                            part_id = reader.GetString(0),
                            part_name = reader.GetString(1),
                            price = reader.GetDecimal(2),
                            quantity = reader.GetInt32(3),
                            img = (!reader.IsDBNull(4)) ? reader.GetString(4) : null
                        };
                    }
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally
            {
                connection.Close();
            }
            return part == null ? 0 : int.Parse(part.part_id.Substring(2));
        }

        public Part GetPartByID(string partID)
        {
            Part part = null;
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                string SQLSelect = "SELECT * FROM [SWP].[dbo].[Part] WHERE part_id = @part_id";
                SqlCommand command = new SqlCommand(SQLSelect, connection);
                command.Parameters.AddWithValue("part_id", partID);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        part = new Part()
                        {
                            part_id = reader.GetString(0),
                            part_name = reader.GetString(1),
                            price = reader.GetDecimal(2),
                            quantity = reader.GetInt32(3),
                            img = (!reader.IsDBNull(4)) ? reader.GetString(4) : null
                        };
                    }
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally
            {
                connection.Close();
            }
            return part;
        }

        public void createPart(Part nPart, List<string> categories)
        {

            try
            {
                string id = "PT" + (getNumberOfPart() + 1);
                connection.Open();
                if (nPart.img != null)
                {
                    SqlCommand command = new SqlCommand("INSERT INTO[SWP].[dbo].[Part] ([part_id],[part_name],[price],[quantity],[img]) " +
                                                        "values(@part_id,@part_name, @price, @quantity, @img)", connection);
                    command.Parameters.AddWithValue("part_id", id);
                    command.Parameters.AddWithValue("part_name", nPart.part_name);
                    command.Parameters.AddWithValue("price", nPart.price);
                    command.Parameters.AddWithValue("quantity", nPart.quantity);
                    command.Parameters.AddWithValue("img", nPart.price);
                    command.ExecuteNonQuery();
                }
                else
                {
                    SqlCommand command = new SqlCommand("INSERT INTO[SWP].[dbo].[Part] ([part_id],[part_name],[price],[quantity]) " +
                                                        "values(@part_id,@part_name, @price, @quantity)", connection);
                    command.Parameters.AddWithValue("part_id", id);
                    command.Parameters.AddWithValue("part_name", nPart.part_name);
                    command.Parameters.AddWithValue("price", nPart.price);
                    command.Parameters.AddWithValue("quantity", nPart.quantity);
                    command.ExecuteNonQuery();
                }

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
        }

        public void createPartCategory(Part nPart, List<string> categories)
        {

            try
            {
                connection.Open();
                if (nPart.img != null)
                {
                    SqlCommand command = new SqlCommand("INSERT INTO[SWP].[dbo].[Part_category] ([part_id],[category_id]) " +
                                                        "values(@part_id,@category_id)", connection);
                    command.Parameters.AddWithValue("part_id", nPart.part_id);
                    command.Parameters.AddWithValue("part_name", nPart.part_name);
                    command.ExecuteNonQuery();
                }

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
        }

        public void editService(Part nPart)
        {
            Part part = new Part();
            try
            {
                part = GetPartByID(nPart.part_id);
                connection.Open();
                if (part != null)
                {
                    SqlCommand command = new SqlCommand("UPDATE [SWP].[dbo].[Part] SET[part_name] = @part_name, " +
                                                        "[price] = @price, [quantity] = @quantity, " +
                                                        "[img] = @img " +
                                                        "WHERE[part_id] = @part_id", connection);
                    command.Parameters.AddWithValue("part_id", nPart.part_id);
                    command.Parameters.AddWithValue("part_name", nPart.part_name);
                    command.Parameters.AddWithValue("price", nPart.price);
                    command.Parameters.AddWithValue("quantity", nPart.quantity);
                    command.Parameters.AddWithValue("img", nPart.img);
                    command.ExecuteNonQuery();

                }
                else
                {
                    throw new Exception("This part does not already exist.");
                }

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
        }


    }
}
