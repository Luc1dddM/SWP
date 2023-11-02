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

            ViewBag.CurrentPage = pageNumber;
            if(myList.Count != 0 || startPrice != null || endPrice != null)
            {
                partList = _partService.GetAllPartFilter(pageNumber, myList, startPrice, endPrice, searchText);
                ViewBag.ListComponetRaw = _partService.GetAllPartFilterRaw(pageNumber, myList, startPrice, endPrice, searchText);

            }
            else
            {
                partList = _partService.GetAllPart(pageNumber,searchText);
                ViewBag.ListComponetRaw = _partService.GetAllPartRaw(pageNumber, searchText);
            }

            return View(partList);
        }

        public IActionResult AddComponent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddComponent(string name, IFormFile img, string price, string quantity, List<string> categories)
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
                _partService.createPart(part, categories);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }

            return Redirect("ListOfComponent?pageNumber=1");
        }

        public IActionResult EditComponent(string part_id)
        {
            Part part = _partService.GetPartByID(part_id);
            return View(part);
        }

        public IActionResult RemovePartCategory(string part_id, string category_id)
        {
            _partService.RemovePartCategory(part_id, category_id);
            return Redirect("EditComponent?part_id="+part_id);
        }

        [HttpPost]
        public IActionResult EditComponent(string id, string name, IFormFile img, string price, string quantity, List<string> categories)
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
                    if (_partService.GetPartByID(id).img != null)
                    {
                        System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", _partService.GetPartByID(id).img));
                    }
                    ImgName = Path.GetFileName(img.FileName);
                    string uploadfilepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", ImgName);
                    var stream = new FileStream(uploadfilepath, FileMode.Create);
                    img.CopyToAsync(stream);
                    stream.Close();
                    Part part = new Part()
                    {
                        part_id = id,
                        part_name = name,
                        price = decimal.Parse(price),
                        quantity = int.Parse(quantity),
                        img = ImgName
                    };
                    _partService.editPart(part,categories);
                }
                else
                {
                    Part part = new Part()
                    {
                        part_id = id,
                        part_name = name,
                        price = decimal.Parse(price),
                        quantity = int.Parse(quantity),
                        img = _partService.GetPartByID(id).img
                    };
                    _partService.editPart(part,categories);
                }


            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return Redirect("ListOfComponent?pageNumber=1&onChange=false");
        }
    }
}
