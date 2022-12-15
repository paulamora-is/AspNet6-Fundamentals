using AspNet_Core6.Fundamentals.Data;
using AspNet_Core6.Fundamentals.Extensions;
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
            try
            {
                var categories = await context.Categories.ToListAsync();
                return Ok(value: new ResultViewModel<List<Category>>(categories));
            }
            catch
            {
                return StatusCode(500, value: new ResultViewModel<List<Category>>(error: "Internal Server Failure."));
            }
        }

        [HttpGet("categories/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] BlogDataContext context, [FromRoute] int id)
        {
            try
            {
                var categoryById = await context.Categories.FirstOrDefaultAsync(x => x.Id.Equals(id));

                if (categoryById == null)
                {
                    return NotFound(value: new ResultViewModel<Category>(error: "Category not found."));
                }

                return Ok(new ResultViewModel<Category>(categoryById));
            }
            catch
            {
                return StatusCode(500, value: new ResultViewModel<Category>(error: "Internal Server Failure."));
            }
        }

        [HttpPost("categories")]
        public async Task<IActionResult> PostAsync([FromServices] BlogDataContext context, [FromBody] EditorCategoryViewModel categoryViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(error: new ResultViewModel<Category>(ModelState.GetErrors()));

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

                return Created($"v1/categories/{category.Id}", value: new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, value: new ResultViewModel<Category>(error: "Unable to add category."));
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
                    return NotFound(value: new ResultViewModel<Category>(error: "Category not found."));
                }

                categoryPut.Name = categoryViewModel.Name;
                categoryPut.Slug = categoryViewModel.Slug;

                context.Update(categoryPut);
                await context.SaveChangesAsync();

                return Ok(value: new ResultViewModel<Category>(categoryPut));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>(error: "Unable to update category"));
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
                    return NotFound(value: new ResultViewModel<Category>(error: "Category not found."));
                }

                context.Remove(id);
                await context.SaveChangesAsync();

                return Ok(value: new ResultViewModel<Category>(category));
            }
            catch
            {
                return StatusCode(500, value: new ResultViewModel<Category>(error: "Unable to delete category"));
            }
        }
    }
}
