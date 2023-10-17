using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using SWP_CarService_Final.Models;
using SWP_CarService_Final.Services;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web;


namespace SWP_CarService_Final.Controllers
{
    public class TaskController : Controller
    {
        TaskService taskService = new TaskService();
        readonly string rootFolder = @"D:\FPT\SWP391\Garage\SWP_CarService_Final\wwwroot\img";


        public IActionResult ListOfServices()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ListOfService(string task_id)
        {
            TaskService taskService = new TaskService();
            taskService.Remove(task_id);
            return RedirectToAction("ListOfServices"); 
        }
        public IActionResult AddService()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddService(string ServiceName,IFormFile fileImg, string Price, string description, string choice)
        {
            string ImgName = "";
            try
            {
                if(fileImg != null)
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
                    ImgName = "";
                }
                Models.Task service = new Models.Task()
                {
                    taskName = ServiceName,
                    img = ImgName,
                    price = decimal.Parse(Price),
                    Description = description,
                    active = choice == "active" ? true : false
                };
                taskService.createService(service);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

            return RedirectToAction("ListOfServices");
        }
        public IActionResult EditService()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Edit(string task_id)
        {
            TempData["serviceId"] = task_id;
            return RedirectToAction("EditService");
        }
        [HttpPost]
        public IActionResult Edit(string ServiceId, string ServiceName, IFormFile fileImg, string Price, string description, string choice)
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
                    ImgName = fileImg.FileName;
                    Models.Task service = new Models.Task()
                    {
                        taskID = ServiceId,
                        taskName = ServiceName,
                        img = ImgName,
                        price = decimal.Parse(Price),
                        Description = description,
                        active = choice == "active" ? true : false
                    };
                    taskService.editService(service);
                }
                else
                {
                    Models.Task service = new Models.Task()
                    {
                        taskID = ServiceId,
                        taskName = ServiceName,
                        img = taskService.GetTaskByID(ServiceId).img,
                        price = decimal.Parse(Price),
                        Description = description,
                        active = choice == "active" ? true : false
                    };
                    taskService.editService(service);
                }

                
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return RedirectToAction("ListOfServices");
        }


    }
}
