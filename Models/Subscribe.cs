using System.ComponentModel.DataAnnotations;

namespace recipeList.Models
{
    public class Subscribe
    {
        [Key]
        public int Id { get; set; }
        public User Subscriber { get; set; }
    }
}
