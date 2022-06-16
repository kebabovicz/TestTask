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

            var result = new List<CategoryDto>();

            foreach (var item in listData)
            {
                var parameters = _context.CustomParameters.Where(x => x.CategoryId == item.Id).ToList();

                var productParameterDtos = new List<ProductParameterDto>();

                productParameterDtos = parameters.Select(x => new ProductParameterDto
                {
                    Name = x.Name
                }).ToList();

                result.Add(new CategoryDto
                {
                    Name = item.CategoryName,
                    ProductParameterDtos = productParameterDtos
                });
            }

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

            return Ok();
        }
    }
}
