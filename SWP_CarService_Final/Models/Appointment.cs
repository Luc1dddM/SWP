using System.ComponentModel.DataAnnotations;

namespace SWP_CarService_Final.Models
{
    public class Appointment
    {
        
        public string appointmentID {  get; set; }
        public string vehicalType { get; set; }
        public string description { get; set; }
        public DateTime timeArrived { get; set; }
        public DateTime createdAt { get; set; }
        public string status { get; set; }
        public List<AppointmentDetail> details { get; set; }
        public Customer customer { get; set; }
        public string WorkOrderID { get; set; }
    }
}
