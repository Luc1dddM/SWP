using SWP_CarService_Final.Areas.User.Models;

namespace SWP_CarService_Final.Models
{
    public class Team
    {
        public string team_id { get; set; }
        public string team_name { get; set; }
        public DateTime created { get; set; }
        public List<User> memberList { get; set; }

    }
}
