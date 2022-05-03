using Microsoft.EntityFrameworkCore;
using SwimmingSchool.Web.Models;
namespace SwimmingSchool.Web.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

    }
}
