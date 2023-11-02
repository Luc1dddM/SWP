using SWP_CarService_Final.Areas.User.Models;

namespace SWP_CarService_Final.Models
{
    public class PartDetail
    {
        public string ItemDetailId { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public string status { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
        public string WorkOrderId { get; set; }
        public string userName { get; set; }
        public string partID { get; set; }
        public WorkOrder? order { get; set; }
        public User? user { get; set; }
        public Part? part { get; set; }
    }
}
