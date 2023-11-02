using SWP_CarService_Final.Areas.User.Models;

namespace SWP_CarService_Final.Models
{
    public class TaskDetail
    {
        public string wod_id { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public string status { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string userName {  get; set; }
        public User? User { get; set; }
        public Task task { get; set; }
        public WorkOrder WorkOrder { get; set; }
    }
}
