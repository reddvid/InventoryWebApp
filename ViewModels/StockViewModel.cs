using ASPNETWebApp48.Models;
using System.Collections.Generic;

namespace ASPNETWebApp48.ViewModels
{
    public class StockViewModel
    {
        public List<Product> ProductsLookup { get; set; }
        public List<ProductWarehouse> ProductWarehousesLookup { get; set; }

        //public List<WarehouseRestockPending> WarehousePendingLookup { get; set; }
        //public List<RestockPending> PendingLookup { get; set; }

        public ReleaseItem ReleaseItem { get; set; }
        public PurchaseItem PurchaseItem { get; set; }
        //public ReleaseReturn ReleaseReturn { get; set; }
        //public RestockApprove RestockApprove { get; set; }
    }
}