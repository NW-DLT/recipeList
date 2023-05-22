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
                                                  .Include(g => g.likes)
                                                  .Include(g => g.comments)
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
                                                  .Include(g => g.likes)
                                                  .Include(g => g.comments)
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
                                                     .Include(g => g.likes)
                                                     .Include(g => g.comments)
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

        [HttpPost("CommentRecipe/{id}")]
        [Authorize]
        public async Task<IActionResult> CommentRecipe(int id, string message)
        {
            try
            {
                var recipe = await _dbContext.recipes.FindAsync(id);
                var userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                comment comment = new comment()
                {
                    message = message,
                    user = await _userManager.FindByIdAsync(userId)
                };

                recipe.comments = await _dbContext.comments.Where(g => g.recipe.id == recipe.id).ToListAsync();
                recipe.comments.Add(comment);
                await _dbContext.SaveChangesAsync();
                return new OkResult();
            }
            catch
            {
                return new BadRequestResult();
            }
        }
        [HttpDelete("DeleteLike/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteLike(int id)
        {
            try
            {
                Like like = await _dbContext.likes.Include(i => i.User).FirstOrDefaultAsync(g => g.Id == id);
                if(like.User.Id != httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    return new BadRequestResult();
                }
                _dbContext.likes.Remove(like);
                await _dbContext.SaveChangesAsync();
                return new OkResult();
            }
            catch
            {
                return new BadRequestResult();
            }
        }
        [HttpDelete("DeleteComment/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                comment comment = await _dbContext.comments.Include(i => i.user).FirstOrDefaultAsync(g => g.id == id);
                if (comment.user.Id != httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    return new BadRequestResult();
                }
                _dbContext.comments.Remove(comment);
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
