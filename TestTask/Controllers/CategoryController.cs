using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTask.DTOs;
using TestTask.Models;

namespace TestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly DbContext _context;

        public CategoryController(DbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> GetList()
        {
            var listData = await _context.Categories.ToListAsync();

            var parameters = await _context.CustomParameters
                .Where(x => listData.Any(c => c.Id == x.CategoryId))
                .ToListAsync();

            var parametersList = parameters
                .GroupBy(x => x.CategoryId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var result = listData
                .Select(item =>
                {
                    var categoryDto = item.Adapt<CategoryDto>();
                    if (parametersList.TryGetValue(item.Id, out var customParameters))
                    {
                        categoryDto.ProductParameterDtos = customParameters.Adapt<List<ProductParameterDto>>();
                    }
                    return categoryDto;
                })
                .ToList();

            return Ok(result);
        }

        public async Task<IActionResult> Save([FromBody]CategoryDto categoryDto)
        {
            var category = new Category();

            category.CategoryName = categoryDto.Name;

            await _context.Categories.AddAsync(category);

            foreach(var item in categoryDto.ProductParameterDtos)
            {
                var parameter = new CustomParameter
                {
                    CategoryId = category.Id,
                    Name = item.Name
                };

                await _context.CustomParameters.AddAsync(parameter);
            }

            return Ok();
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var category = _context.Categories.Find(id);

            _context.Remove(category);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
