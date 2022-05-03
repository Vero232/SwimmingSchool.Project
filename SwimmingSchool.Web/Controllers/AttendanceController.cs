using Microsoft.AspNetCore.Mvc;
using SwimmingSchool.Web.Models;
using Newtonsoft.Json;

namespace SwimmingSchool.Web.Controllers
{

    public class AttendanceController : Controller
    {
        private readonly AppDbContext _db;

        public AttendanceController(AppDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
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

        public IActionResult RecordAttendance(AttendanceRecord AttendanceRecord)
        {

            if (!ModelState.IsValid)
                return View();

            try
            {
            
             var attendanceRecord =  _db.AttendanceRecords.Where(x => x.AttendanceDate == AttendanceRecord.AttendanceDate).FirstOrDefault(x => x.Key == AttendanceRecord.Key);
             if(attendanceRecord == null)
                _db.AttendanceRecords.Add(AttendanceRecord);
                _db.SaveChanges();
                return Json(new { success = true, responseText = "Success" });
            }
            catch (Exception ex) {

                return Json(new { success = false, responseText = ex.Message});
            }
      
        }

        public IActionResult AttendanceReports(DateTime? start)
        {
            if (HttpContext.Session.GetString("AdminUser") != null)
            {
                var model = _db.AttendanceRecords
                .Join(_db.Members, p => p.Key, n => n.Id,
                    ((record, member) => new { record, member }))
                .AsEnumerable()
                .Select(x => Tuple.Create(x.record, x.member))
                .GroupBy((x => x.Item1.AttendanceDate)).Take(3)
                .ToList();

                if (start != null)
                    model = model.Where(x => x.Key == start).ToList();

                return View(model);
            }
            else {
                return RedirectToAction("Login", "Admin");
            }


        }

        public IActionResult RemoveAttendance(int id, DateTime? date) {

            var removeAttendent = _db.AttendanceRecords.Where(x => x.AttendanceDate == date).FirstOrDefault(x => x.Key == id);
            _db.AttendanceRecords.Remove(removeAttendent);
            _db.SaveChanges();

            return RedirectToAction("AttendanceReports");
        }

        [Route("Member/Details/{id}")]
        public IActionResult Details(int id)
        {
            if (HttpContext.Session.GetString("AdminUser") != null)
            {
                var member = _db.Members.FirstOrDefault(x => x.Id == id);

                return View(member);
            }
            else {
                return RedirectToAction("Login", "Admin");
            }
        }
        

    }
}
