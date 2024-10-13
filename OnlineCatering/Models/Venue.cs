using System;
using System.Collections.Generic;

namespace OnlineCatering.Models
{
    public partial class Venue
    {
        public Venue()
        {
            Bookings = new HashSet<Booking>();
        }

        public int VenueId { get; set; }
        public string? VenueName { get; set; }
        public int? CatererVenue { get; set; }

        public virtual UserInfo? CatererVenueNavigation { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
