using System.Collections.Generic;

namespace TestTask.DTOs
{
    public class ProductDto
    {
        public string ProductName { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Weight { get; set; }
        public string CategoryName { get; set; }
        public List<ProductParameterDto> ProductParameterDtos { get; set; } = new List<ProductParameterDto>();
    }
}
