using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BookSharing.Models.ViewModels
{
    public class CreateBookViewModel
    {

        [Required]
        public string EmailCreator { get; set; }

        [Required]
        [Display(Name = "Book Name")]
        [RegularExpression("^[a-zA-Z\\u00C0-\\u00FF ]*$", ErrorMessage = "Book Name can only contain letters")]
        public string BookName { get; set; }

        [Required]
        [Display(Name = "Book Description")]
        [RegularExpression("^[a-zA-Z\\u00C0-\\u00FF ]*$", ErrorMessage = "Book Description can only contain letters")]
        public string BookDescription { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z\\u00C0-\\u00FF ]*$", ErrorMessage = "Author can only contain letters")]
        public string Author { get; set; }

        [Required]
        // [MinLength(, ErrorMessage = "Contact can only have 9 digits")]
        public int Contact { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string genero { get; set; }

        [Required]
        [Display(Name = "Language")]
        public string idioma { get; set; }

        [DefaultValue(false)]
        [Display(Name = "Is Rented")]
        public bool isRented { get; set; }


        [Required]
        public IFormFile Photo { get; set; }
    }
}