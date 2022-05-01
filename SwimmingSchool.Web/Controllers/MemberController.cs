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

        [Route("Registration")]
        public IActionResult RegisterMember(Member Member)
        {
           
            if (!ModelState.IsValid)
                return View();

            
            Member.RegistrationDate = DateTime.Today;

            _db.Members.Add(Member);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [Route("Members")]
        public IActionResult Members()
        {
            var members = _db.Members.ToList();

            return View(members);
        }

        [Route("Member/Details/{id}")]
        public IActionResult Details (int id)
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
    }
}
