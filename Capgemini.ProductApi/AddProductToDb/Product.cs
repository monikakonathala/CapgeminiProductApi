using System;
using System.Collections.Generic;
using System.Text;

namespace AddProdcttoDb
{
    public class Product
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
    }
}