using System;
using System.Collections.Generic;

namespace OnlineCatering.Models
{
    public partial class OrderStatus
    {
        public OrderStatus()
        {
            Bookings = new HashSet<Booking>();
        }

        public int OrderStatusId { get; set; }
        public string? OrderStatusName { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
