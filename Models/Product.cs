using System.ComponentModel.DataAnnotations;

namespace recipeList.Models
{
    public class Product
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        public List<Recipe> Recipes { get; set; }
    }
}
