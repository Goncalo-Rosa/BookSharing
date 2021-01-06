using System.ComponentModel.DataAnnotations;

namespace BookSharing.Models.ViewModels
{
    public class EditRoleViewModel
    {
        public string Id { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z\\u00C0-\\u00FF ]*$", ErrorMessage = "Name can only contain letters")]
        public string Name { get; set; }
    }
}