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

            var parameters = await _context.ProductParameters
                .Where(x => listData.Any(c => c.Id == x.ProductId))
                .ToListAsync();

            var parametersList = parameters
                .GroupBy(x => x.ProductId)
                .ToDictionary(d => d.Key, g => g.ToList());

            var result = listData
                .Select(item =>
                {
                    var productDto = item.Adapt<ProductDto>();
                    if(parametersList.TryGetValue(item.Id, out var customParameters))
                    {
                        productDto.ProductParameterDtos = customParameters.Adapt<List<ProductParameterDto>>();
                    }
                    return productDto;
                        
                })
                .ToList();

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

            var category = await _context.Categories.SingleOrDefaultAsync(x => x.CategoryName == productDto.CategoryName);
            data.CategoryId = category.Id;

            var parameterNames = productDto.ProductParameterDtos.Select(p => p.Name);
            var productParameters = await _context.ProductParameters
                .Include(p => p.CustomParameter)
                .Where(p => parameterNames.Contains(p.CustomParameter.Name))
                .ToListAsync();

            foreach (var item in productDto.ProductParameterDtos)
            {
                var parameter = productParameters.SingleOrDefault(p => p.CustomParameter.Name == item.Name);
                if (parameter != null)
                {
                    parameter.Value = item.Value;
                    _context.ProductParameters.Add(parameter);
                }
            }

            await _context.Products.AddRangeAsync(data);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
