﻿using System.Data.SqlClient;

namespace SWP_CarService_Final.Services
{
    public class DBContext
    {
        protected SqlConnection connection { get; set; }

        public DBContext() { 
            connection = new SqlConnection();
            connection.ConnectionString = "Data Source=DESKTOP-IBRTGQQ\\PT_LAP;Initial Catalog=SWP_L;Integrated Security=True;Trusted_Connection=true;MultipleActiveResultSets=True;";
        }

        public SqlConnection _connection()
        {
            return connection;
        }
    }
}
