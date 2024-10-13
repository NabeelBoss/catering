namespace OnlineCatering.Models
{
    public class BookingViewModel
    {
        public Booking BookingInfo { get; set; }
        public List<Menu> Menus { get; set; }
        public List<Venue> Venues { get; set; }
    }
}
