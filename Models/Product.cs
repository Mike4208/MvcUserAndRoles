using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcUserAndRoles.Models
{
    public enum Category
    {
        Smartphone, Smartwatch, Tablet, Laptop
    }
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public Category Category { get; set; }
    }
}