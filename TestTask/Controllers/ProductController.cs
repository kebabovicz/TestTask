using Mapster;
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

            var parameters = _context.ProductParameters.ToList();

            foreach (var item in listData)
            {
                var productParameterDtos = new List<ProductParameterDto>();

                var productParameters = parameters.Where(x => x.ProductId == item.Id).ToList();

                var product = item.Adapt<ProductDto>();
                product.ProductParameterDtos = productParameters.Adapt<List<ProductParameterDto>>();

                result.Add(product);
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
