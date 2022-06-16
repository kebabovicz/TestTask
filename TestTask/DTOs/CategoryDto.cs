using System.Collections.Generic;

namespace TestTask.DTOs
{
    public class CategoryDto
    {
        public string Name { get; set; }

        public List<ProductParameterDto> ProductParameterDtos = new List<ProductParameterDto>();
    }
}
