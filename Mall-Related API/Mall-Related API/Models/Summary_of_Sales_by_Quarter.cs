﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Mall_Related_API.Models
{
    public partial class Summary_of_Sales_by_Quarter
    {
        public DateTime? ShippedDate { get; set; }
        public int OrderID { get; set; }
        public decimal? Subtotal { get; set; }
    }
}
