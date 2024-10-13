using System;
using System.Collections.Generic;

namespace OnlineCatering.Models
{
    public partial class Booking
    {
        public int BookingId { get; set; }
        public int? BookingCaterer { get; set; }
        public int? BookingVenue { get; set; }
        public int? BookingMenu { get; set; }
        public int? BookingPrice { get; set; }
        public int? TotalGuest { get; set; }
        public DateTime? BookingDate { get; set; }

        public virtual UserInfo? BookingCatererNavigation { get; set; }
        public virtual Menu? BookingMenuNavigation { get; set; }
        public virtual Venue? BookingVenueNavigation { get; set; }
    }
}
