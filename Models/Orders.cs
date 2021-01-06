using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookSharing.Models
{
    public class Orders
    {
        [Required]
        public int OrdersId { get; set; }

        public Book Book { get; set; }
    }
}