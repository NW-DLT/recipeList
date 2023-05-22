using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using recipeList.Data;
using recipeList.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.Json;

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
        public async Task<ActionResult<string>> getRecipes()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                var res = await _dbContext.recipes.Include(g => g.user)
                                                  .Include(g => g.products)
                                                  .OrderByDescending(g => g.id)
                                                  .ToListAsync();

                var jsonString = JsonSerializer.Serialize(res, options);

                return jsonString;
            }
            catch
            {
                return new NoContentResult();
            }
        }
        [HttpGet("getUserRecipes/{userId}")]
        public async Task<ActionResult<string>> getRecipes(string userId)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                var res = await _dbContext.recipes.Include(g => g.user)
                                                  .Include(g => g.products)
                                                  .Where(g => g.user.Id == userId)
                                                  .ToListAsync();

                var jsonString = JsonSerializer.Serialize(res, options);

                return jsonString;
            }
            catch
            {
                return new NoContentResult();
            }
        }
        [HttpGet("getRecipe/{id}")]
        public async Task<ActionResult<string>> getRecipe(int id)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                var recipe = await _dbContext.recipes.Include(g => g.user)
                                                    .FirstOrDefaultAsync(o => o.id == id);

                if (recipe == null)
                {
                    return new NotFoundResult();
                }

                var jsonString = JsonSerializer.Serialize(recipe, options);

                return jsonString;
            }
            catch
            {
                return new NoContentResult();
            }
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> newRecipe(newRecipeModel model)
        {
            try
            {
                var userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);
                Recipe recipe = new Recipe()
                {
                    name = model.Name,
                    description = model.Discription
                };

                _dbContext.recipes.Add(recipe);
                user.Recipes.Add(recipe);
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

        public class newRecipeModel
        {
            [Required]
            public string Name { get; set; }
            [Required]
            public string Discription { get; set; }
        }
    }
}
