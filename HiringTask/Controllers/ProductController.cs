using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HiringTask.Models;
using HiringTask.Models.Data;
using HiringTask.DTO;
using System.Linq;
namespace HiringTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly TaskContext _context;
    
            public ProductController(TaskContext context)
        {
                _context = context;
            }


        [HttpPost]
        public IActionResult AddProduct(ProductCategoryDTO productCategoryDTO)
        {
            var category = _context.Categories.Include(x => x.Products).FirstOrDefault(x => x.Id == productCategoryDTO.CategoryId);

            if (category == null)
            {
                return NotFound("Category Not Found :(");
            }

            var product = new Product
            {
                ArabicName = productCategoryDTO.ArabicName,
                EnglishName = productCategoryDTO.EnglishName,
                Price = productCategoryDTO.Price,
                Description = productCategoryDTO.Description,
                Manufacturer = productCategoryDTO.Manufacturer,
                State = productCategoryDTO.State,
                UserId = productCategoryDTO.UserId,
                CategoryId = productCategoryDTO.CategoryId
            };

            _context.Products.Add(product);
            _context.SaveChanges();

            return Ok("Product Added Successfully :)");
        }

        [HttpPut]
        public IActionResult UpdateProduct(ProductCategoryDTO productCategoryDTO)
        {
            var category = _context.Categories.Include(x => x.Products).FirstOrDefault(x => x.Id == productCategoryDTO.CategoryId);

            if (category == null)
            {
                return NotFound("Category Not Found :(");
            }

            var product = _context.Products.FirstOrDefault(x => x.Id == productCategoryDTO.Id);

            if (product == null)
            {
                return NotFound("Product Not Found :(");
            }

            product.ArabicName = productCategoryDTO.ArabicName;
            product.EnglishName = productCategoryDTO.EnglishName;
            product.Price = productCategoryDTO.Price;
            product.Description = productCategoryDTO.Description;
            product.Manufacturer = productCategoryDTO.Manufacturer;
            product.State = productCategoryDTO.State;
            product.UserId = productCategoryDTO.UserId;
            product.CategoryId = productCategoryDTO.CategoryId;

            _context.SaveChanges();

            return Ok("Product Updated Successfully :)");
        }
       
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return NotFound("Product Not Found :(");
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return Ok("Product Deleted Successfully :)");
        }

     
        [HttpGet("category/{id}")]
        public IActionResult GetAllProductsByCategoryId(int id)
        {
            var category = _context.Categories.Include(x => x.Products).FirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                return NotFound("Category Not Found :(");
            }

            var products = category.Products;

            if (products.Count == 0)
            {
                return NotFound("No Products Found :(");
            }

            return Ok(products);
        }
        //search by name for product
        [HttpGet("search/{name}")]
        public IActionResult SearchProductsByName(string name)
        {
            string lowercaseName = name.ToLower();
            var products = _context.Products.Where(x => x.EnglishName.ToLower().Contains(lowercaseName) || x.ArabicName.ToLower().Contains(lowercaseName)).ToList();

            if (products.Count == 0)
            {
                return NotFound("No Products Found :(");
            }

            return Ok(products);
        }


[HttpGet("search")]
    public IActionResult SearchProductsByCategoryIdAndName(int categoryId=0, string EnglishName = null, string ArabicName =null)
    {
        var query = _context.Products.ToList();

        if (categoryId != 0)
        {
                query = query.Where(p => p.CategoryId == categoryId).ToList();
            //query = query.Include(c=> c.Category).Where(p => p.CategoryId == categoryId).ToList();
        }

        if (!string.IsNullOrEmpty(EnglishName) && !string.IsNullOrEmpty(ArabicName))
        {
            string lowercaseName = EnglishName.ToLower();
           var result = query.Where(p =>
                p.EnglishName.ToLower().Contains(lowercaseName) &&
                p.ArabicName.ToLower().Contains(lowercaseName)

            
            );
        }
        var Products = query.ToList();

        if (Products.Count == 0)
        {
            return NotFound("No products found.");
        }

        return Ok(Products);
    }



}
}
