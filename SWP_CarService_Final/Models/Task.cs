namespace SWP_CarService_Final.Models
{
    public class Task
    {
        public string taskID;
        public string taskName { get; set; }  
        public decimal price { get; set; }
        public bool actice { get; set; }
        public string img {  get; set; }
    }
}
