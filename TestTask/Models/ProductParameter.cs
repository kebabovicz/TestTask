using System;

namespace TestTask.Models
{
    public class ProductParameter : Entity
    {
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
        public Guid CustomAttributeId { get; set; }
        public virtual CustomParameter CustomParameter { get; set; }
        public string Value { get; set; }
    }
}
