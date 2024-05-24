using ASPNETWebApp48.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETWebApp48.ViewModels
{
    public class TransferViewModel
    {
        public List<TransferItemStore> StoreLookup { get; set; }
        public List<TransferItemWarehouse> WarehouseLookup { get; set; }
    }
}