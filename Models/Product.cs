using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ASPNETWebApp48.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("BrandId")] public Brand Brand { get; set; }
        public int BrandId { get; set; }

        public string SKU { get; set; }

        [ForeignKey("CategoryId")] public Category Category { get; set; }
        public int CategoryId { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SellingPrice { get; set; }

        public string Unit { get; set; }

        public string PictureFilename { get; set; } // Save as image file on disk

        public int MinQty { get; set; }

        public int CeilingQty { get; set; }

        //for deduct entities
        public List<ReleaseItem> ReleaseItems { get; set; }
        [NotMapped] public decimal TotalReleaseQty => ReleaseItems?.Sum(t => t.Quantity) ?? 0;

        public List<TransferItemStore> TransferItemStores { get; set; }
        [NotMapped] public decimal TransferStoreQty => TransferItemStores?.Sum(t => t.Quantity) ?? 0;

        [NotMapped] public decimal AdjustDeductQty => AdjustItems?.Sum(t => t.QtyDeduct) ?? 0;

        public List<PurchaseReturnItem> PurchaseReturnItems { get; set; }
        [NotMapped] public decimal PoReturnQty => PurchaseReturnItems?.Sum(t => t.Quantity) ?? 0;

        public List<TransferItemWarehouse> TransferItemWarehouses { get; set; }
        [NotMapped] public decimal TransferWhQty => TransferItemWarehouses?.Sum(t => t.Quantity) ?? 0;

        //for returned entities
        //public List<ReleaseReturn> ReleaseReturns { get; set; }
        //[NotMapped] public decimal ReturnQty => ReleaseReturns?.Sum(t => t.Quantity) ?? 0;

        //public List<ReleaseReturnDmg> ReleaseReturnDmgs { get; set; }
        //[NotMapped] public decimal ReleaseDmgReturnQty => ReleaseReturnDmgs?.Sum(t => t.Quantity) ?? 0;

        //adjust product
        public List<AdjustItem> AdjustItems { get; set; }
        [NotMapped] public decimal AdjustAddQty => AdjustItems?.Sum(t => t.QtyAdd) ?? 0;

        [NotMapped] public decimal TotalReceive => TransferWhQty + AdjustAddQty;

        [NotMapped] public decimal TotalOrder => TotalReleaseQty + TransferStoreQty + AdjustDeductQty;

        [NotMapped] public decimal StockOnHand => TotalReceive - TotalOrder + TotalReturn;

        [NotMapped] public decimal AddReturn => PoReturnQty /*+ ReturnQty + ReleaseDmgReturnQty*/;

        [NotMapped] public decimal TotalReturn => AddReturn;

    }

    public class ProductWarehouse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("BrandId")] public Brand Brand { get; set; }
        public int BrandId { get; set; }

        [ForeignKey("CategoryId")] public Category Category { get; set; }
        public int CategoryId { get; set; }

        public decimal PurchasePrice { get; set; }

        public string Unit { get; set; }

        public string PictureFilename { get; set; } // Save as image file on disk

        public int MinQty { get; set; }

        public int CeilingQty { get; set; }

        //restock
        public List<ReceiveApproveItem> ReceivingApproveItems { get; set; }
        [NotMapped] public decimal ReceivingQty => ReceivingApproveItems?.Sum(t => t.Quantity) ?? 0;

        public List<TransferItemStore> TransferItemStores { get; set; }
        [NotMapped] public decimal TransferStoreQty => TransferItemStores?.Sum(t => t.Quantity) ?? 0;

        //deduct
        public List<TransferItemWarehouse> TransferItemWarehouses { get; set; }
        [NotMapped] public decimal TransferWhQty => TransferItemWarehouses?.Sum(t => t.Quantity) ?? 0;

        public List<PurchaseReturnItem> PurchaseReturnItems { get; set; }
        [NotMapped] public decimal PoReturnQty => PurchaseReturnItems?.Sum(t => t.Quantity) ?? 0;

        //adjust product
        public List<AdjustItem> AdjustItems { get; set; }
        [NotMapped] public decimal AdjustAddQty => AdjustItems?.Sum(t => t.QtyAdd) ?? 0;
        [NotMapped] public decimal AdjustDeductQty => AdjustItems?.Sum(t => t.QtyDeduct) ?? 0;

        [NotMapped] public decimal Restock => ReceivingQty + TransferStoreQty + AdjustAddQty;
        [NotMapped] public decimal Deduct => PoReturnQty + TransferWhQty + AdjustDeductQty;
        [NotMapped] public decimal StockOnHand => Restock - Deduct;
    }

    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class Brand
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class ProductSupplier
    {
        public int Id { get; set; }

        [ForeignKey("ProductId")] public Product Product { get; set; }
        public int ProductId { get; set; }

        [ForeignKey("SupplierId")] public Supplier Supplier { get; set; }
        public int SupplierId { get; set; }
    }

    public class Adjustment
    {
        public int Id { get; set; }

        [ForeignKey("LocationId")] public Location Locations { get; set; }
        public int LocationId { get; set; }

        public DateTime AdjustmentDate { get; set; }

        public string User { get; set; }

        public List<AdjustItem> AdjustItems { get; set; }
    }

    public class AdjustItem
    {
        public int Id { get; set; }

        [ForeignKey("AdjustmentId")] public Adjustment Adjustments { get; set; }
        public int AdjustmentId { get; set; }

        [ForeignKey("ProductId")] public Product Product { get; set; }
        public int? ProductId { get; set; }

        [ForeignKey("WarehouseId")] public ProductWarehouse ProductWarehouse { get; set; }
        public int? WarehouseId { get; set; }

        public int QtyAdd { get; set; }

        public int QtyDeduct { get; set; }
    }

    public class Location
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}