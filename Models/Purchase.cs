using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ASPNETWebApp48.Models
{
    public class Purchase
    {
        public int Id { get; set; }

        public string PurchaseNo { get; set; }

        [ForeignKey("SupplierId")] public Supplier Supplier { get; set; }
        public int SupplierId { get; set; }

        public DateTime OrderDate { get; set; }

        public List<PurchaseItem> PurchaseItems { get; set; }
        [NotMapped] public decimal TotalQty => PurchaseItems?.Sum(t => t.Quantity) ?? 0;

        [NotMapped]
        public decimal OrderTotal
        {
            get { return PurchaseItems != null ? PurchaseItems.Sum(t => t.Amount) : 0; }
        }
    }

    public class PurchaseItem
    {
        public int Id { get; set; }

        [ForeignKey("PurchaseId")] public Purchase Purchase { get; set; }
        public int PurchaseId { get; set; }

        public DateTime OrderDate { get; set; }

        [ForeignKey("ProductSupId")] public ProductSupplier ProductSupplier { get; set; }
        public int ProductSupId { get; set; }

        [ForeignKey("ProductId")] public Product Product { get; set; }
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal PurchasePrice { get; set; }

        [NotMapped]
        public decimal Amount
        {
            get { return Quantity * PurchasePrice; }
        }
    }

    public class PurchasePrice
    {
        public int Id { get; set; }

        [ForeignKey("ProductId")] public Product Product { get; set; }
        public int ProductId { get; set; }

        public DateTime PriceDate { get; set; }

        public decimal UpdatedPrice { get; set; }

        public decimal Percent { get; set; }
    }

    public class Receiving
    {
        public int Id { get; set; }

        [ForeignKey("PurchaseId")] public Purchase Purchase { get; set; }
        public int PurchaseId { get; set; }

        public string PurchaseNo { get; set; }

        [ForeignKey("SupplierId")] public Supplier Supplier { get; set; }
        public int SupplierId { get; set; }

        [ForeignKey("LocationId")] public Location Locations { get; set; }
        public int LocationId { get; set; }

        public DateTime ReceiveDate { get; set; }

        public string Status { get; set; }

        public string Notes { get; set; }

        public string Reason { get; set; }

        public List<ReceivingItem> ReceivingItems { get; set; }

        [NotMapped] public decimal TotalQty => ReceivingItems?.Sum(t => t.Quantity) ?? 0;

        [NotMapped]
        public decimal OrderTotal
        {
            get { return ReceivingItems != null ? ReceivingItems.Sum(t => t.Amount) : 0; }
        }
    }

    public class ReceivingItem
    {
        public int Id { get; set; }

        [ForeignKey("ReceiveId")] public Receiving Receiving { get; set; }
        public int ReceiveId { get; set; }

        [ForeignKey("ProductSupId")] public ProductSupplier ProductSupplier { get; set; }
        public int ProductSupId { get; set; }

        [ForeignKey("ProductId")] public Product Product { get; set; }
        public int? ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal PurchasePrice { get; set; }

        [NotMapped]
        public decimal Amount
        {
            get { return Quantity * PurchasePrice; }
        }
    }

    public class ReceiveApproveItem
    {
        public int Id { get; set; }

        [ForeignKey("ReceiveId")] public Receiving Receiving { get; set; }
        public int ReceiveId { get; set; }

        [ForeignKey("ProductId")] public Product Product { get; set; }
        public int? ProductId { get; set; }

        [ForeignKey("WarehouseId")] public ProductWarehouse ProductWarehouse { get; set; }
        public int? WarehouseId { get; set; }

        public int Quantity { get; set; }

        public decimal PurchasePrice { get; set; }

        [NotMapped]
        public decimal Amount
        {
            get { return Quantity * PurchasePrice; }
        }
    }

    public class PurchaseReturn
    {
        public int Id { get; set; }

        [ForeignKey("PurchaseId")] public Purchase Purchase { get; set; }
        public int PurchaseId { get; set; }

        [ForeignKey("SupplierId")] public Supplier Supplier { get; set; }
        public int SupplierId { get; set; }

        public DateTime ReturnDate { get; set; }

        public string Reason { get; set; }

        public string Status { get; set; }

        public List<PurchaseReturnItem> PurchaseReturnItems { get; set; }
    }

    public class PurchaseReturnItem
    {
        public int Id { get; set; }

        [ForeignKey("ReturnId")] public PurchaseReturn PurchaseReturn { get; set; }
        public int ReturnId { get; set; }

        [ForeignKey("ProductId")] public Product Product { get; set; }
        public int ProductId { get; set; }

        [ForeignKey("WarehouseId")] public ProductWarehouse ProductWarehouse { get; set; }
        public int WarehouseId { get; set; }

        public int Quantity { get; set; }

        public decimal PurchasePrice { get; set; }

        [NotMapped]
        public decimal Amount
        {
            get { return Quantity * PurchasePrice; }
        }
    }

    public class Supplier
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string PhoneNo { get; set; }

        public string Email { get; set; }

        public string Status { get; set; }
    }
}