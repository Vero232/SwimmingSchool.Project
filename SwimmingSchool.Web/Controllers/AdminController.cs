using Microsoft.AspNetCore.Mvc;
using SwimmingSchool.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Cryptography;
using System.Text;
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
            var initialAdminUser = new AdminUser();

            initialAdminUser.EmailAddress = "admin@gmail.com";
            initialAdminUser.Password  = "admin01";

            if (_db.AdminUsers.Where(x => x.EmailAddress == "admin@gmail.com").FirstOrDefault() == null)
            {
                _db.AdminUsers.Add(initialAdminUser);
                _db.SaveChanges();
            }

            if (HttpContext.Session.GetString("AdminUser") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        public IActionResult Login(AdminUser adminUser)
        {
            if (ModelState.IsValid)
            {
            

                var data = _db.AdminUsers.Where(s => s.EmailAddress.Equals(adminUser.EmailAddress) && s.Password.Equals(adminUser.Password)).ToList();
                if (data.Count() > 0)
                {

                    HttpContext.Session.SetString("AdminUser", data.FirstOrDefault().EmailAddress);
                 
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Login failed";
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



        [Route("create-admin")]
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
