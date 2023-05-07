﻿using System.ComponentModel.DataAnnotations;
using recipeList.Models;

namespace recipeList.Models
{
    public class Recipe
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public List<Product> products { get; set; }
    }
}