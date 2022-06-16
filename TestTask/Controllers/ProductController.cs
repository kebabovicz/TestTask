using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTask.DTOs;
using TestTask.Models;

namespace TestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DbContext _context;

        public ProductController(DbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetList()
        {
            var listData = await _context.Products.ToListAsync();

            var result = new List<ProductDto>();
             
            foreach (var item in listData)
            {
                var productParameterDtos = new List<ProductParameterDto>();

                var parameters = _context.ProductParameters.Where(x => x.ProductId == item.Id).ToList();

                productParameterDtos = parameters.Select(x => new ProductParameterDto
                {
                    Name = x.CustomParameter.Name,
                    Value = x.Value
                }).ToList();

                result.Add(new ProductDto 
                {
                   ProductName = item.ProductName,
                   Url = item.Url,
                   Description = item.Description,
                   Price = item.Price,
                   Weight = item.Weight,
                   CategoryName = item.Category.CategoryName,
                   ProductParameterDtos = productParameterDtos 
                });
            }
            
            return Ok(result);
        }

        public async Task<IActionResult> Save([FromBody] ProductDto productDto)
        {
            var data = new Product();

            data.ProductName = productDto.ProductName;
            data.Description = productDto.Description;
            data.Weight = productDto.Weight;
            data.Url = productDto.Url;
            data.Price = productDto.Price;

            var category = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryName == productDto.CategoryName);
            data.CategoryId = category.Id;

            foreach(var item in productDto.ProductParameterDtos)
            {
                var parameter = await _context.ProductParameters.FirstOrDefaultAsync(x => x.CustomParameter.Name == item.Name);
                parameter.Value = item.Value;
            }

            await _context.Products.AddAsync(data);

            await _context.SaveChangesAsync();
            
            return Ok();
        }
    }
}
