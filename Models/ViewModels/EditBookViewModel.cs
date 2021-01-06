using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BookSharing.Models.ViewModels
{
    public class EditBookViewModel : CreateBookViewModel
    {
        [Required]
        public int BookId { get; set; }

        [Display(Name = "Foto Atual")]
        public string ExistingPhotoPath { get; set; }
    }
}