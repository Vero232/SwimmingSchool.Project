using System.ComponentModel.DataAnnotations.Schema;

namespace SwimmingSchool.Web.Models
{
    public class AttendanceRecord
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Key { get; set; }

        public DateTime AttendanceDate { get; set; }

    }
}
