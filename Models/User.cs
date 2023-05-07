using System.ComponentModel.DataAnnotations;

namespace recipeList.Models
{
    public class User
    {
        [Key]
        public string id {get; set;}
        public List<Recipe> recipes { get; set; }
    }
}
