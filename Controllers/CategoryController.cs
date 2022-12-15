using AspNet_Core6.Fundamentals.Data;
using AspNet_Core6.Fundamentals.Models;
using AspNet_Core6.Fundamentals.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNet_Core6.Fundamentals.Controllers
{
    [ApiController]
    [Route("v1")]
    public class CategoryController : ControllerBase
    {
        [HttpGet("categories")]
        public async Task<IActionResult> GetAsync([FromServices] BlogDataContext context)
        {
            var categories = await context.Categories.ToListAsync();

            return Ok(categories);
        }

        [HttpGet("categories/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] BlogDataContext context, [FromRoute] int id)
        {
            try
            {
                var categoryId = await context.Categories.FirstOrDefaultAsync(x => x.Id.Equals(id));

                if (categoryId == null)
                {
                    return NotFound();
                }

                return Ok(categoryId);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("categories")]
        public async Task<IActionResult> PostAsync([FromServices] BlogDataContext context, [FromBody] EditorCategoryViewModel categoryViewModel)
        {
            try
            {
                var category = new Category
                {
                    Id = 0,
                    Posts = null,
                    Name = categoryViewModel.Name,
                    Slug = categoryViewModel.Slug.ToLower(),
                };
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();
                return Created($"v1/categories/{category.Id}", category);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("categories/{id}")]
        public async Task<IActionResult> PutAsync([FromServices] BlogDataContext context, [FromRoute] int id, [FromBody] EditorCategoryViewModel categoryViewModel)
        {
            try
            {
                var categoryPut = await context.Categories.FirstOrDefaultAsync(x => x.Id.Equals(id));

                if (categoryPut == null)
                {
                    return NotFound();
                }

                categoryPut.Name = categoryViewModel.Name;
                categoryPut.Slug = categoryViewModel.Slug;

                context.Update(categoryPut);
                await context.SaveChangesAsync();

                return Ok(categoryPut);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> DeleteAsync([FromServices] BlogDataContext context, [FromRoute] int id)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id.Equals(id));

                if (category == null)
                {
                    return NotFound();
                }

                context.Remove(id);
                await context.SaveChangesAsync();
                return Ok(category);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
