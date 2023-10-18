namespace SWP_CarService_Final.Areas.User.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string User_fullname { get; set; }
        public string phone_number { get; set;}
        public string email { get; set;}
        public string password { get; set;}
        public Boolean account_status { get; set; }
        public DateTime created { get; set; }
        
    }
}
