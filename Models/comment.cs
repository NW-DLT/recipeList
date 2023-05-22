using System.ComponentModel.DataAnnotations;

namespace recipeList.Models
{
    public class comment
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string message { get; set; }
        public User user { get; set; }
        public Recipe recipe { get; set; }
    }
}
