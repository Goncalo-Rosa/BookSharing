using System.ComponentModel.DataAnnotations;

namespace BookSharing.Models.ViewModels
{
    public class CreateContactViewModel
    {

        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        [RegularExpression("^[a-zA-Z\\u00C0-\\u00FF ]*$", ErrorMessage = "Full Name can only contain letters")]
        public string FullName { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z\\u00C0-\\u00FF ,.!?]*$", ErrorMessage = "Message can only contain letters")]
        public string Message { get; set; }

    }
}