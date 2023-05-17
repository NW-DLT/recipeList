using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace recipeList.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public Image? Image { get; set; }
        public List<Recipe> Recipes { get; set; } = new List<Recipe>();
        public List<Subscribe> subscribers {  get; set; }
    }
}
