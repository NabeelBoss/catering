using System;
using System.Collections.Generic;

namespace OnlineCatering.Models
{
    public partial class Category
    {
        public Category()
        {
            Menus = new HashSet<Menu>();
        }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryImage { get; set; }

        public virtual ICollection<Menu> Menus { get; set; }
    }
}
