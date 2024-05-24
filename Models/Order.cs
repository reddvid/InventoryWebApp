using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ASPNETWebApp48.Models
{
    public class Releasing
    {
        public int Id { get; set; }

        public string ClientName { get; set; }

        public DateTime OrderDate { get; set; }

        public string Notes { get; set; }

        public string Status { get; set; }

        public List<ReleaseItem> ReleaseItems { get; set; }

        [NotMapped]
        public decimal OrderTotal
        {
            get { return ReleaseItems != null ? ReleaseItems.Sum(t => t.Amount) : 0; }
        }
    }

    public class ReleaseItem
    {
        public int Id { get; set; }

        [ForeignKey("ReleaseId")] public Releasing Releasing { get; set; }
        public int ReleaseId { get; set; }

        [ForeignKey("ProductId")] public Product Product { get; set; }
        public int ProductId { get; set; }

        public DateTime PurchaseDate { get; set; }

        public int Quantity { get; set; }

        public decimal SellingPrice { get; set; }

        [NotMapped]
        public decimal Amount
        {
            get { return Quantity * SellingPrice; }
        }
    }

    public class ReleaseReturn
    {
        public int Id { get; set; }

        [ForeignKey("ReleaseId")] public Releasing Releasing { get; set; }
        public int ReleaseId { get; set; }

        public DateTime ReturnDate { get; set; }

        public string Reason { get; set; }

        public string Status { get; set; }

        public List<ReleaseReturnItem> ReleaseReturnItems { get; set; }
    }

    public class ReleaseReturnItem
    {
        public int Id { get; set; }

        [ForeignKey("ReturnId")] public ReleaseReturn ReleaseReturn { get; set; }
        public int ReturnId { get; set; }

        [ForeignKey("ProductId")] public Product Product { get; set; }
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal SellingPrice { get; set; }

        [NotMapped]
        public decimal Amount
        {
            get { return Quantity * SellingPrice; }
        }
    }

    public class ReleaseReturnItemApprove
    {
        public int Id { get; set; }

        [ForeignKey("ReturnId")] public ReleaseReturn ReleaseReturn { get; set; }
        public int ReturnId { get; set; }

        [ForeignKey("ProductId")] public Product Product { get; set; }
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal SellingPrice { get; set; }

        //[NotMapped]
        //public decimal Amount
        //{
        //    get { return Quantity * SellingPrice; }
        //}
    }

    public class TransferDetail
    {
        public int Id { get; set; }

        [ForeignKey("LocationId")] public Location Locations { get; set; }
        public int LocationId { get; set; }

        public DateTime TransferDate { get; set; }

        public string Notes { get; set; }

        public string Status { get; set; }

        public List<TransferItemStore> TransferItemStores { get; set; }
        public List<TransferItemWarehouse> TransferItemWarehouses { get; set; }
    }

    public class TransferItemStore
    {
        public int Id { get; set; }

        [ForeignKey("TransferId")] public TransferDetail TransferDetails { get; set; }
        public int TransferId { get; set; }

        [ForeignKey("ProductId")] public Product Product { get; set; }
        public int ProductId { get; set; }

        [ForeignKey("WarehouseId")] public ProductWarehouse ProductWarehouse { get; set; }
        public int WarehouseId { get; set; }

        public int Quantity { get; set; }
    }

    public class TransferItemWarehouse
    {
        public int Id { get; set; }

        [ForeignKey("TransferId")] public TransferDetail TransferDetails { get; set; }
        public int TransferId { get; set; }

        [ForeignKey("ProductId")] public Product Product { get; set; }
        public int ProductId { get; set; }

        [ForeignKey("WarehouseId")] public ProductWarehouse ProductWarehouse { get; set; }
        public int WarehouseId { get; set; }

        public int Quantity { get; set; }
    }
}