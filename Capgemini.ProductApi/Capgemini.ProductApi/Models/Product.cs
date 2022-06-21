using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Capgemini.ProductApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime ManufacturingDate { get; set; }
        [Range(1,double.MaxValue,ErrorMessage = "Price shoud be > 0")]
        public double Price { get; set; }
        [Required]
        public string Category { get; set; }
    }
}
