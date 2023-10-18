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
        readonly string rootFolder = @"D:\FPT\SWP391\Garage\SWP_CarService_Final\wwwroot\img";


        public IActionResult ListOfServices()
        {
            return View();
        }

        public IActionResult Remove(string task_id)
        {
            _taskService.Remove(task_id);
            return Redirect("ListOfServices");
        }

        public IActionResult AddService()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddService(string ServiceName, IFormFile fileImg, string Price, string description, string choice)
        {
            string ImgName = "";
            try
            {
                if (fileImg != null)
                {
                    if (System.IO.File.Exists(Path.Combine(rootFolder, fileImg.FileName)))
                    {
                        // If file found, delete it
                        System.IO.File.Delete(Path.Combine(rootFolder, fileImg.FileName));
                    }
                    ImgName = Path.GetFileName(fileImg.FileName);
                    string uploadfilepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", ImgName);
                    var stream = new FileStream(uploadfilepath, FileMode.Create);
                    fileImg.CopyToAsync(stream);
                    stream.Close();
                    ImgName = fileImg.FileName;
                }
                else
                {
                    ImgName = null;
                }
                Task service = new Task()
                {
                    taskName = ServiceName,
                    img = ImgName,
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
        public IActionResult EditService(string ServiceId, string ServiceName, IFormFile fileImg, string Price, string description, string choice)
        {
            string ImgName = "";
            try
            {
                if (fileImg != null)
                {
                    if (System.IO.File.Exists(Path.Combine(rootFolder, fileImg.FileName)))
                    {
                        // If file found, delete it
                        System.IO.File.Delete(Path.Combine(rootFolder, fileImg.FileName));
                    }
                    if (_taskService.GetTaskByID(ServiceId).img != null)
                    {
                        System.IO.File.Delete(Path.Combine(rootFolder, _taskService.GetTaskByID(ServiceId).img));
                    }
                    ImgName = Path.GetFileName(fileImg.FileName);
                    string uploadfilepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", ImgName);
                    var stream = new FileStream(uploadfilepath, FileMode.Create);
                    fileImg.CopyToAsync(stream);
                    stream.Close();
                    Task service = new Task()
                    {
                        taskID = ServiceId,
                        taskName = ServiceName,
                        img = ImgName,
                        price = decimal.Parse(Price),
                        Description = description,
                        active = choice == "active" ? true : false
                    };
                    _taskService.editService(service);
                }
                else
                {
                    Task service = new Task()
                    {
                        taskID = ServiceId,
                        taskName = ServiceName,
                        img = _taskService.GetTaskByID(ServiceId).img,
                        price = decimal.Parse(Price),
                        Description = description,
                        active = choice == "active" ? true : false
                    };
                    _taskService.editService(service);
                }


            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return Redirect("ListOfServices");
        }


    }
}
