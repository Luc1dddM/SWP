namespace SWP_CarService_Final.Models
{
    public class WorkOrder
    {
        public string WorkOrderID { get; set; }
        public string VehicleType { get; set;}
        public string CustomerName { get; set;}
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set;}
        public DateTime createdAt { get; set; }
    }
}
