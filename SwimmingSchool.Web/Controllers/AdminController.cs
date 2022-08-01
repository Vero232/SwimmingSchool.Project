using Microsoft.AspNetCore.Mvc;
using SwimmingSchool.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Configuration;

namespace SwimmingSchool.Web.Controllers
{
    public class AdminController : Controller
    {

        private readonly AppDbContext _db;

        public AdminController(AppDbContext db)
        {
            _db = db;
        }
 
        public IActionResult Index()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Development.json", optional: false);

            IConfiguration config = builder.Build();
            var initialAdminUser = new AdminUser();

            initialAdminUser.EmailAddress = config.GetValue<string>("DefaultAdminLogin:username");
            initialAdminUser.Password = config.GetValue<string>("DefaultAdminLogin:password");

            if (_db.AdminUsers.Where(x => x.EmailAddress == "admin@gmail.com").FirstOrDefault() == null)
            {
                _db.AdminUsers.Add(initialAdminUser);
                _db.SaveChanges();
            }

            if (HttpContext.Session.GetString("AdminUser") != null)
            {
                return RedirectToAction("AttendanceReports", "Attendance");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Login(AdminUser adminUser)
        {
            if (ModelState.IsValid)
            {
            

                var admin = _db.AdminUsers.Where(s => s.EmailAddress.Equals(adminUser.EmailAddress) && s.Password.Equals(adminUser.Password));
                if (admin.Count() > 0)
                {

                    HttpContext.Session.SetString("AdminUser", admin.FirstOrDefault().EmailAddress);
                 
                    return RedirectToAction("Index");
                }
                else
                {
                    
                    return RedirectToAction("Login");
                }
            }
            return View();
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }


        [HttpGet, Route("create-admin")]
        public IActionResult CreateAdmin()
        {

            return View();
        }

        [HttpPost, Route("create-admin")]
        public IActionResult CreateAdmin(AdminUser admin)
        {

            if (!ModelState.IsValid)
                return View();

            if (_db.AdminUsers.Where(x => x.EmailAddress == admin.EmailAddress).FirstOrDefault() == null)
            {
                _db.AdminUsers.Add(admin);
                _db.SaveChanges();
            }

            return RedirectToAction("Login");
        }
    }
}
