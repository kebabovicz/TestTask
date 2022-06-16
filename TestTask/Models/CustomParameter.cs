using System;

namespace TestTask.Models
{
    public class CustomParameter : Entity
    {
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
