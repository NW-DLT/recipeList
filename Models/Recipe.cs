﻿using System.ComponentModel.DataAnnotations;
using recipeList.Models;

namespace recipeList.Models
{
    public class Recipe
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public User user { get; set; }
        public List<Product> products { get; set; }
        public List<Like> likes { get; set; }
        public List<comment> comments { get; set; }
    }
}
