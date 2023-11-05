using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using SWP_CarService_Final.Areas.User.Models;
using SWP_CarService_Final.Models;
using SWP_CarService_Final.Services;

namespace Areas
{
    [Area("user")]

    public class PartDetailController : Controller
    {
        private readonly OrderService _orderService;
        private readonly PartDetailService _partDetailService;
        private readonly PartService _partService;
        private readonly IHttpContextAccessor _contx;

        public PartDetailController(OrderService orderService, IHttpContextAccessor contx, PartService partService, PartDetailService partDetailService)
        {
            _orderService = orderService;
            _contx = contx;
            _partService = partService;
            _partDetailService = partDetailService;
        }

        public IActionResult view()
        {
            string cUserString = _contx.HttpContext.Session.GetString("cUser");
            User cUser = JsonConvert.DeserializeObject<User>(cUserString);
            List<PartDetail> listParts = _partDetailService.getPartDetailsOfMember(cUser.UserName);
            return View(listParts);
        }

        [HttpPost]
        public IActionResult request(string partID, string WorkOrderId, string quantity)
        {
            string cUserString = _contx.HttpContext.Session.GetString("cUser");
            User cUser = JsonConvert.DeserializeObject<User>(cUserString);

            Part part = _partService.GetPartByID(partID);
            PartDetail newPartDetail = new PartDetail()
            {
                ItemDetailId = "1",
                quantity = int.Parse(quantity),
                price = part.price,
                created = DateTime.Now,
                updated = DateTime.Now,
                status = "Request Use",
                WorkOrderId = WorkOrderId,
                userName = cUser.UserName,
                partID = partID,
                part = part
            };
            _partDetailService.createPartDetail(newPartDetail);
            return Redirect("/user/part/listOfComponent?pageNumber=1&onChange=true");
        }

        public IActionResult viewListRequest()
        {

            string cUserString = _contx.HttpContext.Session.GetString("cUser");
            User cUser = JsonConvert.DeserializeObject<User>(cUserString);
            List<PartDetail> listParts = _partDetailService.getPartRequestList(cUser.UserName);
            return View(listParts);
        }

        public IActionResult EditRequest(string partDetailId, string workOrderDetail, string quantity)
        {
            int newQuantity = 0;
            PartDetail partDetail = _partDetailService.getPartDetailById(partDetailId);
            Part part = _partService.GetPartByID(partDetail.partID);
            if (partDetail.status.Trim() == "Accepted")
            {
                newQuantity = part.quantity + partDetail.quantity;
                partDetail.status = "Request Use";
            }
            else
            {
                newQuantity = part.quantity;
            }
            partDetail.quantity = int.Parse(quantity);
            part.quantity = newQuantity;
            _partService.editPart(part, null);
            _partDetailService.updatePartDetail(partDetail);
            _orderService.updateTotalWordOrder(partDetail.WorkOrderId);
            return Redirect($"/user/OrderDetail/view?WorkOrderID={workOrderDetail}");
        }


        public IActionResult ResponseRequest(string partDetailId, string? workOrderId, string Response)
        {
            PartDetail newTaskDetail = _partDetailService.getPartDetailById(partDetailId);
            Part part = _partService.GetPartByID(newTaskDetail.partID);
            part.quantity -= newTaskDetail.quantity;
            newTaskDetail.status = Response;
            _partDetailService.updatePartDetail(newTaskDetail);
            _partService.editPart(part, null);
            _orderService.updateTotalWordOrder(newTaskDetail.WorkOrderId);
            if (workOrderId != null)
            {
                return Redirect($"/user/OrderDetail/view?WorkOrderID={workOrderId}");
            }
            else
            {
                return Redirect("/user/PartDetail/viewListRequest");
            }
        }

        public IActionResult delete(string partDetailId, string? wodId)
        {
            PartDetail newTaskDetail = _partDetailService.getPartDetailById(partDetailId);
            _partDetailService.deletePartDetail(partDetailId);
            _orderService.updateTotalWordOrder(newTaskDetail.WorkOrderId);
            if (wodId != null)
            {
                return Redirect($"/user/OrderDetail/view?WorkOrderID={wodId}");
            }
            else
            {
                return Redirect("/user/PartDetail/view");
            }

        }
    }
}
