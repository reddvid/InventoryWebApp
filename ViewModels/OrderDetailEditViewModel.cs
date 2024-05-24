using ASPNETWebApp48.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETWebApp48.ViewModels
{
    public class OrderDetailEditViewModel
    {
        public Order Orders { get; set; }
        public OrderItem OrderItems { get; set; }
        public List<District> DistrictLookups { get; set; }
    }
}