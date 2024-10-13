using System;
using System.Collections.Generic;

namespace OnlineCatering.Models
{
    public partial class UserInfo
    {
        public UserInfo()
        {
            Bookings = new HashSet<Booking>();
            Menus = new HashSet<Menu>();
            Venues = new HashSet<Venue>();
        }

        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserImage { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPassword { get; set; }
        public string? UserAddress { get; set; }
        public long? UserPhoneNo { get; set; }
        public string? UserRole { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
        public virtual ICollection<Venue> Venues { get; set; }
    }
}
