using System.ComponentModel.DataAnnotations;

namespace E_Administration.Models
{
    public class LoginModel
    {
        [Display(Name = "Enter your Email Address"), Required]
        public string? Email { get; set; }
        [Display(Name = "Enter your Password"), Required]
        public string? Password { get; set; }
    } 
}
