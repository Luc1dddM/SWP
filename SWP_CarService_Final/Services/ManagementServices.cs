using SWP_CarService_Final.Models;
using System.Data.SqlClient;

namespace SWP_CarService_Final.Services
{
    public class ManagementServices : DBContext
    {
        public decimal StatisticIncomeForADay()
        {
            decimal result = 0;
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT SUM(Total) FROM [SWP].[dbo].[Work_order] where  CAST( created AS Date ) = CAST( GETDATE() AS Date )", connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = (reader.IsDBNull(0))?0:reader.GetDecimal(0);
                    }
                }
            }
            finally { connection.Close(); }
            return result;
        }

        public int StatisticOrderForADay()
        {
            int result = 0;
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(WorkOrder_id) FROM [SWP].[dbo].[Work_order] where  CAST( created AS Date ) = CAST( GETDATE() AS Date )", connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = (reader.IsDBNull(0)) ? 0 : reader.GetInt32(0);
                    }
                }
            }
            finally { connection.Close(); }
            return result;
        }

        public int StatisticNumberOfEmployee()
        {
            int result = 0;
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(a.UserName) from [User] a join User_role b on a.UserName = b.userName where b.role_id != 1", connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = (reader.IsDBNull(0)) ? 0 : reader.GetInt32(0);
                    }
                }
            }
            finally { connection.Close(); }
            return result;
        }

        public int StatisticNumberOfCustomer()
        {
            int result = 0;
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT([user_name]) from Customer where account_status = 1", connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = (reader.IsDBNull(0)) ? 0 : reader.GetInt32(0);
                    }
                }
            }
            finally { connection.Close(); }
            return result;
        }

        public List<decimal> StatisticIncomeByMonthForYear()
        {
            List<decimal> results = new List<decimal>();
            try
            {
                connection.Open();
                for (int i = 1; i <= 12; i++)
                {
                    string SQL = "SELECT SUM(Total) FROM [SWP].[dbo].[Work_order] where  Month(CAST( created AS Date )) = "+ i +" and YEAR(CAST( created AS Date )) = YEAR(CAST( GETDATE() AS Date ))";
                    SqlCommand cmd = new SqlCommand(SQL, connection);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            decimal result = (reader.IsDBNull(0)) ? 0 : reader.GetDecimal(0);
                            results.Add(result);
                        }
                    }
                }

            }
            finally { connection.Close(); }
            return results;
        }

        public List<decimal> StatisticIncomeByWeekForMonth()
        {
            List<decimal> results = new List<decimal>();
            try
            {
                connection.Open();
                for (int i = 0; i < 28; i+=7 )
                {
                    string SQL = "SELECT SUM(Total) FROM Work_order where CAST(created as date ) " +
                        " BETWEEN CAST(DATEADD(DAY, "+i+" , CAST(DATEADD(month, DATEDIFF(month, 1, CAST(GETDATE() as date )), 0) AS date)) AS DATE) " +
                        " AND CAST(DATEADD(DAY, "+(i+6)+" , CAST(DATEADD(month, DATEDIFF(month, 1, CAST(GETDATE() as date )), 0) AS date)) AS DATE)";
                    SqlCommand cmd = new SqlCommand(SQL, connection);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            decimal result = (reader.IsDBNull(0)) ? 0: reader.GetDecimal(0);
                            results.Add(result);
                        }
                    }
                }

            }
            finally { connection.Close(); }
            return results;
        }

        public int StatisticOrderNotComplete(string user)
        {
            int result = 0;
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(WorkOrder_id) FROM [SWP].[dbo].[Work_order] WHERE [user] = @user and [Status] = 0 and CAST(created as date) = CAST(GetDate() as date)", connection);
                cmd.Parameters.AddWithValue("user", user);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = (reader.IsDBNull(0)) ? 0 : reader.GetInt32(0);
                    }
                }
            }
            finally { connection.Close(); }
            return result;
        }

        public int StatisticRequestComplete(string user)
        {
            int result = 0;
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("select COUNT(wod_id) from task_detail " +
                                                "where task_detail.userName in (select [user].UserName " +
                                                                               "from [user] join Team_Members on [user].UserName = Team_Members.userName " +
                                                                               "where Team_Members.team_id = (select team_id from Team_Members " +
                                                                                                             "join [user] on [user].UserName = Team_Members.userName where [User].UserName = @user)) " +
                                                "and task_detail.status = 'Request Complete' and CAST(task_detail.updated as date) = CAST(GETDATE() as date)", connection);
                cmd.Parameters.AddWithValue("user", user);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = (reader.IsDBNull(0)) ? 0 : reader.GetInt32(0);
                    }
                }
            }
            finally { connection.Close(); }
            return result;
        }

        public int StatisticRequestRepair(string user)
        {
            int result = 0;
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("select COUNT(wod_id) from task_detail " +
                                                "where task_detail.userName in (select [user].UserName " +
                                                                               "from [user] join Team_Members on [user].UserName = Team_Members.userName " +
                                                                               "where Team_Members.team_id = (select team_id from Team_Members " +
                                                                                                             "join [user] on [user].UserName = Team_Members.userName where [User].UserName = @user)) " +
                                                "and task_detail.status = 'Request Repair' and CAST(task_detail.updated as date) = CAST(GETDATE() as date)", connection);
                cmd.Parameters.AddWithValue("user", user);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = (reader.IsDBNull(0)) ? 0 : reader.GetInt32(0);
                    }
                }
            }
            finally { connection.Close(); }
            return result;
        }

        public int StatisticRequestUseItem(string user)
        {
            int result = 0;
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("select COUNT(item_detail_id) from part_detail " +
                                                "where part_detail.userName in (select [user].UserName " +
                                                                               "from [user] join Team_Members on [user].UserName = Team_Members.userName " +
                                                                               "where Team_Members.team_id = (select team_id from Team_Members " +
                                                                                                             "join [user] on [user].UserName = Team_Members.userName where [User].UserName = @user)) " +
                                                "and part_detail.status = 'Request Use' and CAST(updated as date) = CAST(GETDATE() as date)", connection);
                cmd.Parameters.AddWithValue("user", user);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = (reader.IsDBNull(0)) ? 0 : reader.GetInt32(0);
                    }
                }
            }
            finally { connection.Close(); }
            return result;
        }

        public int StatisticInProcessTask(string user)
        {
            int result = 0;
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("select COUNT(wod_id) from task_detail " +
                                                "where task_detail.userName in (select [user].UserName " +
                                                                               "from [user] join Team_Members on [user].UserName = Team_Members.userName " +
                                                                               "where Team_Members.team_id = (select team_id from Team_Members " +
                                                                                                             "join [user] on [user].UserName = Team_Members.userName where [User].UserName = @user)) " +
                                                "and task_detail.status = 'Process' and CAST(task_detail.updated as date) = CAST(GETDATE() as date)", connection);
                cmd.Parameters.AddWithValue("user", user);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = (reader.IsDBNull(0)) ? 0 : reader.GetInt32(0);
                    }
                }
            }
            finally { connection.Close(); }
            return result;
        }
    }
}
