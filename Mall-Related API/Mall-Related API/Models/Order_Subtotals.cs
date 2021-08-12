using System;
using System.Collections.Generic;

#nullable disable

namespace Mall_Related_API.Models
{
    public partial class Order_Subtotals
    {
        public int OrderID { get; set; }
        public decimal? Subtotal { get; set; }
    }
}
