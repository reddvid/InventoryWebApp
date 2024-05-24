using ASPNETWebApp48.Models;
using System.Collections.Generic;

namespace ASPNETWebApp48.ViewModels
{
    public class ReleaseReportViewModel
    {
        public List<Releasing> Releasing { get; set; }
        public List<ReleaseItem> ReleaseItems { get; set; }
        public Product Product { get; set; }
    }
}