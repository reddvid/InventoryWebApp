using ASPNETWebApp48.Models;
using System.Collections.Generic;

namespace ASPNETWebApp48.ViewModels
{
    public class DashboardViewModel
    {
        public List<Product> Products { get; set; }
        public List<Releasing> Releasings { get; set; }
        //public List<RestockApprove> RestockApproves { get; set; }
        public List<Purchase> Purchases { get; set; }
        public List<Receiving> Receivings { get; set; }
    }
}