using System;
using System.Collections.Generic;

namespace OnlineCatering.Models
{
    public partial class Menu
    {
        public Menu()
        {
            Bookings = new HashSet<Booking>();
        }

        public int MenuId { get; set; }
        public string? MenuName { get; set; }
        public string? MenuDescription { get; set; }
        public string? MenuImage { get; set; }
        public int? MenuPrice { get; set; }
        public int? CatererMenu { get; set; }
        public int? MenuCat { get; set; }

        public virtual UserInfo? CatererMenuNavigation { get; set; }
        public virtual Category? MenuCatNavigation { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
