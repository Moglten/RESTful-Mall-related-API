using System;
using System.Collections.Generic;

#nullable disable

namespace Mall_Related_API.Models
{
    public partial class Quarterly_Orders
    {
        public string CustomerID { get; set; }
        public string CompanyName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
