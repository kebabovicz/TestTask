using System.Collections.Generic;

namespace TestTask.Models
{
    public class Category : Entity
    {
        public string CategoryName { get; set; }
        public virtual List<CustomParameter> CustomParameters { get; set; }
        public virtual List<Product> Products { get; set; }  

    }
}
