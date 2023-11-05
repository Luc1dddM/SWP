using SWP_CarService_Final.Models;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace SWP_CarService_Final.Services
{
    public class PartDetailService : DBContext
    {
        public List<PartDetail> getPartRequestList(string UserName)
        {
            List<PartDetail> list = new List<PartDetail>();
            PartService partService = new PartService();
            UserServices userSerivce = new UserServices();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("select * from part_detail where part_detail.userName in (select [user].UserName from [user] join Team_Members on [user].UserName = Team_Members.userName where Team_Members.team_id = " +
                    "(select team_id from Team_Members join [user] on [user].UserName = Team_Members.userName where [User].UserName = @UserName))" +
                    "and part_detail.status = 'Request Use'", connection);
                cmd.Parameters.AddWithValue("UserName", UserName);
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) {
                        PartDetail detail = new PartDetail()
                        {
                            ItemDetailId = reader.GetString(0),
                            quantity = reader.GetInt32(1),
                            price = reader.GetDecimal(2),
                            status = reader.GetString(3),
                            created = reader.GetDateTime(4),
                            updated = reader.GetDateTime(5),
                            WorkOrderId = reader.GetString(6),
                            userName = reader.GetString(7),
                            user = userSerivce.getUserByUsername(reader.GetString(7)),
                            partID = reader.GetString(8),
                            part = partService.GetPartByID(reader.GetString(8))
                        };
                        list.Add(detail);
                    }
                }
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }finally { connection.Close(); }
            return list;
        }

        public string getLastID()
        {
            string lastId = null;
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT top 1 item_detail_id FROM part_detail ORDER BY CAST(SUBSTRING(part_detail.item_detail_id, PATINDEX('%[0-9]%', item_detail_id), LEN(item_detail_id)) AS INT) desc", connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lastId = reader.GetString(0);
                    }
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
            return lastId;
        }

        public PartDetail checkPartDetailExist(string wodId, string partId, string userName)
        {
            PartDetail part = null;
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("select * from part_detail where part_detail.WorkOrder_id = @WodId and part_detail.part_id = @PartId and part_detail.userName = @UserName", connection);
                cmd.Parameters.AddWithValue("WodId", wodId);
                cmd.Parameters.AddWithValue("PartId", partId);
                cmd.Parameters.AddWithValue("UserName", userName);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        part = new PartDetail()
                        {
                            ItemDetailId = reader.GetString(0),
                            quantity = reader.GetInt32(1),
                            price = reader.GetDecimal(2),
                            status = reader.GetString(3),
                            created = reader.GetDateTime(4),
                            updated = reader.GetDateTime(5),
                            WorkOrderId = reader.GetString(6),
                            userName = reader.GetString(7),
                            partID = reader.GetString(8),
                        };
                    }
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
            return part;
        }

        public void updatePartDetail(PartDetail updatePartDetail)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("update part_detail set quantity = @quantity, [status] = @status, updated = @update where item_detail_id = @id", connection);
                cmd.Parameters.AddWithValue("quantity", updatePartDetail.quantity);
                cmd.Parameters.AddWithValue("status", updatePartDetail.status);
                cmd.Parameters.AddWithValue("update", updatePartDetail.updated);
                cmd.Parameters.AddWithValue("id", updatePartDetail.ItemDetailId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
        }

        public void createPartDetail(PartDetail newPartDetail)
        {
            string IDFormat = "ITD";

            try
            {
                PartDetail partDetailTemp = checkPartDetailExist(newPartDetail.WorkOrderId, newPartDetail.partID, newPartDetail.userName);
                if (partDetailTemp != null)
                {
                    throw new Exception("Part still in your request list!");
                }
                else
                {
                    string lastID = getLastID();
                    string id;
                    if (lastID != null)
                    {
                        id = IDFormat + (int.Parse(lastID.Substring(IDFormat.Length)) + 1);
                    }
                    else
                    {
                        id = IDFormat + "1";
                    }

                    connection.Open();
                    SqlCommand cmd = new SqlCommand("insert part_detail values(@id, @quantity, @Price, @status, @created, @updated, @WorkOrderId, @UserName, @partID)", connection);
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.AddWithValue("quantity", newPartDetail.quantity);
                    cmd.Parameters.AddWithValue("Price", newPartDetail.price);
                    cmd.Parameters.AddWithValue("status", newPartDetail.status);
                    cmd.Parameters.AddWithValue("created", newPartDetail.created);
                    cmd.Parameters.AddWithValue("updated", newPartDetail.updated);
                    cmd.Parameters.AddWithValue("WorkOrderId", newPartDetail.WorkOrderId);
                    cmd.Parameters.AddWithValue("UserName", newPartDetail.userName);
                    cmd.Parameters.AddWithValue("partID", newPartDetail.partID);
                    cmd.ExecuteNonQuery();
                }
            }
                
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
        }

        public PartDetail getPartDetailById(string id)
        {
            PartDetail partDetail = null;
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("select * from part_detail where part_detail.item_detail_id = @id", connection);
                cmd.Parameters.AddWithValue("id", id);
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        partDetail = new PartDetail()
                        {
                            ItemDetailId = reader.GetString(0),
                            quantity = reader.GetInt32(1),
                            price = reader.GetDecimal(2),
                            status = reader.GetString(3),
                            created = reader.GetDateTime(4),
                            updated = reader.GetDateTime(5),
                            WorkOrderId = reader.GetString(6),
                            userName = reader.GetString(7),
                            partID = reader.GetString(8),
                        };
                    }
                }
            }catch (Exception ex) { throw new Exception(ex.Message, ex); } finally { connection.Close(); };
            return partDetail;
        }

        public List<PartDetail> getPartDetailsOfMember(string UserName)
        {
            PartService partService = new PartService();
            List<PartDetail> partDetails = new List<PartDetail>();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("select * from part_detail where part_detail.userName =  @UserName", connection);
                cmd.Parameters.AddWithValue("UserName", UserName);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PartDetail partDetail = new PartDetail()
                        {
                            ItemDetailId = reader.GetString(0),
                            quantity = reader.GetInt32(1),
                            price = reader.GetDecimal(2),
                            status = reader.GetString(3),
                            created = reader.GetDateTime(4),
                            updated = reader.GetDateTime(5),
                            WorkOrderId = reader.GetString(6),
                            userName = reader.GetString(7),
                            partID = reader.GetString(8),
                            part = partService.GetPartByID(reader.GetString(8)),
                        };
                        partDetails.Add(partDetail);
                    }
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
            return partDetails;
        }

        public List<PartDetail> GetPartDetailsByOrderID(string id)
        {
            List<PartDetail> partDetails = new List<PartDetail>();
            PartService partService = new PartService();
            UserServices userSerivce = new UserServices();
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("select * from part_detail where part_detail.WorkOrder_id = @id", connection);
                cmd.Parameters.AddWithValue("id", id);
                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PartDetail part = new PartDetail()
                        {
                            ItemDetailId = reader.GetString(0),
                            quantity = reader.GetInt32(1),
                            price = reader.GetDecimal(2),
                            status = reader.GetString(3),
                            created = reader.GetDateTime(4),
                            updated = reader.GetDateTime(5),
                            WorkOrderId = reader.GetString(6),
                            userName = reader.GetString(7),
                            user = userSerivce.getUserByUsername(reader.GetString(7)),
                            partID = reader.GetString(8),
                            part = partService.GetPartByID(reader.GetString(8)),
                        };
                        partDetails.Add(part);
                    }
                }
            }catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
            return partDetails;
        }

        public List<PartDetail> GetPartDetailsByOrderID(string id, string UserName)
        {
            List<PartDetail> partDetails = new List<PartDetail>();
            PartService partService = new PartService();
            UserServices userSerivce = new UserServices();
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("select * from part_detail where part_detail.WorkOrder_id = @id and part_detail.userName = @userName", connection);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("userName", UserName);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PartDetail part = new PartDetail()
                        {
                            ItemDetailId = reader.GetString(0),
                            quantity = reader.GetInt32(1),
                            price = reader.GetDecimal(2),
                            status = reader.GetString(3),
                            created = reader.GetDateTime(4),
                            updated = reader.GetDateTime(5),
                            WorkOrderId = reader.GetString(6),
                            userName = reader.GetString(7),
                            user = userSerivce.getUserByUsername(reader.GetString(7)),
                            partID = reader.GetString(8),
                            part = partService.GetPartByID(reader.GetString(8)),
                        };
                        partDetails.Add(part);
                    }
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
            return partDetails;
        }

        public void deletePartDetail(string id)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("delete part_detail where item_detail_id = @id", connection);
                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
            }catch (Exception ex) { throw new Exception(ex.Message); }
            finally { connection.Close(); }
        }

       
    }
}
