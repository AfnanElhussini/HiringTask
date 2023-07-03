using HiringTask.DTO;
using HiringTask.Models;
using HiringTask.Models.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HiringTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly TaskContext _context;

        public CategoryController(TaskContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _context.Categories.Include(x => x.Products).ToList();

            if (categories.Count == 0)
            {
                return NotFound("Don't Have Any Categories :(");
            }

            var categoryProductDTOs = categories.Select(category => new CategoryProductDTO
            {
                Id = category.Id,
                ArabicName = category.ArabicName,
                EnglishName = category.EnglishName,
                StartDate = category.StartDate,
                EndDate = category.EndDate,
                State = category.State,
                Products = category.Products.Select(product => product.ArabicName).ToList()

       
            }).ToList();

            return Ok(categoryProductDTOs);
        }



        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _context.Categories.Include(x => x.Products).FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return NotFound("Category Not Found :(");
            }

            var categoryProductDTO = new CategoryProductDTO
            {
                Id = category.Id,
                ArabicName = category.ArabicName,
                EnglishName = category.EnglishName,
                StartDate = category.StartDate,
                EndDate = category.EndDate,
                State = category.State,
          Products = category.Products.Select(product => product.ArabicName).ToList()

            };

            return Ok(categoryProductDTO);
        }

        [HttpPost]
        public IActionResult Create(CategoryProductDTO categoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = new Category
            {
                ArabicName = categoryDTO.ArabicName,
                EnglishName = categoryDTO.EnglishName,
                StartDate = categoryDTO.StartDate,
                EndDate = categoryDTO.EndDate,
                State = categoryDTO.State
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            return Ok("Category Created Successfully :)");
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CategoryProductDTO categoryDTO)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return NotFound("Category Not Found :(");
            }

            category.ArabicName = categoryDTO.ArabicName;
            category.EnglishName = categoryDTO.EnglishName;
            category.StartDate = categoryDTO.StartDate;
            category.EndDate = categoryDTO.EndDate;
            category.State = categoryDTO.State;

            _context.SaveChanges();

            return Ok("Category Updated Successfully :)");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return NotFound("Category Not Found :(");
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return Ok("Category Deleted Successfully :)");
        }
    }
}
