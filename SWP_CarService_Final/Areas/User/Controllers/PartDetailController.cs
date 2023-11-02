using Microsoft.AspNetCore.Mvc;
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
        public IActionResult request(string partID, string quantity/*, string pagenumber*/)
        {
            string cUserString = _contx.HttpContext.Session.GetString("cUser");
            User cUser = JsonConvert.DeserializeObject<User>(cUserString);
            string OrderId = _orderService.getCurrentWorkOrderId(cUser.UserName);
            Console.WriteLine(OrderId);
            if(OrderId != null)
            {
                Part part = _partService.GetPartByID(partID);
                PartDetail newPartDetail = new PartDetail()
                {
                    ItemDetailId = "1",
                    quantity = int.Parse(quantity),
                    price = part.price,
                    created = DateTime.Now,
                    updated = DateTime.Now,
                    status = "Request Use",
                    WorkOrderId = OrderId,
                    userName = cUser.UserName,
                    partID = partID,
                    part = part
                };
                _partDetailService.createPartDetail(newPartDetail);
                return Redirect("/user/PartDetail/view");
            }
            else
            {
                TempData["Message"] = "You not have any task currently";
                return Redirect($"/user/part/ListOfComponent?pageNumber=1&onChange=true");
            }
        }

        public IActionResult viewListRequest()
        {

            string cUserString = _contx.HttpContext.Session.GetString("cUser");
            User cUser = JsonConvert.DeserializeObject<User>(cUserString);
            List<PartDetail> listParts = _partDetailService.getPartRequestList(cUser.UserName);
            return View(listParts);
        }

        public IActionResult EditRequest(string partDetailId, string quantity)
        {
            int newQuantity = 0;
            PartDetail partDetail = _partDetailService.getPartDetailById(partDetailId);
            Part part = _partService.GetPartByID(partDetail.partID);
            if(partDetail.status.Trim() == "Accepted") { 
                newQuantity = part.quantity + partDetail.quantity;
                partDetail.status = "Request Use";
            }
            else
            {
                newQuantity = part.quantity;
            }
            partDetail.quantity = int.Parse(quantity);
            part.quantity = newQuantity;
            _partService.editService(part);
            _partDetailService.updatePartDetail(partDetail);
            return Redirect("/user/PartDetail/view");
        }

        public IActionResult ResponseRequest(string partDetailId, string Response)
        {
            PartDetail newTaskDetail = _partDetailService.getPartDetailById(partDetailId);
            Part part = _partService.GetPartByID(newTaskDetail.partID);
            part.quantity -= newTaskDetail.quantity;
            newTaskDetail.status = Response;
            _partDetailService.updatePartDetail(newTaskDetail);
            _partService.editService(part);
            _orderService.updateTotalWordOrder(newTaskDetail.WorkOrderId);
            return Redirect("/user/PartDetail/viewListRequest");
        }

        public IActionResult delete(string partDetailId, string wodId) {
            _partDetailService.deletePartDetail(partDetailId);
            _orderService.updateTotalWordOrder(wodId);
            return Redirect($"/user/OrderDetail/view?WorkOrderID={wodId}");
        }
    }
}
