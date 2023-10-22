using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SWP_CarService_Final.Models;
using SWP_CarService_Final.Services;

namespace SWP_CarService_Final.Controllers
{
    public class ProfileController : Controller
    {
        readonly string rootFolder = @"D:\SE1702\SWP code\SWP_CarService_Final\wwwroot\img";
        private readonly CustomerAccountService _accountService;
        private readonly IHttpContextAccessor _contx;


        public ProfileController(CustomerAccountService accountService, IHttpContextAccessor contx)
        {
            _accountService = accountService;
            _contx = contx;
        }

        public IActionResult EditProfile()
        {
            string cCustomerString = _contx.HttpContext.Session.GetString("cCus");
            Customer cCustomer = JsonConvert.DeserializeObject<Customer>(cCustomerString);
            /*ViewBag.Customer = cCustomer;*/
            return View();
        }

        [HttpPost]
        public IActionResult EditProfile(string user_name, string fullname, string email, string phone_number, IFormFile fileImg, string account_status)
        {
            string ImgName = "";
            try
            {
                string cCustomerString = _contx.HttpContext.Session.GetString("cCus");
                Customer cCustomer = JsonConvert.DeserializeObject<Customer>(cCustomerString);

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

                    Models.Customer cus = new Models.Customer()
                    {
                        user_name = cCustomer.user_name,
                        fullName = fullname,
                        email = email,
                        phone_number = phone_number,
                        account_status = account_status == "active" ? true : false,
                        img = ImgName
                    };
                    _accountService.editProfile(cus);
                    string currentCustomer = JsonConvert.SerializeObject(cus);
                    _contx.HttpContext.Session.SetString("cCus", currentCustomer);
                }
                else
                {
                    Models.Customer cus = new Models.Customer()
                    {
                        user_name = cCustomer.user_name,
                        fullName = fullname,
                        email = email,
                        phone_number = phone_number,
                        account_status = true,
                        img = cCustomer.img
                    };
                    _accountService.editProfile(cus);
                    string currentCustomer = JsonConvert.SerializeObject(cus);
                    _contx.HttpContext.Session.SetString("cCus", currentCustomer);
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return RedirectToAction("Index", "Home");
        }
    }
}

