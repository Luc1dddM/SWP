using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SWP_CarService_Final.Models;
using SWP_CarService_Final.Services;

namespace SWP_CarService_Final.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "admin")]
    public class CustomerController : Controller
    {
        readonly string rootFolder = @"D:\SE1702\SWP code\SWP_CarService_Final\wwwroot\img";
        private readonly CustomerAccountService _customerAccountService;
        private readonly IHttpContextAccessor _contx;

        public CustomerController(CustomerAccountService customerAccountService, IHttpContextAccessor contx)
        {
            _customerAccountService = customerAccountService;
            _contx = contx;
        }

        public IActionResult ListCustomers()
        {
            return View();
        }

        public IActionResult DeleteCus(string user_name)
        {
            _customerAccountService.DeleteCustomer(user_name);
            return Redirect("ListCustomers");
        }

        public IActionResult EditCustomer(string user_name)
        {
            ViewBag.username = user_name;
            return View();
        }

        [HttpPost]
        public IActionResult EditCustomer(string user_name, string fullname, string email, string phone_number, IFormFile fileImg, string account_status)
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
                        if (_customerAccountService.GetCustomerByUsername(user_name).img != null)
                        {
                            System.IO.File.Delete(Path.Combine(rootFolder, _customerAccountService.GetCustomerByUsername(user_name).img));
                        }
                        ImgName = Path.GetFileName(fileImg.FileName);
                        string uploadfilepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", ImgName);
                        var stream = new FileStream(uploadfilepath, FileMode.Create);
                        fileImg.CopyToAsync(stream);
                        stream.Close();
                        Customer cus = new Customer()
                        {
                            user_name = user_name,
                            fullName = fullname,
                            email = email,
                            phone_number = phone_number,
                            account_status = account_status == "active" ? true : false,
                            img = ImgName
                        };
                        _customerAccountService.editProfile(cus);
                    }
                else
                {
                    Customer cus = new Customer()
                    {
                        user_name = user_name,
                        fullName = fullname,
                        email = email,
                        phone_number = phone_number,
                        account_status = account_status == "active" ? true : false,
                        img = _customerAccountService.GetCustomerByUsername(user_name).img
                    };
                    _customerAccountService.editProfile(cus);
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return Redirect("ListCustomers");
        }

        public IActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCustomer(string user_name, string password, string fullName, string email, string phone_number, string account_status)
        {
            Customer newAccount = new Customer()
            {
                user_name = user_name,
                password = password,
                fullName = fullName,
                email = email,
                phone_number = phone_number,
                account_status = true,
                /*img = null*/
            };
            _customerAccountService.CreateCustomer(newAccount);

            return Redirect("ListCustomers");
        }
    }
}
    
    

