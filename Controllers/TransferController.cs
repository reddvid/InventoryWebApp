using ASPNETWebApp48.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

namespace ASPNETWebApp48.Controllers
{
    [Authorize]
    public class TransferController : Controller
    {
        InventoryDbContext _db = new InventoryDbContext();
        public ActionResult Manage()
        {
            if (User.IsInRole("admin") || User.IsInRole("wrhmgr") || User.IsInRole("salesmgr"))
            {
                var transfer = _db.TransferDetails
                    .Include(x => x.TransferItemStores)
                    .Include(x => x.TransferItemWarehouses)
                    //.Where(r => r.Status == "Not Transfer")
                    .ToList();
                
                ViewBag.TransferDate = DateTime.Now;
                ViewBag.ForCreatePartial = new Dictionary<string, object>
                {
                    { "LocLookup", _db.Locations.ToList()}
                };

                return View(transfer);
            }
            else if (User.IsInRole("purchmgr"))
                ViewBag.Message = "sales & warehouse manager can access stock transfer";

            return View();
        }

        [HttpPost]
        public ActionResult Create(TransferDetail transfer)
        {
            if (transfer.LocationId == 1)
            {
                _db.TransferDetails.Add(transfer);
                string username = Session["user"] as string;
                LogActivity(username, "Transfer Stock", "Create Main Store");
                _db.SaveChanges();
                return RedirectToAction("Get", new { Id = transfer.Id });
            }
            if (transfer.LocationId == 2)
            {
                _db.TransferDetails.Add(transfer);
                string username = Session["user"] as string;
                LogActivity(username, "Transfer Stock", "Create Warehouse");
                _db.SaveChanges();
                return RedirectToAction("GetWh", new { Id = transfer.Id });
            }
            return View();
        }

        public ActionResult Get(int Id) // Order.Id
        {
            if (User.IsInRole("admin") || User.IsInRole("wrhmgr") || User.IsInRole("salesmgr"))
            {
                var transfer = _db.TransferDetails
                    .Where(x => x.Id == Id)
                    .Include(x => x.TransferItemStores)
                    .Include(x => x.Locations)
                    .SingleOrDefault();

                if (transfer == null)
                {
                    TempData["alertbox"] = "Transfer Not Exist";
                    return RedirectToAction("Manage");
                }
                if (transfer.LocationId == 2)
                {
                    TempData["alertbox"] = "Cannot access this transfer from Warehouse";
                    return RedirectToAction("Manage");
                }

                var productMain = _db.Products
                    .Include(x => x.ReleaseItems)
                    .Include(x => x.TransferItemStores)
                    .Include(x => x.AdjustItems)
                    //.Include(x => x.RestockApproves)
                    .Include(x => x.PurchaseReturnItems)
                    .ToList();

                var productMainWithStock = productMain
                        .Where(x => x.StockOnHand > 0)
                        .ToList();

                ViewBag.ForCreatePartial = new Dictionary<string, object>
                {
                    { "ProductLookup", productMainWithStock },
                    { "TransferId", transfer.Id }
                };

                return View(transfer);
            }
            else if (User.IsInRole("purchmgr"))
                ViewBag.Message = "sales & warehouse manager can access stock transfer";

            return View();
        }

        public ActionResult GetWh(int Id) // Order.Id
        {
            if (User.IsInRole("admin") || User.IsInRole("wrhmgr") || User.IsInRole("salesmgr"))
            {
                var transfer = _db.TransferDetails
                    .Where(x => x.Id == Id)
                    .Include(x => x.TransferItemWarehouses)
                    .Include(x => x.Locations)
                    .SingleOrDefault();

                if (transfer == null)
                {
                    TempData["alertbox"] = "Transfer Not Exist";
                    return RedirectToAction("Manage");
                }
                if(transfer.LocationId == 1)
                {
                    TempData["alertbox"] = "Cannot access this transfer from Store";
                    return RedirectToAction("Manage");
                }

                var productWarehouses = _db.ProductWarehouses
                    .Include(x => x.ReceivingApproveItems)
                    //.Include(x => x.RestockApproves)
                    .Include(x => x.PurchaseReturnItems)
                    //.Include(x => x.WarehouseRestockApprove)
                    .Include(x => x.TransferItemStores)
                    .Include(x => x.TransferItemWarehouses)
                    .ToList();

                var productWarehousesWithStock = productWarehouses
                        .Where(x => x.StockOnHand > 0)
                        .ToList();

                ViewBag.ForCreatePartial2 = new Dictionary<string, object>
                {
                    { "ProductLookup", productWarehousesWithStock },
                    { "TransferId", transfer.Id }
                };

                return View(transfer);
            }
            else if (User.IsInRole("purchmgr"))
                ViewBag.Message = "sales & warehouse manager can access stock transfer";

            return View();
        }

