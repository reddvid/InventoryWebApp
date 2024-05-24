using ASPNETWebApp48.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ASPNETWebApp48.Controllers
{
    [Authorize]
    public class AdjustController : Controller
    {
        InventoryDbContext _db = new InventoryDbContext();
        public ActionResult Manage()
        {
            if (User.IsInRole("admin") || User.IsInRole("purchmgr"))
            {
                var adjustments = _db.Adjustments
                    .OrderByDescending(x => x.Id)
                    .ToList();

                ViewBag.AdjustmentDate = DateTime.Now;

                ViewBag.ForCreatePartial = new Dictionary<string, object>
                {
                    { "LocLookup", _db.Locations.ToList()}
                };

                return View(adjustments);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "admin & purchase manager can access adjustment";

            return View();
        }

        [HttpPost]
        public ActionResult Create(Adjustment adjustment)
        {
            if(adjustment.LocationId == 1)
            {
                _db.Adjustments.Add(adjustment);
                string username = Session["user"] as string;
                LogActivity(username, "Adjustment", "Create Main Store");
                _db.SaveChanges();
                return RedirectToAction("Get", new { Id = adjustment.Id });
            }
            if (adjustment.LocationId == 2)
            {
                _db.Adjustments.Add(adjustment);
                string username = Session["user"] as string;
                LogActivity(username, "Adjustment", "Create Warehouse");
                _db.SaveChanges();
                return RedirectToAction("GetWh", new { Id = adjustment.Id });
            }
            return View();
        }

        public ActionResult Get(int Id) // Order.Id
        {
            if (User.IsInRole("admin") || User.IsInRole("purchmgr"))
            {
                var adjust = _db.Adjustments
                    .Where(x => x.Id == Id)
                    .Include(x => x.AdjustItems)
                    .Include(x => x.Locations)
                    .SingleOrDefault();

                var prod = _db.Products
                    .Include(x => x.ReleaseItems)
                    .Include(x => x.TransferItemStores)
                    .Include(x => x.TransferItemWarehouses)
                    .Include(x => x.AdjustItems)
                    .ToList();

                if (adjust == null)
                {
                    TempData["alertbox"] = "Adjust Not Exist";
                    return RedirectToAction("Manage");
                }

                ViewBag.ForCreatePartial = new Dictionary<string, object>
                {
                    { "ProductLookup", _db.Products.ToList() },
                    { "AdjustmentId", adjust.Id }
                };

                return View(adjust);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "admin & purchase manager can access adjustment";

            return View();
        }

        public ActionResult GetWh(int Id) // Order.Id
        {
            if (User.IsInRole("admin") || User.IsInRole("purchmgr"))
            {
                var adjust = _db.Adjustments
                    .Where(x => x.Id == Id)
                    .Include(x => x.AdjustItems)
                    .Include(x => x.Locations)
                    .SingleOrDefault();

                var product = _db.ProductWarehouses
                    .Include(x => x.ReceivingApproveItems)
                    .Include(x => x.PurchaseReturnItems)
                    .Include(x => x.TransferItemStores)
                    .Include(x => x.TransferItemWarehouses)
                    .Include(x => x.AdjustItems)
                    .ToList();

                if (adjust == null)
                {
                    TempData["alertbox"] = "Adjust Not Exist";
                    return RedirectToAction("Manage");
                }

                ViewBag.ForCreatePartial2 = new Dictionary<string, object>
                {
                    { "ProductLookup", product },
                    { "AdjustmentId", adjust.Id }
                };

                return View(adjust);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "admin & purchase manager can access adjustment";

            return View();
        }

        [HttpPost]
        public ActionResult CreateItem(AdjustItem adjustItem)
        {
            if (ModelState.IsValid)
            {
                string username = Session["user"] as string;
                LogActivity(username, "Adjustment", "Add Item Main");
                TransactionActivity("Adjustment Add", adjustItem.QtyAdd);
                TransactionActivity("Adjustment Deduct", adjustItem.QtyDeduct);
                adjustItem.WarehouseId = null;
                _db.AdjustItems.Add(adjustItem);
                _db.SaveChanges();
                int createdItemId = adjustItem.Id;
                List<int> orderItemIds = Session["orderItemIds"] as List<int> ?? new List<int>();
                orderItemIds.Add(createdItemId);
                Session["orderItemIds"] = orderItemIds;
                string orderItemIdsString = string.Join(", ", orderItemIds);
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("Get", new { Id = adjustItem.AdjustmentId });
        }

        [HttpPost]
        public ActionResult CreateItemWh(AdjustItem adjustItem)
        {
            if (ModelState.IsValid)
            {
                string username = Session["user"] as string;
                LogActivity(username, "Adjustment", "Create Item Wh");
                TransactionActivity("Adjustment Add", adjustItem.QtyAdd);
                TransactionActivity("Adjustment Deduct", adjustItem.QtyDeduct);
                adjustItem.ProductId = null;
                _db.AdjustItems.Add(adjustItem);
                _db.SaveChanges();
                int createdItemId = adjustItem.Id;
                List<int> orderItemIds = Session["orderItemIds"] as List<int> ?? new List<int>();
                orderItemIds.Add(createdItemId);
                Session["orderItemIds"] = orderItemIds;
                string orderItemIdsString = string.Join(", ", orderItemIds);
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("GetWh", new { Id = adjustItem.AdjustmentId });
        }

        public ActionResult DelItem(int id)
        {
            var orderItem = _db.AdjustItems.Find(id);
            if (orderItem != null)
            {
                string username = Session["user"] as string;
                LogActivity(username, "Adjustment", "Delete Item");
                TransactionActivity("Adjustment Delete", orderItem.QtyAdd);
                TransactionActivity("Adjustment Delete", orderItem.QtyDeduct);
                _db.AdjustItems.Remove(orderItem);
                _db.SaveChanges();
            }

            return RedirectToAction("Get", new { Id = orderItem.AdjustmentId });
        }

        public ActionResult DelItemWh(int id)
        {
            var orderItem = _db.AdjustItems.Find(id);
            if (orderItem != null)
            {
                string username = Session["user"] as string;
                LogActivity(username, "Adjustment", "Delete Item");
                TransactionActivity("Adjustment Delete", orderItem.QtyAdd);
                TransactionActivity("Adjustment Delete", orderItem.QtyDeduct);
                _db.AdjustItems.Remove(orderItem);
                _db.SaveChanges();
            }

            return RedirectToAction("GetWh", new { Id = orderItem.AdjustmentId });
        }

        public ActionResult Cancel()
        {
            // Retrieve order item IDs from the session, if available
            List<int> orderItemIds = Session["orderItemIds"] as List<int>;

            if (orderItemIds != null && orderItemIds.Any())
            {
                foreach (int itemId in orderItemIds)
                {
                    var orderItem = _db.AdjustItems.Find(itemId);
                    if (orderItem != null)
                    {
                        string username = Session["user"] as string;
                        LogActivity(username, "Adjustment", "Cancel Item");
                        TransactionActivity("Adjustment Cancel", orderItem.QtyAdd);
                        TransactionActivity("Adjustment Cancel", orderItem.QtyDeduct);
                        _db.AdjustItems.Remove(orderItem);
                    }
                }

                Session.Remove("orderItemIds");
                _db.SaveChanges();
                TempData["alertbox"] = "adjustment canceled." + Session["orderItemIds"];
            }
            else
                TempData["alertbox"] = "There are no adjustment item to cancel." + Session["orderItemIds"];

            return RedirectToAction("Manage");
        }

        [HttpPost]
        public ActionResult EditDetail(Adjustment updatedOrder)
        {
            if (ModelState.IsValid)
            {
                if (Session["orderItemIds"] != null)
                {
                    // If orderItemIds exist in the session, log the activity
                    string username = Session["user"] as string;
                    LogActivity(username, "Adjustment", "Update Detail");
                    _db.Entry(updatedOrder).State = EntityState.Modified;
                    _db.SaveChanges();
                }
                else
                {
                    // If orderItemIds do not exist in the session, update the order status
                    _db.Entry(updatedOrder).State = EntityState.Modified;
                    _db.SaveChanges();
                }
                Session.Remove("orderItemIds");
                TempData["alertbox"] = "adjustment changes have been successfully saved.";
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("Manage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                .Include(x => x.PurchaseReturnItems)
                .Include(x => x.TransferItemWarehouses)
                .ToList();

            var productwhs = _db.ProductWarehouses
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