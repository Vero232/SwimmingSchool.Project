using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwimmingSchool.Web.Models
{
    public class Member
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "You must provide a first name")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "You must provide last name")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "You must provide date of birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "You must provide a email address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Not a valid email address")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password Incorrect")]
        public string ConfirmPassword { get; set; }
      

        [Required(ErrorMessage = "You must provide a phone number")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]      
        public string PhoneNumber { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}
