using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using recipeList.Data;
using recipeList.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace recipeList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipeController
    {
        private readonly AppDBContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        public RecipeController(AppDBContext dbContext, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public async Task<ActionResult<List<Recipe>>> getRecipes()
        {
            try
            {
                var res = await _dbContext.recipes.Include(g => g.products).ToListAsync();
                return res;
            }
            catch
            {
                return new NoContentResult();
            }
        }
        [HttpGet("getRecipe/{id}")]
        public async Task<ActionResult<Recipe>> getRecipe(int id)
        {
            try
            {
                return await _dbContext.recipes.FindAsync(id);
            }
            catch
            {
                return new NoContentResult();
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> newRecipe(Recipe recipe)
        {
            try
            {
                _dbContext.recipes.Add(recipe);
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
        public async Task<IActionResult> editRecipe(Recipe recipe)
        {
            try
            {
                _dbContext.recipes.Update(recipe);
                await _dbContext.SaveChangesAsync();
                return new OkResult();
            }
            catch
            {
                return new BadRequestResult();
            }
        }
        [HttpDelete("deleteRecipe/{id}")]
        [Authorize]
        public async Task<IActionResult> deleteRecipe(int id)
        {
            try
            {
                var recipe = await _dbContext.recipes.FindAsync(id);
                _dbContext.recipes.Remove(recipe);
                await _dbContext.SaveChangesAsync();
                return new OkResult();
            }
            catch
            {
                return new BadRequestResult();
            }
        }
        [HttpPost("LikeRecipe/{id}")]
        [Authorize]
        public async Task<IActionResult> LikeRecipe(int id)
        {
            try
            {
                var recipe = await _dbContext.recipes.FindAsync(id);
                var userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Like like = new Like();
                like.User = await _userManager.FindByIdAsync(userId);
                recipe.likes = await _dbContext.likes.Where(g => g.Id == recipe.id).ToListAsync();
                recipe.likes.Add(like);
                await _dbContext.SaveChangesAsync();
                return new OkResult();
            }
            catch
            {
                return new BadRequestResult();
            }
        }
    }
}
