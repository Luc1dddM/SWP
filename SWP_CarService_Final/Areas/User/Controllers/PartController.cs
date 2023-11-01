using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP_CarService_Final.Services;
using SWP_CarService_Final.Models;
using Newtonsoft.Json;

namespace SWP_CarService_Final.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "admin, member")]
    public class PartController : Controller
    {
        private readonly PartService _partService;
        private readonly IHttpContextAccessor _contx;

        public PartController(PartService partService, IHttpContextAccessor contx)
        {
            _partService = partService;
            _contx = contx;
        }

        public IActionResult ListOfComponent(int pageNumber, List<string>? filterString, string? StartPrice, string? EndPrice, bool onChange, string? SearchText)
        {
            List<Part> partList = null;
            List<string> myList = null;
            string startPrice = null;
            string endPrice = null;
            string searchText = null;

            if (onChange)
            {
                string currentFilterString = JsonConvert.SerializeObject(filterString);
                _contx.HttpContext.Session.SetString("cFil", currentFilterString);

                string currentStartPrice = JsonConvert.SerializeObject(StartPrice);
                _contx.HttpContext.Session.SetString("cStart", currentStartPrice);

                string currentEndPrice = JsonConvert.SerializeObject(EndPrice);
                _contx.HttpContext.Session.SetString("cEnd", currentEndPrice);

                string currentSearchText = JsonConvert.SerializeObject(SearchText);
                _contx.HttpContext.Session.SetString("cSText", currentSearchText);

            }
            string cFilterString = _contx.HttpContext.Session.GetString("cFil");
            myList = JsonConvert.DeserializeObject<List<string>>(cFilterString);

            string cStartPrice = _contx.HttpContext.Session.GetString("cStart");
            startPrice = JsonConvert.DeserializeObject<string>(cStartPrice);

            string cEndPrice = _contx.HttpContext.Session.GetString("cEnd");
            endPrice = JsonConvert.DeserializeObject<string>(cEndPrice);

            string cSearchText = _contx.HttpContext.Session.GetString("cSText");
            searchText = JsonConvert.DeserializeObject<string>(cSearchText);

            ViewBag.ListComponetRaw = _partService.GetAllPartFilterRaw(pageNumber, myList, startPrice, endPrice, searchText);
            ViewBag.CurrentPage = pageNumber;
            partList = _partService.GetAllPartFilter(pageNumber, myList, startPrice, endPrice,searchText);

            return View(partList);
        }

        public IActionResult AddComponent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddComponent(string name, IFormFile img, string price, string quantity)
        {
            string ImgName = "";
            try
            {
                if (img != null)
                {
                    if (System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", img.FileName)))
                    {
                        // If file found, delete it
                        System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", img.FileName));
                    }
                    ImgName = Path.GetFileName(img.FileName);
                    string uploadfilepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", ImgName);
                    var stream = new FileStream(uploadfilepath, FileMode.Create);
                    img.CopyToAsync(stream);
                    stream.Close();
                    ImgName = img.FileName;
                }
                else
                {
                    ImgName = null;
                }
                Part part = new Part()
                {
                    part_name = name,
                    price = decimal.Parse(price),
                    img = ImgName,
                    quantity = int.Parse(quantity)
                };
                /*                _partService.createPart(part);*/
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

            return Redirect("ListOfComponent?pageNumber=1");
        }
    }
}
