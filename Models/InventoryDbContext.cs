using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace ASPNETWebApp48.Models
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext() : base("LocalDb") // name_of_dbconnection_string
        {
        }

        // Map model classes to database tables
        public DbSet<UserAccount> Users { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<TransactionLodge> TransactionLodges { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Adjustment> Adjustments { get; set; }
        public DbSet<AdjustItem> AdjustItems { get; set; }
        public DbSet<PurchasePrice> PurchasePrices { get; set; }
        public DbSet<ProductWarehouse> ProductWarehouses { get; set; }
        public DbSet<ProductSupplier> ProductSuppliers { get; set; }

        //releasing entities
        public DbSet<Releasing> Releasings { get; set; }
        public DbSet<ReleaseItem> ReleaseItems { get; set; }
        public DbSet<ReleaseReturn> ReleaseReturns { get; set; }
        public DbSet<ReleaseReturnItem> ReleaseReturnItems { get; set; }
        public DbSet<ReleaseReturnItemApprove> ReleaseReturnItemApproves { get; set; }

        //purchase entities
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseItem> PurchaseItems { get; set; }
        public DbSet<Receiving> Receivings { get; set; }
        public DbSet<ReceivingItem> ReceivingItems { get; set; }
        public DbSet<ReceiveApproveItem> ReceiveApproveItems { get; set; }
        public DbSet<PurchaseReturn> PurchaseReturns { get; set; }
        public DbSet<PurchaseReturnItem> PurchaseReturnItems { get; set; }
        public DbSet<TransferDetail> TransferDetails { get; set; }
        public DbSet<TransferItemStore> TransferItemStores { get; set; }
        public DbSet<TransferItemWarehouse> TransferItemWarehouses { get; set; }
    }
}