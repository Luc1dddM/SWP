using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using Task = SWP_CarService_Final.Models.Task;
using SWP_CarService_Final.Services;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;

namespace SWP_CarService_Final.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "admin")]
    public class TaskController : Controller
    {

        private readonly TaskService _taskService;


        public TaskController(TaskService taskService)
        {
            _taskService = taskService;
        }


        public IActionResult ListOfServices()
        {
            return View();
        }

/*        public IActionResult Remove(string task_id)
        {
            _taskService.Remove(task_id);
            return Redirect("ListOfServices");
        }*/

        public IActionResult AddService()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddService(string ServiceName, string Price, string description, string choice)
        {
            string ImgName = "";
            try
            {
                Task service = new Task()
                {
                    taskName = ServiceName,
                    price = decimal.Parse(Price),
                    Description = description,
                    active = choice == "active" ? true : false
                };
                _taskService.createService(service);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

            return Redirect("ListOfServices");
        }

        public IActionResult EditService(string task_id)
        {
            ViewBag.ServiceId = task_id;
            return View();
        }
        [HttpPost]
        public IActionResult EditService(string ServiceId, string ServiceName, string Price, string description, string choice)
        {
            string ImgName = "";
            try
            {

                    Task service = new Task()
                    {
                        taskID = ServiceId,
                        taskName = ServiceName,
                        price = decimal.Parse(Price),
                        Description = description,
                        active = choice == "active" ? true : false
                    };
                    _taskService.editService(service);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return Redirect("ListOfServices");
        }


    }
}
