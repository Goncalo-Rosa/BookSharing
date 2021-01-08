using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookSharing.Models.ViewModels
{
    public class CreateOrder
    {
        [Required]
        public string RequisitorEmail { get; set; }

        [Required]
        public int BookId { get; set; }

        public Book Book { get; set; }
    }
}