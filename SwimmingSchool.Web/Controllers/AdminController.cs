using Microsoft.AspNetCore.Mvc;
using SwimmingSchool.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

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
      
          //  var hashedPassword = new PasswordHasher<object?>().HashPassword(null, password);
            if (HttpContext.Session.GetString("AdminUser") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        public IActionResult Login(string emailAddress, string password)
        {
            if (ModelState.IsValid)
            {

                var data = _db.AdminUsers.Where(s => s.EmailAddress.Equals(emailAddress) && s.Password.Equals(password)).ToList();
                if (data.Count() > 0)
                {
                    //add session

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


        //Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();//remove session
            return RedirectToAction("Login");
        }

    }
}
