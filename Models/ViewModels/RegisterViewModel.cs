using System.ComponentModel.DataAnnotations;

namespace BookSharing.Models.ViewModels
{
    public class RegisterViewModel
    {

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Confirm Phone Number")]
        [Compare("PhoneNumber")]
        public string ConfirmPhoneNumber { get; set; }
    }
}