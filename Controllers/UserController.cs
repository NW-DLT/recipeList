using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using recipeList.Data;
using recipeList.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace recipeList.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AppDBContext _dbContext;
        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, AppDBContext appDBContext, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = appDBContext;
            this.httpContextAccessor = httpContextAccessor;
        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            var user = new User { UserName = registerModel.Email, Email = registerModel.Email , Name = registerModel.Name   };
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
        [HttpPost("avatar")]
        [Authorize]
        public async Task<IActionResult> Avatar(IFormFile avatar)
        {
            try
            {
                var currentUserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var currentUser = await _userManager.FindByIdAsync(currentUserId);
                Image image = new Image() { FormFile = avatar };
                image.Src = image.getSrc(image);
                currentUser.Image = image;
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }

        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Invalid login attempt.");
            }
        }
        [HttpPost("subscribe")]
        [Authorize]
        public async Task<IActionResult> subscribe(User user)
        {
            try
            {
                var currentUserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var currentUser = await _userManager.FindByIdAsync(currentUserId);
                Subscribe subscribe = new Subscribe();
                subscribe.Subscriber = currentUser;
                user.subscribers.Add(subscribe);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

    }
    public class RegisterModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
