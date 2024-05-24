using ASPNETWebApp48.Models;
using System.Collections.Generic;

namespace ASPNETWebApp48.ViewModels
{
    public class OrderReportViewModel
    {
        public List<Order> Orders { get; set; }
        public District Districts { get; set; }
    }
}