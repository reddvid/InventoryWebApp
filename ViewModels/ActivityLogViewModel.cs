using ASPNETWebApp48.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETWebApp48.ViewModels
{
    public class ActivityLogViewModel
    {
        public Product Product { get; set; }
        public ProductWarehouse ProductWarehouse { get; set; }
        public Supplier Supplier { get; set; }

        public Order Order { get; set; }

        public Purchase Purchase { get; set; }
    }
}