        [HttpPost]
        public ActionResult CreateItem(TransferItemStore transferItem)
        {
            if (ModelState.IsValid)
            {
                var product = _db.Products
                    .Include(x => x.ReleaseItems)
                    .Include(x => x.TransferItemStores)
                    .Include(x => x.AdjustItems)
                    //.Include(x => x.RestockApproves)
                    .Include(x => x.PurchaseReturnItems)
                    .FirstOrDefault(x => x.Id == transferItem.ProductId);

                if (product != null)
                {
                    if (product.StockOnHand >= transferItem.Quantity && transferItem.Quantity > 0)
                    {
                        var existingWarehouse = _db.ProductWarehouses.FirstOrDefault(pw => pw.Name == product.Name);
                        transferItem.WarehouseId = existingWarehouse.Id;
                        string username = Session["user"] as string;
                        LogActivity(username, "Transfer Stock Main", "Create Item Main");
                        TransactionActivity("Transfer Stock Main", transferItem.Quantity);
                        _db.TransferItemStores.Add(transferItem);
                        _db.SaveChanges();
                        int createdItemId = transferItem.Id;
                        List<int> orderItemIds = Session["orderItemIds"] as List<int> ?? new List<int>();
                        orderItemIds.Add(createdItemId);
                        Session["orderItemIds"] = orderItemIds;
                        string orderItemIdsString = string.Join(", ", orderItemIds);
                    }
                    else if (transferItem.Quantity <= 0)
                        TempData["alertbox"] = "Please enter a positive quantity.";
                    else
                        TempData["alertbox"] = "There is insufficient stock for this product.";
                }
                else
                    TempData["alertbox"] = "Product not found.";
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("Get", new { Id = transferItem.TransferId });
        }

        [HttpPost]
        public ActionResult CreateItemWh(TransferItemWarehouse transferItem)
        {
            if (ModelState.IsValid)
            {
                var product = _db.ProductWarehouses
                    .Include(x => x.ReceivingApproveItems)
                    .Include(x => x.TransferItemStores)
                    //.Include(x => x.WarehouseRestockApprove)
                    .Include(x => x.TransferItemWarehouses)
                    //.Include(x => x.RestockApproves)
                    .Include(x => x.PurchaseReturnItems)
                    .Include(x => x.AdjustItems)
                    .FirstOrDefault(x => x.Id == transferItem.WarehouseId);

                if (product != null)
                {
                    if (product.StockOnHand >= transferItem.Quantity && transferItem.Quantity > 0)
                    {
                        var existingMain = _db.Products.FirstOrDefault(pw => pw.Name == product.Name);
                        transferItem.ProductId = existingMain.Id;
                        string username = Session["user"] as string;
                        LogActivity(username, "Transfer Stock Warehouse", "Add Item");
                        TransactionActivity("Transfer Stock Warehouse", transferItem.Quantity);
                        _db.TransferItemWarehouses.Add(transferItem);
                        _db.SaveChanges();
                        int createdItemId = transferItem.Id;
                        List<int> orderItemIds = Session["orderItemIds"] as List<int> ?? new List<int>();
                        orderItemIds.Add(createdItemId);
                        Session["orderItemIds"] = orderItemIds;
                        string orderItemIdsString = string.Join(", ", orderItemIds);
                    }
                    else if (transferItem.Quantity <= 0)
                        TempData["alertbox"] = "Please enter a positive quantity.";
                    else
                        TempData["alertbox"] = "There is insufficient stock for this product.";
                }
                else
                    TempData["alertbox"] = "Product not found.";
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("GetWh", new { Id = transferItem.TransferId });
        }

        public ActionResult DelItem(int id)
        {
            var orderItem = _db.TransferItemStores.Find(id);
            if (orderItem != null)
            {
                string username = Session["user"] as string;
                LogActivity(username, "Transfer Stock", "Delete Item Main");
                TransactionActivity("Transfer Stock Store - Delete Item", orderItem.Quantity);
                _db.TransferItemStores.Remove(orderItem);
                _db.SaveChanges();
            }

            return RedirectToAction("Get", new { Id = orderItem.TransferId });
        }

        public ActionResult DelItemWh(int id)
        {
            var orderItem = _db.TransferItemWarehouses.Find(id);
            if (orderItem != null)
            {
                string username = Session["user"] as string;
                LogActivity(username, "Transfer Stock", "Delete Item Wh");
                TransactionActivity("Transfer Stock Warehouse - Delete Item", orderItem.Quantity);
                _db.TransferItemWarehouses.Remove(orderItem);
                _db.SaveChanges();
            }

            return RedirectToAction("GetWh", new { Id = orderItem.TransferId });
        }

        public ActionResult Cancel()
        {
            // Retrieve order item IDs from the session, if available
            List<int> orderItemIds = Session["orderItemIds"] as List<int>;

            if (orderItemIds != null && orderItemIds.Any())
            {
                foreach (int itemId in orderItemIds)
                {
                    var orderItem = _db.TransferItemStores.Find(itemId);
                    if (orderItem != null)
                    {
                        TransactionActivity("Cancel Transfer Stock Store", orderItem.Quantity);
                        _db.TransferItemStores.Remove(orderItem);
                    }
                }

                string username = Session["user"] as string;
                LogActivity(username, "Transfer Stock", "Cancel Warehouse");
                Session.Remove("orderItemIds");
                _db.SaveChanges();
                TempData["alertbox"] = "transfer canceled." + Session["orderItemIds"];
            }
            else
                TempData["alertbox"] = "There are no transfer item to cancel." + Session["orderItemIds"];

            return RedirectToAction("Manage");
        }

        public ActionResult CancelWh()
        {
            // Retrieve order item IDs from the session, if available
            List<int> orderItemIds = Session["orderItemIds"] as List<int>;

            if (orderItemIds != null && orderItemIds.Any())
            {
                foreach (int itemId in orderItemIds)
                {
                    var orderItem = _db.TransferItemWarehouses.Find(itemId);
                    if (orderItem != null)
                    {
                        TransactionActivity("Cancel Transfer Stock Wh", orderItem.Quantity);
                        _db.TransferItemWarehouses.Remove(orderItem);
                    }
                }

                string username = Session["user"] as string;
                LogActivity(username, "Transfer Stock", "Cancel Warehouse");
                Session.Remove("orderItemIds");
                _db.SaveChanges();
                TempData["alertbox"] = "transfer canceled." + Session["orderItemIds"];
            }
            else
                TempData["alertbox"] = "There are no transfer item to cancel." + Session["orderItemIds"];

            return RedirectToAction("Manage");
        }

        [HttpPost]
        public ActionResult EditDetail(TransferDetail updatedOrder)
        {
            if (ModelState.IsValid)
            {
                if (Session["orderItemIds"] != null)
                {
                    // If orderItemIds exist in the session, log the activity
                    string username = Session["user"] as string;
                    LogActivity(username, "Transfer Stock", "Update detail");
                    _db.Entry(updatedOrder).State = EntityState.Modified;
                    _db.SaveChanges();
                }
                else
                {
                    // If orderItemIds do not exist in the session, update the order status
                    string username = Session["user"] as string;
                    LogActivity(username, "Transfer Stock", "Update detail");
                    _db.Entry(updatedOrder).State = EntityState.Modified;
                    _db.SaveChanges();
                }
                Session.Remove("orderItemIds");
                TempData["alertbox"] = "transfer changes have been successfully saved.";
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("Manage");
        }

        public ActionResult Print(int Id) //for pdf
        {
            var transfer = _db.TransferDetails
                .Where(x => x.Id == Id)
                .Include(x => x.TransferItemStores.Select(tis => tis.Product))
                .Include(x => x.TransferItemWarehouses.Select(tiw => tiw.ProductWarehouse))
                .Include(x => x.Locations)
                .SingleOrDefault();

            return new Rotativa.ViewAsPdf(transfer);
        }

        private void LogActivity(string username, string activity, string action)
        {
            // Create a new ActivityLog object
            ActivityLog log = new ActivityLog
            {
                Username = username,
                Activity = activity,
                Action = action,
                DateTime = DateTime.Now
            };

            // Add the log to your database
            _db.ActivityLogs.Add(log);
            _db.SaveChanges();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        private void TransactionActivity(string activity, int qty)
        {
            var products = _db.Products
                .Include(x => x.ReleaseItems)
                .Include(x => x.TransferItemStores)
                .Include(x => x.AdjustItems)
                //.Include(x => x.RestockApproves)
                .Include(x => x.PurchaseReturnItems)
                .Include(x => x.TransferItemWarehouses)
                .ToList();

            var productwhs = _db.ProductWarehouses
                //.Include(x => x.RestockApproves)
                //.Include(x => x.WarehouseRestockApprove)
                .Include(x => x.ReceivingApproveItems)
                .Include(x => x.PurchaseReturnItems)
                .Include(x => x.TransferItemStores)
                .Include(x => x.TransferItemWarehouses)
                .Include(x => x.AdjustItems)
                .ToList();

            // Sum the stock quantities of all products
            decimal totalStockOnHandmain = products.Sum(x => x.StockOnHand);
            decimal totalStockOnHandWh = productwhs.Sum(x => x.StockOnHand);

            // Create a new ActivityLog object
            TransactionLodge log = new TransactionLodge
            {
                Activity = activity,
                Quantity = qty,
                Date = DateTime.Now,
                CurrentStoreQty = totalStockOnHandmain, // Assuming CurrentStoreQty is a decimal representing the total stock on hand
                CurrentWarehouseQty = totalStockOnHandWh
            };

            // Add the log to your database
            _db.TransactionLodges.Add(log);
            _db.SaveChanges();
        }
    }
}