using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Newtonsoft.Json;
using SWP_CarService_Final.Areas.User.Models;
using SWP_CarService_Final.Models;
using SWP_CarService_Final.Services;
using System.Diagnostics.Metrics;
using Task = SWP_CarService_Final.Models.Task;

namespace Areas
{
    [Area("User")]
    public class TaskDetailController : Controller
    {
        private readonly TaskDetailService _taskDetailService;
        private readonly IHttpContextAccessor _context;
        private readonly OrderService _orderService;
        private readonly TaskService _taskService;


        public TaskDetailController(TaskDetailService taskDetailService, IHttpContextAccessor context, OrderService orderService, TaskService taskService )
        {
            _taskDetailService = taskDetailService;
            _context = context;
            _orderService = orderService;
            _taskService = taskService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult create(string WodId)
        {
            ViewBag.WodId = WodId;  
            List<Task> possibleTask = _taskDetailService.getPossibleListRequest(WodId);
            return View(possibleTask);
        }

        [HttpPost]
        public IActionResult create(string WodId, string taskId, string? desctiption)
        {
            string cUserString = _context.HttpContext.Session.GetString("cUser");
            User cUser = JsonConvert.DeserializeObject<User>(cUserString);
            Task task = _taskService.GetTaskByID(taskId);
            TaskDetail newTask;
            if (cUser.role_name.Trim() == "member")
            {
                 newTask = new TaskDetail()
                {
                    wod_id = "1",
                    quantity = 1,
                    price = task.price,
                    description = desctiption != null ? desctiption : "",
                    status = "Request Repair",
                    createdAt = DateTime.Now,
                    updatedAt = DateTime.Now,
                    userName = cUser.UserName,
                    taskId = taskId,
                    WorkOrderId = WodId
                };
            }
            else
            {
                 newTask = new TaskDetail()
                {
                    wod_id = "1",
                    quantity = 1,
                    price = task.price,
                    description = desctiption != null ? desctiption : "",
                    status = "Process",
                    createdAt = DateTime.Now,
                    updatedAt = DateTime.Now,
                    userName = cUser.UserName,
                    taskId = taskId,
                    WorkOrderId = WodId
                };
            }
            
            _taskDetailService.createTaskDetail(newTask);
            _orderService.updateTotalWordOrder(WodId);
            return Redirect($"/user/OrderDetail/view?WorkOrderID={WodId}");
        }

        public IActionResult view()
        {
            string cUserString = _context.HttpContext.Session.GetString("cUser");
            User cUser = JsonConvert.DeserializeObject<User>(cUserString);
            List<TaskDetail> listDetails = _taskDetailService.GetTaskDetailsOfMember(cUser.UserName);
            return View(listDetails);
        }

        public IActionResult ViewListRequest()
        {
            string cUserString = _context.HttpContext.Session.GetString("cUser");
            User cUser = JsonConvert.DeserializeObject<User>(cUserString);
            List<TaskDetail> listDetails = _taskDetailService.getRequestCompleteList(cUser.UserName);
            return View(listDetails);
        }

        public IActionResult ViewListRequestRepair()
        {
            string cUserString = _context.HttpContext.Session.GetString("cUser");
            User cUser = JsonConvert.DeserializeObject<User>(cUserString);
            List<TaskDetail> listDetails = _taskDetailService.getRequestRepair(cUser.UserName);
            return View(listDetails);
        }

        public IActionResult RequestComplete(string wodID)
        {
            TaskDetail newTaskDetail = _taskDetailService.getTaskDetailByID(wodID);
            newTaskDetail.status = "Request Complete";
            _taskDetailService.updateTaskDetail(newTaskDetail);
            return Redirect($"/user/OrderDetail/view?WorkOrderID={newTaskDetail.WorkOrderId}");
        }

        public IActionResult Edit(string wodID)
        {
            TaskDetail detail = _taskDetailService.getTaskDetailByID(wodID);
            _orderService.updateTotalWordOrder(wodID);
            return View(detail);
        }

        [HttpPost]
        public IActionResult Edit(string id, string member)
        {
            TaskDetail newTaskDetail = _taskDetailService.getTaskDetailByID(id);
            newTaskDetail.userName = member;
            _taskDetailService.updateTaskDetail(newTaskDetail);
            _orderService.updateTotalWordOrder(newTaskDetail.WorkOrderId);
            return Redirect($"/user/OrderDetail/view?WorkOrderID={newTaskDetail.WorkOrderId}");
        }

        public IActionResult ResponseRequest(string wodID, string? workOrderId, string Response, string? repair)
        {
            TaskDetail newTaskDetail = _taskDetailService.getTaskDetailByID(wodID);
            newTaskDetail.status = Response;
            _taskDetailService.updateTaskDetail(newTaskDetail);
            _orderService.updateTotalWordOrder(newTaskDetail.WorkOrderId);
            if (workOrderId != null)
            {
                return Redirect($"/user/OrderDetail/view?WorkOrderID={newTaskDetail.WorkOrderId}");
            }
            else
            {
                if(repair != null)
                {
                    return Redirect("/user/TaskDetail/ViewListRequestRepair");
                }
                else
                {
                    return Redirect("/user/TaskDetail/ViewListRequest");
                }
            }
        }

        public IActionResult delete(string taskId, string woid, string? go)
        {
            _taskDetailService.DeleteTaskDetail(taskId);
            _orderService.updateTotalWordOrder(woid);
            if(go != null)
            {
                return Redirect($"/user/OrderDetail/view?WorkOrderID={woid}");
            }
            else{
                return Redirect("/user/TaskDetail/ViewListRequestRepair");
            }
        }
    }
}
