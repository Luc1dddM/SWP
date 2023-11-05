using SWP_CarService_Final.Areas.User.Models;
using SWP_CarService_Final.Models;
using System.Data.SqlClient;

namespace SWP_CarService_Final.Services
{
    public class RoleService : DBContext
    {
        public List<Role> getLeaderAndMemberRoleId()
        {
            List<Role> roles = new List<Role>();
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT role_id, role_name FROM [Role] Where role_name IN ('leader', 'member') AND role_name <> 'admin'", connection);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var role = new Role()
                        {
                           role_id = reader.GetString(0).Trim(),
                           role_name = reader.GetString(1).Trim(),
                        };
                        roles.Add(role);
                    }

                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return roles;
        }
    }
}
