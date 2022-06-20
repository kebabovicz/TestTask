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

            var result = new List<CategoryDto>();

            var parameters = _context.CustomParameters.ToList();

            foreach (var item in listData)
            {               
                var productParameterDtos = new List<ProductParameterDto>();

                var customParameters = parameters.Where(x => x.CategoryId == item.Id).ToList();

                var category = item.Adapt<CategoryDto>();
                category.ProductParameterDtos = customParameters.Adapt<List<ProductParameterDto>>();

                result.Add(category);
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
