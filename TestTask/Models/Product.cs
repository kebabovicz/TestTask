using System;

namespace TestTask.Models
{
    public class Product : Entity
    {
        public string ProductName { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Weight { get; set; }
        public virtual Category Category { get; set; }
        public Guid CategoryId { get; set; }
    }
}
