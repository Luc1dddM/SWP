using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Newtonsoft.Json;
using SWP_CarService_Final.Areas.User.Models;
using SWP_CarService_Final.Models;
using SWP_CarService_Final.Services;
using System.Diagnostics.Metrics;

namespace Areas
{
    [Area("User")]
    public class TaskDetailController : Controller
    {
        private readonly TaskDetailService _taskDetailService;
        private readonly IHttpContextAccessor _context;
        private readonly OrderService _orderService;


        public TaskDetailController(TaskDetailService taskDetailService, IHttpContextAccessor context, OrderService orderService)
        {
            _taskDetailService = taskDetailService;
            _context = context;
            _orderService = orderService;
        }
        public IActionResult Index()
        {
            return View();
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

        public IActionResult RequestComplete(string wodID)
        {
            TaskDetail newTaskDetail = _taskDetailService.getTaskDetailByID(wodID);
            newTaskDetail.status = "Request Complete";
            _taskDetailService.updateTaskDetail(newTaskDetail);
            Console.WriteLine(newTaskDetail.WorkOrder.WorkOrderID);
            return Redirect("/user/TaskDetail/view");
        }

        public IActionResult Edit(string wodID)
        {
            TaskDetail detail = _taskDetailService.getTaskDetailByID(wodID);
            _orderService.updateTotalWordOrder(wodID);
            return View(detail);
        }

        [HttpPost]
        public IActionResult Edit(string id, int quantity, string member)
        {
            TaskDetail newTaskDetail = _taskDetailService.getTaskDetailByID(id);
            newTaskDetail.quantity = quantity;
            newTaskDetail.userName = member;
            _taskDetailService.updateTaskDetail(newTaskDetail);
            _orderService.updateTotalWordOrder(newTaskDetail.WorkOrder.WorkOrderID);
            Console.WriteLine(newTaskDetail.WorkOrder.WorkOrderID);
            return Redirect($"/user/OrderDetail/view?WorkOrderID={newTaskDetail.WorkOrder.WorkOrderID}");
        }

        public IActionResult ResponseRequest(string wodID, string Response)
        {
            TaskDetail newTaskDetail = _taskDetailService.getTaskDetailByID(wodID);
            newTaskDetail.status = Response;
            _taskDetailService.updateTaskDetail(newTaskDetail);
            _orderService.updateTotalWordOrder(newTaskDetail.WorkOrder.WorkOrderID);
            return Redirect("/user/TaskDetail/view");
        }

        public IActionResult delete(string taskId, string woid)
        {
            _taskDetailService.DeleteTaskDetail(taskId);
            _orderService.updateTotalWordOrder(woid);
            return Redirect($"/user/OrderDetail/view?WorkOrderID={woid}");

        }
    }
}
