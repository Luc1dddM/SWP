using SWP_CarService_Final.Areas.User.Models;

namespace SWP_CarService_Final.Models
{
    public class WorkOrder
    {
        public string WorkOrderID { get; set; }
        public string VehicleType { get; set;}
        public decimal Total {  get; set;}
        public string CustomerName { get; set;}
        public string CreatedBy { get; set; }
        public DateTime createdAt { get; set; }
        public Customer? customer { get; set; }
        public List<TaskDetail>? taskDetails { get; set; }
        public List<PartDetail>? partDetails { get; set; }
    }
}
