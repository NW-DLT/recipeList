using System.ComponentModel.DataAnnotations;
using recipeList.Models;

namespace recipeList.Models
{
    public class Like
    {
        [Key]
        public int Id { get; set; }
        public User User { get; set; }
        public Recipe Recipe { get; set; }
    }
}
