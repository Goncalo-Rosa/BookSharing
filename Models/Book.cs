using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookSharing.Models
{
    public class Book
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        public string EmailCreator { get; set; }

        [Required]
        [Display(Name = "Book Name")]
        public string BookName { get; set; }

        [Required]
        [Display(Name = "Book Description")]
        public string BookDescription { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public int Contact { get; set; }

        [Required]
        public string genero { get; set; }

        [Required]
        public string idioma { get; set; }

        [DefaultValue(false)]
        public bool isRented { get; set; }

        [Display(Name = "Foto")]
        public string PhotoPath { get; set; }

        public ICollection<Orders> Orders { get; set; }
    }
}