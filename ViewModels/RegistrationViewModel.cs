using System.ComponentModel.DataAnnotations;

namespace LogixTask.ViewModels
{
    public class RegistrationViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "The password must be at least {2} characters long")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [StringLength(100,MinimumLength = 3,ErrorMessage = "First Name must be at least 3  characters long")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "First Name must be at least 3  characters long")]
        public string LastName { get; set; }
        public string FullName { get; set; }
        [Required]
        public string DateOfBirth { get; set; }
        [Required]
        //[RegularExpression(@"\d{3}-\d{3}-\d{4}",ErrorMessage = "Phone number must be in format (999) 999 9999")]
        public long PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
