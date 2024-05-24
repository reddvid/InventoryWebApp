using ASPNETWebApp48.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ASPNETWebApp48.ViewModels
{
    public class QtyOnHandViewModel
    {
        public List<Product> Products { get; set; }
        public List<ProductWarehouse> ProductWarehouses { get; set; }
        public List<MergedProduct> MergedProducts { get; set; }

        //for deduct entities
        public List<ReleaseItem> ReleaseItems { get; set; }
        [NotMapped] public decimal TotalReleaseQty => ReleaseItems?.Sum(t => t.Quantity) ?? 0;

        public List<TransferItemStore> TransferItemStores { get; set; }
        [NotMapped] public decimal TransferStoreQty => TransferItemStores?.Sum(t => t.Quantity) ?? 0;

        [NotMapped] public decimal AdjustDeductQty => AdjustItems?.Sum(t => t.QtyDeduct) ?? 0;

        //for receive entities
        public List<PurchaseReturnItem> PurchaseReturnItems { get; set; }
        [NotMapped] public decimal PoReturnQty => PurchaseReturnItems?.Sum(t => t.Quantity) ?? 0;

        public List<TransferItemWarehouse> TransferItemWarehouses { get; set; }
        [NotMapped] public decimal TransferWhQty => TransferItemWarehouses?.Sum(t => t.Quantity) ?? 0;

        //adjust product
        public List<AdjustItem> AdjustItems { get; set; }
        [NotMapped] public decimal AdjustAddQty => AdjustItems?.Sum(t => t.QtyAdd) ?? 0;

        public List<ReceiveApproveItem> ReceivingApproveItems { get; set; }
        [NotMapped] public decimal ReceivingQty => ReceivingApproveItems?.Sum(t => t.Quantity) ?? 0;

        //Main Store Entities
        [NotMapped] public decimal TotalReceiveMain => TransferWhQty + AdjustAddQty;

        [NotMapped] public decimal TotalOrderMain => TotalReleaseQty + TransferStoreQty + AdjustDeductQty;

        [NotMapped] public decimal StockOnHandMain => TotalReceiveMain - TotalOrderMain;

        //Warehouse entities
        [NotMapped] public decimal RestockWarehouse => ReceivingQty + TransferStoreQty + AdjustAddQty;
        [NotMapped] public decimal DeductWarehouse => PoReturnQty + TransferWhQty + AdjustDeductQty;
        [NotMapped] public decimal StockOnHandWarehouse => RestockWarehouse - DeductWarehouse;

        [NotMapped] public decimal StockOnHand => StockOnHandMain + StockOnHandWarehouse;
    }

    public class MergedProduct
    {
        public string Name { get; set; }
        public decimal StockOnHandMain { get; set; }
        public decimal StockOnHandWarehouse { get; set; }
        public decimal TotalStockOnHand { get; set; }
    }
}