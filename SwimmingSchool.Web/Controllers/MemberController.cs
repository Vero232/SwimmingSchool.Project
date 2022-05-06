using Microsoft.AspNetCore.Mvc;
using SwimmingSchool.Web.Models;

namespace SwimmingSchool.Web.Controllers
{

    public class MemberController : Controller
    {
        private readonly AppDbContext _db;

        public MemberController(AppDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet, Route("registration")]
        public IActionResult RegisterMember()
        {

            return View();
        }


        [HttpPost, Route("registration")]
        public IActionResult RegisterMember(Member Member)
        {
           
            if (!ModelState.IsValid)
                return View();

            
            Member.RegistrationDate = DateTime.Today;

            _db.Members.Add(Member);
            _db.SaveChanges();

            return RedirectToAction("ThankYou");
        }


        [Route("members")]
        public IActionResult Members()
        {
            if (HttpContext.Session.GetString("AdminUser") != null)
            {
                var members = _db.Members.ToList();
                return View(members);
            }
            else {

                return RedirectToAction("Login", "Admin");
            }
    
        }

        [HttpGet, Route("login")]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost, Route("login")]
        public IActionResult Login(string EmailAddress, string Password)
        {

            if (!String.IsNullOrEmpty(EmailAddress) || !String.IsNullOrEmpty(Password))
            {
                var member = _db.Members.Where(s => s.EmailAddress.Equals(EmailAddress) && s.Password.Equals(Password));
                if (data.Count() > 0)
                {

                    HttpContext.Session.SetString("memberUser", member.FirstOrDefault().EmailAddress);

                    return RedirectToAction("Edit", new { id = member.FirstOrDefault().Id});
                }
                else
                {
                    
                    return RedirectToAction("Login");
                }
            }
            return View();
        }

        [Route("Member/Details/{id}")]
        public IActionResult Details(int id)
        {
            if (HttpContext.Session.GetString("AdminUser") != null)
            {
                var member = _db.Members.FirstOrDefault(x => x.Id == id);

                return View(member);
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }



        public IActionResult Edit(int id)
        {

            var member = _db.Members.FirstOrDefault(x => x.Id == id);
      
            return View(member);

        }

        public IActionResult Delete(int id)
        {

            var member = _db.Members.FirstOrDefault(x => x.Id == id);
            _db.Members.Remove(member);

            return RedirectToAction("Members");
        }

        public IActionResult Update(Member member)
        {

            if (!ModelState.IsValid)
                return RedirectToAction("Edit", new {id = member.Id });

            _db.Members.Update(member);
            _db.SaveChanges();

            return RedirectToAction("Edit", new { id = member.Id });
        }

        public IActionResult ThankYou()
        {
            return View();
        }
    }
}
