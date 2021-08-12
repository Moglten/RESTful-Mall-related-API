using System;
using System.Collections.Generic;

#nullable disable

namespace Mall_Related_API.Models
{
    public partial class ProductsAboveAveragePrice
    {
        public string ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
