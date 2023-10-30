using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LogixTask.Models
{
    public class WebUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public long PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
