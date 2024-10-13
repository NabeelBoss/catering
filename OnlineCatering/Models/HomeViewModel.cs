namespace OnlineCatering.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Category> categoryTable { get; set; }
        public IEnumerable<Menu> menuTable { get; set; }
        public IEnumerable<UserInfo> userTable { get; set; }

    }
}
