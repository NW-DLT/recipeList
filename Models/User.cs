using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace recipeList.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public List<Subscribe> subscribers {  get; set; }
    }
}
