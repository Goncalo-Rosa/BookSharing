using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookSharing.Models
{
    public class AcceptedOrders
    {
        [Required]
        public int AcceptedOrdersId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string BookName { get; set; }

        public Book Book { get; set; }
    }
}