using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using recipeList.Data;
using recipeList.Models;

namespace recipeList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController
    {
        private readonly AppDBContext _dbContext;
        public ProductsController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            return await _dbContext.products.ToListAsync();
        }
        [HttpGet("getProduct/{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            try
            {
                return await _dbContext.products.FindAsync(id);
            }
            catch
            {
                return new NotFoundResult();
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> newProduct(Product product)
        {
            try
            {
                _dbContext.products.Add(product);
                await _dbContext.SaveChangesAsync();
                return new OkResult();
            }
            catch
            {
                return new BadRequestResult();
            }
        }
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> editProduct(Product product)
        {
            try
            {
                _dbContext.products.Update(product);
                await _dbContext.SaveChangesAsync();
                return new OkResult();
            }
            catch
            {
                return new BadRequestResult();
            }
        }
        [HttpDelete("deleteProduct/{id}")]
        [Authorize]
        public async Task<IActionResult> deleteProduct(int id)
        {
            try
            {
                var product = await _dbContext.products.FindAsync(id);
                _dbContext.products.Remove(product);
                await _dbContext.SaveChangesAsync();
                return new OkResult();
            }
            catch { return new BadRequestResult(); }
        }
    }
}
